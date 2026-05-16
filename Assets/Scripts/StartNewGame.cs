using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour
{
    [SerializeField] private Button NormalModeButton;
    [SerializeField] private Button HardModeButton;
    [SerializeField] private Button RejectNewGameButton;

    private void Start()
    {
        NormalModeButton.onClick.AddListener(() => ConfirmNewGameClick(false));
        HardModeButton.onClick.AddListener(() => ConfirmNewGameClick(true));
        RejectNewGameButton.onClick.AddListener(RejectNewGameClick);
    }

    private void ConfirmNewGameClick(bool isHardMode)
    {
        SoundManager.Instance.StopAllSounds();
        EventBus.Raise(EventType.ResetGameState);
        GameState gameState = ObjectResolver.Resolve<GameState>();
        gameState.isHardMode = isHardMode;
        SceneManager.LoadScene(gameState.currentLevel);
        gameObject.SetActive(false);
    }

    private void RejectNewGameClick()
    {
        gameObject.SetActive(false);
    }
}
