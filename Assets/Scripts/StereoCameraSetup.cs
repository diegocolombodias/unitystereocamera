using UnityEngine;

public class StereoCameraSetup : MonoBehaviour
{
    public Camera leftCamera;
    public Camera rightCamera;
    public float ipd = 0.065f; // Distância interpupilar padrão

    [Tooltip("Ative para forçar a renderização simultânea e evitar flicks.")]
    public bool forceManualRender = true;

    void Start()
    {
        // Sincroniza os parâmetros da câmera direita a partir da câmera esquerda
        rightCamera.CopyFrom(leftCamera);

        // Define a posição inicial e a projeção
        UpdateStereoSetup();

        // Usa o sistema de projeção estéreo nativo do Unity (para VR nativo)
        leftCamera.stereoTargetEye = StereoTargetEyeMask.Left;
        rightCamera.stereoTargetEye = StereoTargetEyeMask.Right;

        // Caso não esteja usando VR nativo, preparamos a renderização manual
        if (!UnityEngine.XR.XRSettings.enabled && forceManualRender)
        {
            // Desativa a renderização automática no loop padrão da Unity
            leftCamera.enabled = false;
            rightCamera.enabled = false;
        }
    }

    void UpdateStereoSetup()
    {
        // Atualiza as posições
        leftCamera.transform.localPosition = new Vector3(-ipd / 2, 0, 0);
        rightCamera.transform.localPosition = new Vector3(ipd / 2, 0, 0);

        // Atualiza as matrizes de projeção (necessário sempre que a IPD mudar)
        if (!UnityEngine.XR.XRSettings.enabled)
        {
            SetStereoProjection(leftCamera, -ipd / 2);
            SetStereoProjection(rightCamera, ipd / 2);
        }
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

    // Mudado para LateUpdate para garantir que a cena inteira já se moveu neste frame
    void LateUpdate()
    {
        bool ipdChanged = false;

        // Permite que o usuário ajuste a IPD durante o runtime
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ipd += 0.001f;
            ipdChanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ipd -= 0.001f;
            ipdChanged = true;
        }

        // Se a IPD mudou, atualiza posição e projeção imediatamente
        if (ipdChanged)
        {
            UpdateStereoSetup();
        }

        // A BARREIRA: Força a renderização sequencial estrita no exato mesmo momento do frame
        if (!UnityEngine.XR.XRSettings.enabled && forceManualRender)
        {
            leftCamera.Render();
            rightCamera.Render();
        }
    }
}
