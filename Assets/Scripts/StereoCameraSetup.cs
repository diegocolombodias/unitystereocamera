using UnityEngine;

public class StereoCameraSetup : MonoBehaviour
{
    public Camera leftCamera;
    public Camera rightCamera;
    public float ipd = 0.065f; // Distância interpupilar padrão

    void Start()
    {
        // Definir as posições das câmeras
        leftCamera.transform.localPosition = new Vector3(-ipd / 2, 0, 0);
        rightCamera.transform.localPosition = new Vector3(ipd / 2, 0, 0);

        // Definir as matrizes de projeção
        SetStereoProjection(leftCamera, -ipd / 2);
        SetStereoProjection(rightCamera, ipd / 2);
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
}
