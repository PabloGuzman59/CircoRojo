using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencias")]
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;
    public GameObject gameOverPanel;

    [Header("Configuración")]
    public VideoClip gameOverVideo;
    public float delayBeforeRestart = 1f;

    private bool isPlayingGameOver = false;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;

            if (videoPlayer.targetTexture == null)
            {
                RenderTexture rt = new RenderTexture(1920, 1080, 0);
                videoPlayer.targetTexture = rt;
                if (videoDisplay != null)
                {
                    videoDisplay.texture = rt;
                }
            }

            videoPlayer.loopPointReached += OnVideoFinished;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TriggerGameOver()
    {
        if (isPlayingGameOver) return;

        isPlayingGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (videoPlayer != null && gameOverVideo != null)
        {
            videoPlayer.clip = gameOverVideo;
            videoPlayer.Play();

            if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
            {
                videoPlayer.SetDirectAudioVolume(0, 1f);
            }
        }
        else
        {
            Debug.LogWarning("Video no configurado. Reiniciando...");
            RestartGame();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Invoke(nameof(RestartGame), delayBeforeRestart);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}