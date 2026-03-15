using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class Avatar : MonoBehaviour
{
    [SerializeField] float speed;

    private Rigidbody rb;
    private NavMeshAgent agent;

    private Vector3 moveDir;
    private Vector3 moveDest; //destination for enemy, teleport for player
    [SerializeField] private Vector3 resetPoint;
    private string animBoolName;

    private int id;
    private bool isMoveDirty;
    private bool isMoveToDirty;
    private bool isTeleportDirty;
    private bool isResetPosDirty;
    private bool isSetAnimationDirty;

    private void Start()
    {
        if (GetComponent<Rigidbody>() != null) 
        { 
            rb = GetComponent<Rigidbody>();
        }
        if (GetComponent<NavMeshAgent>() != null) 
        { 
            agent = GetComponent<NavMeshAgent>();
        }
    }
    public void SetID(int newId)
    {
        id = newId;
    }

    public void Move(Vector3 direction)
    {
        moveDir = direction;
        isMoveDirty = true;
    }

    public void MoveTo(Vector3 destination)
    {
        moveDest = destination;
        isMoveToDirty = true;
    }

    public void Teleport(Vector3 destination)
    {
        moveDest = destination;
        isTeleportDirty = true;
    }

    public void ResetPos()
    {
        isResetPosDirty = true;
    }

    public void SetAnimation(string animBoolName)
    {
        this.animBoolName = animBoolName;   
        isSetAnimationDirty = true;
    }

    private void Update()
    {
        if (isMoveDirty) 
        {
            rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
            EventBus.Raise<(int, Transform)>(EventType.UpdatePosition, (id, transform));
            
            if (moveDir == Vector3.zero)
            {
                isMoveDirty = false;
            }
        }

        if (isMoveToDirty) 
        {
            agent.ResetPath();
            agent.SetDestination(moveDest);
            EventBus.Raise<(int, Transform)>(EventType.UpdatePosition, (id, transform));

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoveToDirty = false;
            }
        }

        if (isTeleportDirty)
        {
            transform.position = moveDest;
            EventBus.Raise<(int, Transform)>(EventType.UpdatePosition, (id, transform));
            isTeleportDirty = false;
        }

        if (isResetPosDirty)
        {
            transform.position = resetPoint;
            EventBus.Raise<(int, Transform)>(EventType.UpdatePosition, (id, transform));
            isResetPosDirty = false;
        }

        if (isSetAnimationDirty)
        {

            isSetAnimationDirty = false;
        }
    }
}
