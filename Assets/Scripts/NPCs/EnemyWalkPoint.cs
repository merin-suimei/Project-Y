using UnityEngine;

public class EnemyWalkPoint : MonoBehaviour
{
    [field:SerializeField] public float waitTime {  get; private set; }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}
