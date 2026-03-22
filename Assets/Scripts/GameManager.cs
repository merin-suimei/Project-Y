using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Avatar player;
    public Enemy[] enemies;

    private Dictionary<int, Avatar> _avatarsDict = new();

    public List<IModel> models = new();

    private void Awake()
    {
        instance = this;

        if (player == null)
        {
            Debug.LogError("Player not assigned in GameManager");

            #if UNITY_EDITOR
                EditorApplication.isPaused = true;
            #endif

            return;
        }

        models.Add(new DetectionService(player, enemies));

        player.SetID(0);
        models.Add(new PlayerModel(0));
        _avatarsDict.Add(0, player);

        int nextId = 1;
        foreach (Enemy enemy in enemies)
        {
            enemy.SetID(nextId);
            models.Add(new EnemyModel(nextId, enemy.type, enemy.EnemyWalkPoints, enemy.IsPatrolPathClosed));
            _avatarsDict.Add(nextId, enemy);
            nextId++;
        }
    }

    private void Start()
    {
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, ResetAllPos);

        // EventBus.Subscribe<int, bool>(EventType.OnObjectToggle, HandleInteractiveObject);
    }

    private void FixedUpdate()
    {
        foreach (IModel model in models)
            model.Tick();
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, ResetAllPos);       
    }

    private void ResetAllPos()
    {
        player.ResetPos();

        foreach (Avatar enemy in enemies)
            enemy.ResetPos();
    }

    private void HandleInteractiveObject(int id, bool state)
    {
        //TODO
    }


    public Coroutine ProxyStartCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void ProxyStopCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }
}
