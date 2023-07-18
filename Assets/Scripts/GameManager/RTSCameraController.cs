using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float cameraSpeed = 5f;
    public float boundaryOffset = 20f;

    void Update()
    {
        // Obtém a posição atual do mouse em relação à tela
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        // Obtém os limites da tela
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Movimento da câmera na horizontal
        if (mouseX < boundaryOffset)
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }
        else if (mouseX > screenWidth - boundaryOffset)
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }

        // Movimento da câmera na vertical
        if (mouseY < boundaryOffset)
        {
            transform.Translate(Vector3.back * cameraSpeed * Time.deltaTime);
        }
        else if (mouseY > screenHeight - boundaryOffset)
        {
            transform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime);
        }
    }
}
