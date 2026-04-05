using UnityEngine;
using UnityEngine.SceneManagement; // Esta línea es la que permite cambiar de escenas

public class MenuPrincipal : MonoBehaviour
{
    // Este método lo llamará el botón Jugar
    public void Jugar()
    {
        // Carga la escena que sigue en el orden (la escena 1, que es tu juego)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Este método lo llamará el botón Salir
    public void Salir()
    {
        Debug.Log("El jugador ha salido del juego..."); // Solo para ver en consola que funciona
        Application.Quit(); // Cierra la aplicación
    }
}