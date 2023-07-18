using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float cameraSpeed = 5f;
    public float boundaryOffset = 20f;

    void Update()
    {
        // Obt�m a posi��o atual do mouse em rela��o � tela
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        // Obt�m os limites da tela
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Movimento da c�mera na horizontal
        if (mouseX < boundaryOffset)
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }
        else if (mouseX > screenWidth - boundaryOffset)
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }

        // Movimento da c�mera na vertical
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
