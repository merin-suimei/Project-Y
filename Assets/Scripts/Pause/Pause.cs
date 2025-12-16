using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private InputsTypes _input;
    [Header("Scene")]
    [SerializeField]
    private SceneField mainMenu;
    public GameObject pauseMenuUI;
    private GameObject firstSelectedButton;

    private bool isPaused = false;

    private void Awake()
    {
        _input = new InputsTypes();

    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        _input.Player.Pause.performed -= OnPause;
        _input.Disable();
    }

    void Start()
    {
        if (EventSystem.current != null)
        {
            firstSelectedButton = EventSystem.current.firstSelectedGameObject;
        }

        Resume();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (!isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        var es = EventSystem.current;
        if (es == null) return;

        es.SetSelectedGameObject(null);
        if (firstSelectedButton != null)
        {
            es.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void Continue()
    {
        Resume();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void Exit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
