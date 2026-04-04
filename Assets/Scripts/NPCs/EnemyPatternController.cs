using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyPatternController : MonoBehaviour
{
    [SerializeField] private DecalProjector patternProj;
    [SerializeField] private Material instanceMat;
    [SerializeField] private GameObject fillPattern;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();

        fillPattern.SetActive(false);
        instanceMat = new Material(patternProj.material.shader);
        instanceMat.CopyPropertiesFromMaterial(patternProj.material);

        patternProj.material = instanceMat;

        instanceMat.SetFloat("_FillAmount", 0f);

        EventBus.Subscribe<int, float>(EventType.OnEnemyDetect, StartAnimation);
        EventBus.Subscribe<int, float>(EventType.OnEnemyLoseAim, ReverseAnimtion);
        EventBus.Subscribe<int, bool>(EventType.EnableEnemyPattern, EnablePattern);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<int, float>(EventType.OnEnemyDetect, StartAnimation);
        EventBus.Unsubscribe<int, float>(EventType.OnEnemyLoseAim, ReverseAnimtion);
        EventBus.Unsubscribe<int, bool>(EventType.EnableEnemyPattern, EnablePattern);
    }

    private void StartAnimation(int sender, float detectProgress)
    {
        if (sender != enemy.ID) return;

            float normalizedElapsedTime = Mathf.Clamp01(detectProgress / enemy.detectDelay); 
            instanceMat.SetFloat("_FillAmount", normalizedElapsedTime);
    }

    private void ReverseAnimtion(int sender, float detectProgress)
    {
        if (sender != enemy.ID) return;
        
            float normalizedElapsedTime = Mathf.Clamp01(detectProgress / enemy.detectDelay);
            instanceMat.SetFloat("_FillAmount", normalizedElapsedTime);
    }

    private void EnablePattern(int sender, bool value)
    {
        if (sender != enemy.ID) return;

        fillPattern.SetActive(value);
        instanceMat.SetFloat("_FillAmount", 0f);
    }
}
