using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public EnemyWalker enemy;

    public EnemyWalkPoint[] enemyWalkPoints;

    [SerializeField] private Vector3 startPoint;

    private Coroutine gameLoopCoroutine;

    private IPlayerInput _input;
    private void Awake()
    {
        instance = this;
        _input = new InputSystemListener();
        ObjectResolver.RegisterInstance<IPlayerInput>(_input);
    }

    private void Start()
    {
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, TeleportToStartPoint);
    }

    public void Run()
    {
       if (gameLoopCoroutine == null)
       {
            gameLoopCoroutine = StartCoroutine(GameLoop());
       }
    }

    private IEnumerator GameLoop()
    {
        while (true) {
            Debug.Log("Run");
            yield return null;
        
        }
    }

    private void TeleportToStartPoint()
    {
        player.rb.transform.position = startPoint;
       // enemy.stateMachine.ChangeState(enemy.patrolState);
    }
}
