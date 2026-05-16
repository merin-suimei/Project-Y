using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CreditSceneManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneField mainMenu;

    [SerializeField] private SoundDataSO cutSceneSoundData;
    [SerializeField] private SoundDataSO titreSoundData;

    [SerializeField] private RawImage cutSceneRawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private GameObject creditsPopUp;

    private SoundEmitter cutSceneEmitter;
    private SoundEmitter creditsEmitter;

    private InputsTypes _input;

    private void Awake()
    {
        _input = new InputsTypes();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.UI.Exit.performed += OnExit;
    }

    private void OnDisable()
    {
        _input.UI.Exit.performed -= OnExit;
        _input.Disable();
    }

    private void Start()
    {
        creditsPopUp.gameObject.SetActive(false);
        cutSceneEmitter = SoundManager.Instance.Get().Initialize(cutSceneSoundData);
        cutSceneEmitter.Play();
        videoPlayer.loopPointReached += OnCutSceneEnd;
    }

    private void OnCutSceneEnd(VideoPlayer source)
    {
        cutSceneEmitter.Stop();
        SoundManager.Instance.Get().Initialize(titreSoundData).Play();
        cutSceneRawImage.gameObject.SetActive(false);
        creditsPopUp.SetActive(true);
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
        EndFinal();
    }

    public void EndFinal(){

        SoundManager.Instance.StopAllSounds();
        EventBus.Raise(EventType.ResetGameState);
        SceneManager.LoadScene(mainMenu);
    }
    
}
