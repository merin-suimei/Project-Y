using UnityEngine;

public class EnemyWalkPoint : MonoBehaviour
{
    [field:SerializeField] public float waitTime { get; private set; }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, 360f, 0.25f);
    }
#endif
}
