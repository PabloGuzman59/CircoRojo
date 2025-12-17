using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VictoryManager : MonoBehaviour
{
    [Header("Referencias")]
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;
    public GameObject victoryPanel;

    [Header("Configuración")]
    public VideoClip victoryVideo;
    public string menuSceneName = "MainMenu";
    public float delayBeforeMenu = 1f;

    private bool isPlayingVictory = false;

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

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void ShowVictory()
    {
        if (isPlayingVictory) return;

        isPlayingVictory = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (videoPlayer != null && victoryVideo != null)
        {
            videoPlayer.clip = victoryVideo;
            videoPlayer.Play();

            if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
            {
                videoPlayer.SetDirectAudioVolume(0, 1f);
            }
        }
        else
        {
            Debug.LogWarning("Video no configurado. Cargando menú...");
            LoadMenu();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Invoke(nameof(LoadMenu), delayBeforeMenu);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}