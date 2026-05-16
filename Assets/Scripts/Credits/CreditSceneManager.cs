using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CreditSceneManager : MonoBehaviour
{
    [SerializeField] private SoundDataSO cutSceneSoundData;
    [SerializeField] private SoundDataSO titreSoundData;

    [SerializeField] private RawImage cutSceneRawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private GameObject creditsPopUp;

    private SoundEmitter cutSceneEmitter;
    private SoundEmitter creditsEmitter;
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
}
