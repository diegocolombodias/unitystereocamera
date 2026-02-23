using UnityEngine;

public class StereoMultiDisplaySetup : MonoBehaviour
{
    public Camera leftCamera;
    public Camera rightCamera;

    [Header("Configurações de Hardware")]
    public float ipd = 0.065f;
    public int refreshRate = 60; // Ajuste para a frequência do seu monitor (Ex: 60, 120, 144)

    void Awake()
    {
        // 1. ATIVAÇÃO DOS MONITORES
        // No Unity, Display[0] é o monitor principal. Display[1] é o secundário.
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            // Opcional: Forçar resolução nativa do monitor secundário
            // Display.displays[1].SetRenderingResolution(1920, 1080);
        }
        else
        {
            Debug.LogWarning("Segundo monitor não detectado!");
        }

        // 2. CONFIGURAÇÃO DE TARGET DISPLAY
        leftCamera.targetDisplay = 0;  // Monitor 1
        rightCamera.targetDisplay = 1; // Monitor 2
        
        // Garante que não há interferência de VR nativo
        leftCamera.stereoTargetEye = StereoTargetEyeMask.None;
        rightCamera.stereoTargetEye = StereoTargetEyeMask.None;

        // 3. SINCRONIZAÇÃO DE FRAME RATE
        // Essencial para evitar que uma janela "corra" mais que a outra
        Application.targetFrameRate = refreshRate;
        QualitySettings.vSyncCount = 1; // Força V-Sync em todos os buffers de saída
    }

    void Start()
    {
        UpdateStereoSetup();
    }

    void LateUpdate()
    {
        bool changed = false;

        // Teclas + e - (considerando layout ABNT e Teclado Numérico)
        if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals))
        {
            ipd += 0.0005f;
            changed = true;
        }
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
        {
            ipd -= 0.0005f;
            changed = true;
        }

        if (changed) UpdateStereoSetup();
    }

    void UpdateStereoSetup()
    {
        // Posicionamento
        leftCamera.transform.localPosition = new Vector3(-ipd / 2, 0, 0);
        rightCamera.transform.localPosition = new Vector3(ipd / 2, 0, 0);

        // Matrizes de Projeção Assimétrica
        SetStereoProjection(leftCamera, -ipd / 2);
        SetStereoProjection(rightCamera, ipd / 2);
    }

    void SetStereoProjection(Camera cam, float shift)
    {
        Matrix4x4 proj = cam.projectionMatrix;
        float w = 2 * cam.nearClipPlane / proj.m00;
        float h = 2 * cam.nearClipPlane / proj.m11;

        float left = -w / 2 - shift;
        float right = w / 2 - shift;
        float top = h / 2;
        float bottom = -h / 2;

        proj[0, 2] = (right + left) / (right - left);
        proj[1, 2] = (top + bottom) / (top - bottom);

        cam.projectionMatrix = proj;
    }
}
