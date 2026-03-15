using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour
{
    [SerializeField] private Button ConfirmNewGameButton;
    [SerializeField] private Button RejectNewGameButton;

    private void Start()
    {
        gameObject.SetActive(false);
        ConfirmNewGameButton.onClick.AddListener(ConfirmNewGameClick);
        RejectNewGameButton.onClick.AddListener (RejectNewGameClick);
    }

    private void ConfirmNewGameClick()
    {
        EventBus.Raise(EventType.ResetGameState);
        GameState gameState = ObjectResolver.Resolve<GameState>();
        SceneManager.LoadScene(gameState.currentLevel);
        gameObject.SetActive(false);
    }

    private void RejectNewGameClick()
    {
        gameObject.SetActive(false);
    }
}
