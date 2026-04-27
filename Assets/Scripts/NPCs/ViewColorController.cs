using UnityEngine;

public class ViewColorController : MonoBehaviour
{
    //[SerializeField] MeshRenderer viewMeshRenderer;
    [Header("Mesh settings")]
    [SerializeField] Color chaseColor;
    [SerializeField] bool useCustomChaseTransparancy;
    [SerializeField] float customChaseTransparency;
    [Header("Lines settings")]
    [SerializeField] bool changeLineColor;
    [SerializeField] bool useCustomChaseLineColor;
    [SerializeField] Color customChaseLineColor;
    private MeshRenderer viewMeshRenderer;
    private Material viewMaterial;
    private Color idleColor;
    private Color lineIdleColor;
    private Color chaseLineColor;
    private float idleTransparency;
    private EnemyConeView coneView;
    private float fillTime;

    private Enemy enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.Log("ViewColorController: Enemy not assigned");
            return;
        }
        coneView = GetComponentInChildren<EnemyConeView>();
        if (coneView == null || coneView.viewMeshFilter == null)
        {
            Debug.Log("ViewColorController: EnemyConeView or viewMeshFilter not assigned");
            return;
        }
        viewMeshRenderer = coneView.viewMeshFilter.GetComponent<MeshRenderer>();
        viewMaterial = viewMeshRenderer.material;
        idleColor = viewMaterial.color;
        if (useCustomChaseTransparancy)
        {
            idleTransparency = viewMaterial.GetFloat("_Transparency");
        }
        if (changeLineColor)
        {
            lineIdleColor = coneView.EdgeLineMaterial.color;
            if (useCustomChaseLineColor)
                chaseLineColor = customChaseLineColor;
            else
            {
                chaseLineColor = chaseColor;
                chaseLineColor.a = lineIdleColor.a;
            }
        }
        fillTime = enemy.detectDelay;

        EventBus.Subscribe<int, float>(EventType.OnEnemyLoseAim, SetDetectProgress);
        EventBus.Subscribe(EventType.OnEnemyCatchPlayer, ResetProgress);
        //EventBus.Subscribe<float>(EventType.OnEnemyDetect, (sender) => { if (sender == this.enemy) StartChangeColor; });
 
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe<int, float>(EventType.OnEnemyLoseAim, SetDetectProgress);
        EventBus.Unsubscribe(EventType.OnEnemyCatchPlayer, ResetProgress);
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
        if (useCustomChaseTransparancy)
        {
            float currentTransparancy = Mathf.Lerp(idleTransparency, customChaseTransparency, normalizedElapsedTime);
            viewMaterial.SetFloat("_Transparency", currentTransparancy);
        }

        if (changeLineColor)
        {
            Color currentLineColor = Color.Lerp(lineIdleColor, chaseLineColor, normalizedElapsedTime);
            coneView.SetLinesColor(currentLineColor);
        }
    }

    private void ResetProgress() {
        if (viewMeshRenderer == null || enemy == null)
        {
            return;
        }
        viewMaterial.color = idleColor;
        if (useCustomChaseTransparancy)
        {
            viewMaterial.SetFloat("_Transparency", idleTransparency);
        }
        if (changeLineColor)
        {
            coneView.SetLinesColor(lineIdleColor);
        }
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
