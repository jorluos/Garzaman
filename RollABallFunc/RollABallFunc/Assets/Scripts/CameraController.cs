using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Arrastra tu esfera aquí
    private Vector3 offset;

    void Start()
    {
        // Calculamos la distancia inicial entre la cámara y el jugador
        offset = transform.position - player.transform.position;
    }

    void LateUpdate() // Se ejecuta después de que el jugador se haya movido
    {
        // Sigue al jugador manteniendo el mismo offset
        transform.position = player.transform.position + offset;
    }
}