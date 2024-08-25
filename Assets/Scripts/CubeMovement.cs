using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        // Obter o input do teclado
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Criar o vetor de movimento baseado no input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Aplicar o movimento ao cubo
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
