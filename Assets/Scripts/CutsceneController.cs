using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CutsceneController : MonoBehaviour
{
    [Header("Настройки")]
    public VideoPlayer videoPlayer;
    public GameObject cutsceneScreen;
    public string sceneName;
    
    private InputsTypes _input;

    private void Awake()
    {
        _input = ObjectResolver.Resolve<InputsTypes>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.UI.SkipAction.performed += OnSkipPerformed;
    }

    private void OnDisable()
    {
        _input.UI.SkipAction.Disable();
        _input.UI.SkipAction.performed -= OnSkipPerformed;
    }

    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void StartCutscene()
    {
        if (cutsceneScreen != null)
            cutsceneScreen.SetActive(true);

        videoPlayer.Play();
    }

    public void StartCutscene(string nextScene)
    {
        sceneName = nextScene;
        StartCutscene();
    }

    private void OnSkipPerformed(InputAction.CallbackContext ctx)
    {
        if (videoPlayer.isPlaying)
        {
            SkipCutscene();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        LoadGame();
    }

    private void SkipCutscene()
    {
        videoPlayer.Stop();
        LoadGame();
    }

    private void LoadGame()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        if (sceneName == null || sceneName == "")
        {
            Debug.LogError("Scene name is not set for CutsceneController.");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}