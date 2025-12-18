using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencias")]
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;
    public GameObject gameOverPanel;
    [Header("VR Camera")]
    public Camera vrCamera; // Arrastra aquí la cámara de XR Origin
    [Header("Configuración")]
    public VideoClip gameOverVideo;
    public float delayBeforeRestart = 1f;
    private bool isPlayingGameOver = false;
    private Canvas canvas;

    void Start()
    {
        // Obtener el Canvas
        canvas = gameOverPanel.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            // Configurar Canvas para VR
            canvas.renderMode = RenderMode.WorldSpace;
            // Si no hay cámara asignada, buscarla automáticamente
            if (vrCamera == null)
            {
                vrCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
                if (vrCamera == null)
                {
                    vrCamera = Camera.main;
                }
            }
            // Posicionar el canvas frente a la cámara VR
            if (vrCamera != null)
            {
                canvas.worldCamera = vrCamera;
            }
        }
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
        // Posicionar el canvas frente a la cámara VR
        if (canvas != null && vrCamera != null)
        {
            // Colocar el canvas 2 metros frente a la cámara
            Vector3 cameraPos = vrCamera.transform.position;
            Vector3 cameraForward = vrCamera.transform.forward;
            canvas.transform.position = cameraPos + cameraForward * 2f;
            canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - cameraPos);
            // Escalar el canvas para que se vea bien en VR
            canvas.transform.localScale = Vector3.one * 0.001f;
        }
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