using UnityEngine;

public class CamaraMenu : MonoBehaviour
{
    public float velocidadGiro = 10f; // Velocidad de giro de la cámara
    public Transform puntoCentral; // El punto alrededor del cual la cámara girará

    void Update() {
        // Girar la cámara alrededor del eje Y
        // Vector3.up (Eje Y - Verde): Gira como un carrusel o un trompo (hacia los lados).
        // Vector3.right (Eje X - Rojo): Gira como una rueda de la fortuna o haciendo "volteretas" (hacia adelante/atrás).
        // Vector3.forward (Eje Z - Azul): Gira como las manecillas de un reloj (hacia los costados)
        // transform.Rotate(Vector3.forward, velocidadGiro * Time.deltaTime);


        // Gira alrededor del punto central, en el eje Y, a cierta velocidad
        transform.RotateAround(puntoCentral.position, Vector3.up, velocidadGiro * Time.deltaTime);

        // Esto hace que la cámara siempre mire hacia el centro mientras gira
        transform.LookAt(puntoCentral);
    }
}
