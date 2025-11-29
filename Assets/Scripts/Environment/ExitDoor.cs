using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("The scene to load when the player enters the door.")]
    [SerializeField] private SceneField sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошел именно Игрок
        if (other.CompareTag("Player"))
        {
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

        SceneManager.LoadScene(sceneToLoad);
    }
}