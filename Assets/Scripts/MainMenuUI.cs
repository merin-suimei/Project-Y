using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button StartNewGameButton;
    [SerializeField] private Button continuePlayButton;
    [SerializeField] private Button authorsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsButton;

    [Space(10)]
    [Header("Сцены")]
    [Tooltip("Перед тем как перетащить сцену, добавьте её в File → Build Profiles → Scene List.")]
    [SerializeField] private SceneField mainMenuScene;
    [SerializeField] private SceneField gameScene;
    [SerializeField] private SceneField creditsScene;

    [SerializeField] private GameObject SettingsPopupUI;
    [SerializeField] private GameObject StartNewGameConfirmationPopup;

    private void Awake()
    {
        StartNewGameButton.onClick.AddListener(StartNewGameClick);
        continuePlayButton.onClick.AddListener(ContinueClick);
        authorsButton.onClick.AddListener(AuthorsClick);
        settingsButton.onClick.AddListener(SettingsClick);
        exitButton.onClick.AddListener(ExitClick);
    }

    private void OnEnable()
    {
        var es = EventSystem.current;
        if (es == null) return;

        es.SetSelectedGameObject(null);

        Debug.Log($"FirstSelected: {es?.firstSelectedGameObject?.name}");
        Debug.Log($"CurrentSelected: {es?.currentSelectedGameObject?.name}");

        if (es.firstSelectedGameObject != null)
            es.SetSelectedGameObject(es.firstSelectedGameObject);
    }

    private void StartNewGameClick()
    {
        StartNewGameConfirmationPopup.gameObject.SetActive(true);

    }

    private void ContinueClick()
    {
        GameState gameState = ObjectResolver.Resolve<GameState>();
        SceneManager.LoadScene(gameState.currentLevel);
    }
    private void AuthorsClick()
    {
        if (creditsScene == null)
        {
            Debug.Log("subtitleScene не установлена в инспекторе");
            return;
        }
        SceneManager.LoadScene(creditsScene);
    }

    private void SettingsClick()
    {
        SettingsPopupUI.SetActive(true);
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
