using UnityEngine;

// Garante que este script rode por último, após o movimento do mouse
[DefaultExecutionOrder(9999)]
public class StereoCameraSetup : MonoBehaviour
{
    public Transform pivot; // Arraste o CameraPivot aqui

    void ApplyStereoProjection(Camera cam, float offset)
    {
        Matrix4x4 m = cam.projectionMatrix;
        float w = 2 * cam.nearClipPlane / m.m00;
        float h = 2 * cam.nearClipPlane / m.m11;
        float left = -w / 2 - offset;
        float right = w / 2 - offset;
        m[0, 2] = (right + left) / (right - left);
        cam.projectionMatrix = m;
    }
}
