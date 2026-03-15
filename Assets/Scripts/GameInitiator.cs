using UnityEngine;
using UnityEngine.SceneManagement;


public class GameInitiator : MonoBehaviour
{
    [Header("InputSystem")]
    private IPlayerInput _input;

    [Space(10)]
    [Header("Сцены")]
    [Tooltip("Перед тем как перетащить сцену, добавьте её в File → Build Profiles → Scene List.")]
    [SerializeField] private SceneField mainMenuScene;
    [SerializeField] private SceneField gameScene;
    [SerializeField] private SceneField creditsScene;
    GameManager gameManager;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(mainMenuScene);

        _input = new InputSystemListener();
        //_input = gameObject.AddComponent<InputSystemListener()>();
        //gameManager = gameObject.AddComponent<GameManager>();
        ObjectResolver.RegisterInstance<IPlayerInput>(_input);

        //EventBus.Subscribe(EventType.OnClickPlay, PlayClickLogic);
        //EventBus.Subscribe(EventType.OnClickAuthors, AuthorsClickLogic);
    }

    private void PlayClickLogic()
    {
        if (gameScene == null)
        {
            Debug.Log("gameScene не установлена в инспекторе");
            return;
        }
        SceneManager.LoadScene(gameScene);
        //gameManager.Run();
        
        
    }

    private void AuthorsClickLogic()
    {
        if (creditsScene == null)
        {
            Debug.Log("subtitleScene не установлена в инспекторе");
            return;
        }
        SceneManager.LoadScene(creditsScene);
    }


}
