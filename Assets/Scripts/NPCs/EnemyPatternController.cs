using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyPatternController : MonoBehaviour
{
    [SerializeField] private DecalProjector patternProj;
    [SerializeField] private Material instanceMat;
    [SerializeField] GameObject fillPattern;
    [SerializeField] Enemy enemy;
    private float fillTime;

    private void Awake()
    {
        
    }
    void Start()
    {
        fillPattern.SetActive(false);
        instanceMat = new Material(patternProj.material.shader);
        instanceMat.CopyPropertiesFromMaterial(patternProj.material);

        patternProj.material = instanceMat;
        fillTime = enemy.detectDelay;

        instanceMat.SetFloat("_FillAmount", 0f);

        EventBus.Subscribe<float>(EventType.OnEnemyDetect, StartAnimation);
        EventBus.Subscribe<float>(EventType.OnEnemyLoseAim, ReverseAnimtion);
        EventBus.Subscribe<Enemy>(EventType.TurnOnEnemyPattern, (sender) => { if (sender == this.enemy) fillPattern.SetActive(true); instanceMat.SetFloat("_FillAmount", 0f); });
        EventBus.Subscribe<Enemy>(EventType.TurnOffEnemyPattern, (sender) => { if (sender == this.enemy) fillPattern.SetActive(false); instanceMat.SetFloat("_FillAmount", 0f); });
    }

    
    private void StartAnimation(float detectProgress)
    {
            float normalizedElapsedTime = Mathf.Clamp01(detectProgress / fillTime); 
            instanceMat.SetFloat("_FillAmount", normalizedElapsedTime);
    }
    private void ReverseAnimtion(float detectProgress)
    {
            float normalizedElapsedTime = Mathf.Clamp01(detectProgress / fillTime);
            instanceMat.SetFloat("_FillAmount", normalizedElapsedTime);
    }

}
