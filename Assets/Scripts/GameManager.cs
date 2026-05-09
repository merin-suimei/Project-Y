using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Avatar player;
    public Enemy[] enemies;
    //public BabaYaga babaYaga;


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

        DetectionService detectionService = new DetectionService(player, enemies);
        models.Add(detectionService);
        /*DebugSceneSwitcher debugSceneSwitcher = ObjectResolver.Resolve<DebugSceneSwitcher>();
        if (debugSceneSwitcher != null)
        {
            debugSceneSwitcher.InitDetectionService(detectionService);
        }*/

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
        //babaYaga.SetID(nextId);
        models.Add(new BabaYagaModel(nextId));
        nextId++;
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

        foreach (IModel model in models) // TODO: Проверить необходимость Destroy() для остальных моделей
            if (model is EnemyModel enemy) enemy.Destroy();
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
