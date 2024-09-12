using UnityEngine;

public class StereoCameraSetup : MonoBehaviour
{
    public Camera leftCamera;
    public Camera rightCamera;
    public float ipd = 0.065f; // Distância interpupilar padrão

    void Start()
    {
        // Sincroniza os parâmetros da câmera direita a partir da câmera esquerda
        rightCamera.CopyFrom(leftCamera);

        // Define a posição das câmeras com base na IPD
        UpdateCameraPositions();

        // Usa o sistema de projeção estéreo nativo do Unity
        leftCamera.stereoTargetEye = StereoTargetEyeMask.Left;
        rightCamera.stereoTargetEye = StereoTargetEyeMask.Right;

        // Caso não esteja usando VR, ajusta a matriz de projeção manualmente
        if (!XR.XRSettings.enabled)
        {
            SetStereoProjection(leftCamera, -ipd / 2);
            SetStereoProjection(rightCamera, ipd / 2);
        }
    }

    void UpdateCameraPositions()
    {
        // Atualiza as posições das câmeras para refletir a IPD
        leftCamera.transform.localPosition = new Vector3(-ipd / 2, 0, 0);
        rightCamera.transform.localPosition = new Vector3(ipd / 2, 0, 0);
    }

    void SetStereoProjection(Camera cam, float shift)
    {
        Matrix4x4 proj = cam.projectionMatrix;
        float w = 2 * cam.nearClipPlane / proj.m00;
        float h = 2 * cam.nearClipPlane / proj.m11;
        float left = -w / 2 + shift;
        float right = left + w;
        float top = h / 2;
        float bottom = -top;

        proj[0, 2] = (right + left) / (right - left);
        proj[1, 2] = (top + bottom) / (top - bottom);

        cam.projectionMatrix = proj;
    }

    void Update()
    {
        // Permite que o usuário ajuste a IPD durante o runtime, se necessário
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ipd += 0.001f;
            UpdateCameraPositions();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ipd -= 0.001f;
            UpdateCameraPositions();
        }
    }
}
