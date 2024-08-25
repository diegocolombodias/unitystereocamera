using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float rotationSpeed = 15.0f;

    void Update()
    {
        // Calcular o ângulo de rotação baseado no tempo
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // Rotacionar o cubo em torno do eixo Y
        transform.Rotate(0, rotationAngle, 0);
    }
}
