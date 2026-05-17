using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class CreditSceneManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneField mainMenu;

    //[SerializeField] private SoundDataSO cutSceneSoundData;
    [SerializeField] private SoundDataSO titreSoundData;

    [SerializeField] private RawImage cutSceneRawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private GameObject creditsPopUp;

    //private SoundEmitter cutSceneEmitter;
    //private SoundEmitter creditsEmitter;

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
        videoPlayer.loopPointReached += OnCutSceneEnd;
        GameState gameState = ObjectResolver.Resolve<GameState>();
        if (gameState.currentLevel == "CreditsScene")
        {
            StartCutScene();
        }
        else
        {
            StartCredits();
        }
    }

    private void StartCutScene()
    {
        if (cutSceneRawImage != null)
            cutSceneRawImage.gameObject.SetActive(true);
        creditsPopUp.gameObject.SetActive(false);
       // cutSceneEmitter = SoundManager.Instance.Get().Initialize(cutSceneSoundData);
        //cutSceneEmitter.Play();
        videoPlayer.Play();
    }

    private void StartCredits()
    {
        SoundManager.Instance.Get().Initialize(titreSoundData).Play();
        creditsPopUp.SetActive(true);
    }

    private void OnCutSceneEnd(VideoPlayer source)
    {
       // cutSceneEmitter.Stop();
        videoPlayer.Stop();
        cutSceneRawImage.gameObject.SetActive(false);
        StartCredits();
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
