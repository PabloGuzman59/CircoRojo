using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PlayerHealthVR : MonoBehaviour
{
    public bool isDead = false;

    [Header("Game Over Video")]
    public VideoPlayer gameOverVideo;

    void Start()
    {
        if (gameOverVideo != null)
        {
            gameOverVideo.loopPointReached += OnVideoFinished;
        }
    }

    public void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("GAME OVER — El jugador ha muerto.");

        // Pausa el juego
        Time.timeScale = 0f;

        // El VideoPlayer funciona con tiempo no escalado
        if (gameOverVideo != null)
        {
            gameOverVideo.Play();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video Game Over terminado. Reiniciando juego...");

        // Volvemos el tiempo a normal
        Time.timeScale = 1f;

        // Reiniciar escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
