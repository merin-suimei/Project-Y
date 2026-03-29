using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BabkaGameEndTest : MonoBehaviour
{
    [SerializeField] GameObject gameEndMenu;
    [SerializeField] Button exitButton;
    [SerializeField] SceneField mainMenuScene;
    private Canvas canvas;
    private VideoPlayer vidPlayer;
    private void Start()
    {
        gameEndMenu.SetActive(false);
        vidPlayer = GetComponent<VideoPlayer>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
        vidPlayer.Stop();
        vidPlayer.time = 0;
        vidPlayer.Prepare();

        EventBus.Subscribe(EventType.OnTimerIsUP, PlayGameEndVideo);

        vidPlayer.loopPointReached += OnGameEndVideoEnd;
        exitButton.onClick.AddListener(() => { SceneManager.LoadScene(mainMenuScene); });
    }
    private void PlayGameEndVideo()
    {
        vidPlayer.Stop();
        vidPlayer.time = 0;
        vidPlayer.Prepare();
        vidPlayer.Play();
        canvas.gameObject.SetActive(true);  
    }
    private void OnGameEndVideoEnd(VideoPlayer source)
    {
        vidPlayer?.Stop();
        gameEndMenu.SetActive(true);
    }
}
