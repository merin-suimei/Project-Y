using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private InputsTypes _input;
    [Header("Scene")]
    [SerializeField]
    private SceneField mainMenu;
    public GameObject pauseMenuUI;

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
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
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
