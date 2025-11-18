using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuScript : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button authorsButton;
    [SerializeField] private Button exitButton;

    [Space(10)]
    [Header("Сцены")]
    [Tooltip("Перед тем как перетащить сцену, добавьте её в File → Build Profiles → Scene List.")]
    [SerializeField] public SceneAsset gameScene;
    [SerializeField] public SceneAsset subtitleScene;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayClick);
        authorsButton.onClick.AddListener(AuthorsClick);
        exitButton.onClick.AddListener(ExitClick);
    }

    private void PlayClick()
    {
        if (gameScene == null)
        {
            Debug.Log("gameScene не установлена в инспекторе");
            return;
        }
        SceneManager.LoadScene(gameScene.name);
    }

    private void AuthorsClick()
    {
        if (subtitleScene == null)
        {
            Debug.Log("subtitleScene не установлена в инспекторе");
            return;
        }
        SceneManager.LoadScene(subtitleScene.name);
    }

    private void ExitClick()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
