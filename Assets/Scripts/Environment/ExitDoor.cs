using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private SoundDataSO levelTransitionSoundData;
    [Header("Transition Settings")]
    [Tooltip("The scene to load when the player enters the door.")]
    [SerializeField] private SceneField sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //EventBus.Raise(EventType.StopAllSound);
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (sceneToLoad == null || string.IsNullOrEmpty(sceneToLoad.SceneName))
        {
            Debug.LogError($"Scene to load is not set on {gameObject.name}!");
            return;
        }

        GameState gameState = ObjectResolver.Resolve<GameState>();
        gameState.currentLevel = sceneToLoad;
        //SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.Get().Initialize(levelTransitionSoundData).Play();
        SceneManager.LoadScene(sceneToLoad);
    }
}