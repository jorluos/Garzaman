using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnDelay = 3.0f; // Tiempo de espera en segundos

    void Start()
    {
        // Iniciamos la cuenta regresiva al empezar el juego
        StartCoroutine(AppearEnemy());
    }

    IEnumerator AppearEnemy()
    {
        // Esperamos la cantidad de segundos que definiste
        yield return new WaitForSeconds(spawnDelay);

        // Activamos al enemigo
        if (enemy != null)
        {
            enemy.SetActive(true);
        }
    }
}