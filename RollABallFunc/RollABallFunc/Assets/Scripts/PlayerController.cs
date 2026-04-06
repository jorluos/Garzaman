using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public AudioSource musicSource;
    public GameObject mainCamera;
    public AudioClip collectSound;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioClip wallHitSound;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private AudioSource playerAudio;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public GameObject explosionFX;
    public GameObject collectFX;

    public GameObject gameOverVideo;
    public VideoPlayer videoPlayer;
    public GameObject hud;

    private bool isGameOver = false;

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        if (gameOverVideo != null)
            gameOverVideo.SetActive(false);

        if (videoPlayer != null){
            videoPlayer.Stop();
            videoPlayer.SetDirectAudioMute(0, true); // 🔇 MUTE al inicio
            videoPlayer.enabled = false;   // <- clave
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.gameObject.CompareTag("PickUp"))
        {
            Instantiate(collectFX, other.transform.position + Vector3.up * 0.5f, Quaternion.identity);

            playerAudio.PlayOneShot(collectSound, 0.4f);
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 17)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, 1.0f);

            if (musicSource != null)
            {
                musicSource.Stop();
            }

            StartCoroutine(VolverAlMenu(10));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isGameOver) return;
            isGameOver = true;

            Instantiate(explosionFX, collision.contacts[0].point, Quaternion.identity);

            if (musicSource != null)
            {
                musicSource.Stop();
            }

            AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, 1.0f);

            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (hud != null)
                hud.SetActive(false);
            else
                countText.gameObject.SetActive(false);

            if (gameOverVideo != null)
                gameOverVideo.SetActive(true);

            if (videoPlayer != null){
                videoPlayer.enabled = true;
                videoPlayer.SetDirectAudioMute(0, false); // 🔊 activamos audio
                videoPlayer.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            AudioSource.PlayClipAtPoint(wallHitSound, Camera.main.transform.position, 1.0f);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    IEnumerator VolverAlMenu(int segundos)
    {
        yield return new WaitForSeconds(segundos);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}