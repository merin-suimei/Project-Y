using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyType : ScriptableObject
{
    [field: Header("Область обнаружения")]
    [field: SerializeField] public float DetectionRange { get; private set; } = 5f;
    [field: SerializeField] public float NearbyDetectionRange { get; private set; } = 2f;
    [field: Range(0f, 90f)]
    [field: SerializeField] public float DetectionSemiconeAngle { get; private set; } = 30f;
    [field: SerializeField] public float CatchThreshold { get; private set; } = 1f;

    [field: Header("Параметры обнаружения")]
    [field: SerializeField] public float DetectDelay { get; private set; } = 1f;
    [field: SerializeField] public float DecaySpeed { get; private set; } = 1f;
    [field: SerializeField] public LayerMask RaycastIgnore  { get; private set; }
    [field: SerializeField] public bool NeedsLightToDetect  { get; private set; } = false;

    [field: Space(10)]
    [field: Header("Движение")]
    [field: SerializeField] public float PatrolSpeed { get; private set; } = 3f;
    [field: SerializeField] public float ChaseSpeed { get; private set; } = 20f;
    [field: SerializeField] public float TurnSpeed { get; private set; } = 4f;
}
