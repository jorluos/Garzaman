using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;  // Importante para usar TextMeshPro



public class PlayerController : MonoBehaviour
{
    public AudioSource musicSource; // Referencia a la fuente de audio para la música de fondo
    public GameObject mainCamera; // Referencia a la cámara principal
    public AudioClip collectSound; // Sonido que se reproducirá al recoger un cubito
    public AudioClip loseSound; // Sonido que se reproducirá al perder
    public AudioClip winSound; // Sonido que se reproducirá al ganar
    public AudioClip wallHitSound;
    public float speed = 0;
    public TextMeshProUGUI countText; // Referencia al texto del contador
    public GameObject winTextObject;  // Referencia al objeto de victoria

    private AudioSource playerAudio;
    private Rigidbody rb;
    private int count;  // Variable para contar los objetos recogidos
    private float movementX;
    private float movementY;

    public GameObject explosionFX;
    public GameObject collectFX;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        count = 0; // Empezamos en cero
        SetCountText();
        winTextObject.SetActive(false); // Nos aseguramos que el mensaje de ganar esté oculto al inicio
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other) {
        // Si chocamos con algo que tiene el tag "PickUp"
        if (other.gameObject.CompareTag("PickUp")) {

            Instantiate(collectFX, other.transform.position + Vector3.up * 0.5f, Quaternion.identity);

            playerAudio.PlayOneShot(collectSound, 0.4f); // Reproducimos el sonido de recogida
            // Desactivamos el objeto para simular que lo hemos recogido
            other.gameObject.SetActive(false);
            count++; // Sumamos uno al contador
            SetCountText(); 
        }
    }
    void SetCountText() {
        countText.text = "Count: " + count.ToString();
        // Si recogimos todos los cubitos GANAMOS
        if (count >= 17) {
            winTextObject.SetActive(true); // Mostramos el mensaje de victoria
            Destroy(GameObject.FindGameObjectWithTag("Enemy")); // Destruimos al enemigo
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, 1.0f); // Reproducimos el sonido de victoria
            if (musicSource != null)
            {
                musicSource.Stop(); // Detenemos la música de fondo
            }
            StartCoroutine(VolverAlMenu(10)); // Volvemos al menú principal después de 10 segundos
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Si el objeto que chocamos tiene el tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy")) {
            Instantiate(explosionFX, collision.contacts[0].point, Quaternion.identity);

            if (musicSource != null) {
                musicSource.Stop(); // Detenemos la música de fondo
            }
            
            AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, 1.0f);
            // Desactivamos al jugador
            // gameObject.SetActive(false);
            GetComponent<Collider>().enabled = false; // Desactivamos el collider para evitar más colisiones
            GetComponent<MeshRenderer>().enabled = false; // Desactivamos el mesh renderer para que el jugador no sea visible

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false; // Le quitas la gravedad
            rb.linearVelocity = Vector3.zero; // La frenas en seco
            rb.angularVelocity = Vector3.zero; // Evitas que siga rotando

            // Mostramos el mensaje de derrota
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            // Volvemos al menú principal después de x segundos
            StartCoroutine(VolverAlMenu(3));
        } else if (collision.gameObject.CompareTag("Wall")) {
            AudioSource.PlayClipAtPoint(wallHitSound, Camera.main.transform.position, 1.0f);
        }
    }

    IEnumerator VolverAlMenu(int segundos) {
        yield return new WaitForSeconds(segundos); // Esperamos los segundos especificados
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Volvemos al menú principal (escena 0)
    }
}
