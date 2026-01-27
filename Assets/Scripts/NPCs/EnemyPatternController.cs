using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterPatternController : MonoBehaviour
{
    [SerializeField] private DecalProjector patternProj;
    [SerializeField] private Material instanceMat;

    [SerializeField] private float fillTime = 1.5f;

    void Start()
    {
        instanceMat = new Material(patternProj.material);
        patternProj.material = instanceMat;

        instanceMat.SetFloat("_FillAmount", 0f);

        EventBus.Subscribe<float>(EventType.OnEnemyDetect, StartAnimation);
        EventBus.Subscribe<float>(EventType.OnEnemyLoseAim, ReverseAnimtion);
    }


    private void StartAnimation(float detectProgress)
    {
            float normalizedElapsedTime = Mathf.Clamp01(detectProgress / fillTime); 
            instanceMat.SetFloat("_FillAmount", normalizedElapsedTime);
            Debug.Log(normalizedElapsedTime);
    }
    private void ReverseAnimtion(float detectProgress)
    {
            float normalizedElapsedTime = Mathf.Clamp01(detectProgress / fillTime);
            instanceMat.SetFloat("_FillAmount", normalizedElapsedTime);
    }

}
