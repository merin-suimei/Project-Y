using UnityEngine;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    [SerializeField] float timer;
    private VideoPlayer player;

    private float animMultiplyer;
    private void Start()
    {
        player = GetComponent<VideoPlayer>();

        animMultiplyer = ((float)player.clip.length -3.0f)/ timer;
        player.playbackSpeed = animMultiplyer;

        player.loopPointReached += OnVideoEnd;
        player.Play();
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        player.Stop();
        player.time = 0;
        player.gameObject.SetActive(false);
        EventBus.Raise(EventType.OnTimerIsUP);

    }
}
