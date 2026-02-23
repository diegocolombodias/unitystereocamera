using UnityEngine;

public class StereoCameraSetup : MonoBehaviour
{
    public Camera leftCamera;
    public Camera rightCamera;
    
    [Header("Configurações Estéreo")]
    public float ipd = 0.065f; // Distância interpupilar
    public float sensitivity = 0.001f;

    void Start()
    {
        if (leftCamera == null || rightCamera == null)
        {
            Debug.LogError("Atribua as câmeras no Inspetor!");
            return;
        }

        ConfigureCameras();
        UpdateStereoSetup();
    }

    void ConfigureCameras()
    {
        // Copia configurações base
        rightCamera.CopyFrom(leftCamera);

        // A MÁGICA CONTRA O FLICK: Viewport Rects
        // Divide a tela exatamente ao meio para renderização síncrona
        leftCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
        rightCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);

        // Garante que o Unity gerencie o loop de renderização (evita dessincronia de CPU/GPU)
        leftCamera.enabled = true;
        rightCamera.enabled = true;

        // Desativa VR nativo se estiver tentando fazer manual
        leftCamera.stereoTargetEye = StereoTargetEyeMask.None;
        rightCamera.stereoTargetEye = StereoTargetEyeMask.None;
    }

    void LateUpdate()
    {
        bool changed = false;

        // Uso das teclas + e - para ajuste (Conforme conversamos antes)
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals))
        {
            ipd += sensitivity;
            changed = true;
        }
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            ipd -= sensitivity;
            changed = true;
        }

        if (changed)
        {
            UpdateStereoSetup();
        }
    }

    void UpdateStereoSetup()
    {
        // 1. Posicionamento físico das câmeras
        leftCamera.transform.localPosition = new Vector3(-ipd / 2, 0, 0);
        rightCamera.transform.localPosition = new Vector3(ipd / 2, 0, 0);

        // 2. Ajuste das Matrizes de Projeção para Projeção Assimétrica (Off-axis)
        // Isso evita a convergência ocular forçada e o cansaço visual
        SetStereoProjection(leftCamera, -ipd / 2);
        SetStereoProjection(rightCamera, ipd / 2);
    }

    void SetStereoProjection(Camera cam, float shift)
    {
        Matrix4x4 proj = cam.projectionMatrix;
        
        // Cálculo da frustum baseado no Near Clip Plane
        // Usando LaTeX para referência: $$ w = \frac{2 \cdot near}{m00} $$
        float w = 2 * cam.nearClipPlane / proj.m00;
        float h = 2 * cam.nearClipPlane / proj.m11;
        
        float left = -w / 2 - shift;
        float right = w / 2 - shift;
        float top = h / 2;
        float bottom = -h / 2;

        // Ajuste dos coeficientes da matriz para o deslocamento lateral
        proj[0, 2] = (right + left) / (right - left);
        proj[1, 2] = (top + bottom) / (top - bottom);

        cam.projectionMatrix = proj;
    }
}
