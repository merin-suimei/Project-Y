using UnityEngine;

public class ViewColorController : MonoBehaviour
{
    //[SerializeField] MeshRenderer viewMeshRenderer;
    [SerializeField] Color chaseColor;
    private MeshRenderer viewMeshRenderer;
    private Material viewMaterial;
    private Color idleColor;
    private float fillTime;

    private Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        viewMeshRenderer = GetComponent<MeshRenderer>();
        enemy = GetComponentInParent<Enemy>();
        if (viewMeshRenderer == null || enemy == null)
        {
            Debug.Log("Enemy or View Mesh Renderer not assigned");
            return;
        }
        viewMaterial = viewMeshRenderer.material;
        idleColor = viewMaterial.color;
        chaseColor.a = idleColor.a;
        fillTime = enemy.detectDelay;

        EventBus.Subscribe<int, float>(EventType.OnEnemyLoseAim, SetDetectProgress);
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, ResetProgress);
        //EventBus.Subscribe<float>(EventType.OnEnemyDetect, (sender) => { if (sender == this.enemy) StartChangeColor; });
        //EventBus.Subscribe<Enemy>(EventType.TurnOnEnemyPattern, (sender) => { if (sender == this.enemy) viewMaterial.color = idleColor; });
        //EventBus.Subscribe<Enemy>(EventType.TurnOffEnemyPattern, (sender) => { if (sender == this.enemy) viewMaterial.color = idleColor; });
    }

    void OnDestroy()
    {
        //EventBus.Unsubscribe<int, float>(EventType.OnEnemyDetect, StartAnimation);
        EventBus.Unsubscribe<int, float>(EventType.OnEnemyLoseAim, SetDetectProgress);
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, ResetProgress);
        //EventBus.Unsubscribe<int, bool>(EventType.EnableEnemyPattern, EnablePattern);
    }

    private void SetDetectProgress(int targetID, float detectProgress)
    {
        if (targetID != enemy.ID) return;

        if (viewMeshRenderer == null || enemy == null)
        {
            return;
        }

        float normalizedElapsedTime = Mathf.Clamp01(detectProgress / fillTime);
        Color currentColor = Color.Lerp(idleColor, chaseColor, normalizedElapsedTime);
        viewMaterial.color = currentColor;
    }

    private void ResetProgress() {
        if (viewMeshRenderer == null || enemy == null)
        {
            return;
        }
        viewMaterial.color = idleColor;
    }
    /*
    private void ReverseChangeColor(float detectProgress)
    {
        float normalizedElapsedTime = Mathf.Clamp01(detectProgress / fillTime);
        Color currentColor = Color.Lerp(idleColor, chaseColor, normalizedElapsedTime);
        viewMaterial.color = currentColor;
    }
    */

}
