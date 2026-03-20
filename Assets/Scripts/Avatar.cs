using UnityEngine;
using UnityEngine.AI;

public class Avatar : MonoBehaviour
{
    private Rigidbody rb;
    protected NavMeshAgent agent;
    public Animator animator { get; private set; }

    public int ID { get; private set; } = -1;
    [SerializeField] protected float speed;
    [SerializeField] protected float turnSpeed;

    private Vector3 moveDir;
    private Vector3 moveDest;
    private Vector3 rotateTarget;

    [Tooltip("По умолчанию авто-назначается начальная позиция")]
    [SerializeField] private Vector3 resetPoint;

    private bool isMoveDirty;
    private bool isMoveToDirty;
    private bool isRotateToDirty;
    private bool isTeleportDirty;
    private bool isResetPosDirty;

    protected virtual void Awake()
    {
        if (resetPoint == Vector3.zero)
            resetPoint = gameObject.transform.position;

        // Назначает null, если компонент отсутсвует
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        EventBus.Subscribe<int, Vector3>(EventType.OnMove, Move);
        EventBus.Subscribe<int, Vector3>(EventType.OnMoveTo, MoveTo);
        EventBus.Subscribe<int, bool>(EventType.OnInterruptMoveTo, InterruptMoveTo);
        EventBus.Subscribe<int, Vector3>(EventType.OnRotateTo, RotateTo);
        EventBus.Subscribe<int, Vector3>(EventType.OnTeleport, Teleport);

        EventBus.Subscribe<int, string>(EventType.OnAnimationStart, AnimationStart);
        EventBus.Subscribe<int, string>(EventType.OnAnimationStop, AnimationStop);

        EventBus.Subscribe<int, Vector3>(EventType.UpdateResetPoint, UpdateResetPoint);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<int, Vector3>(EventType.OnMove, Move);
        EventBus.Unsubscribe<int, Vector3>(EventType.OnMoveTo, MoveTo);
        EventBus.Unsubscribe<int, bool>(EventType.OnInterruptMoveTo, InterruptMoveTo);
        EventBus.Unsubscribe<int, Vector3>(EventType.OnRotateTo, RotateTo);
        EventBus.Unsubscribe<int, Vector3>(EventType.OnTeleport, Teleport);

        EventBus.Unsubscribe<int, string>(EventType.OnAnimationStart, AnimationStart);
        EventBus.Unsubscribe<int, string>(EventType.OnAnimationStop, AnimationStop);

        EventBus.Unsubscribe<int, Vector3>(EventType.UpdateResetPoint, UpdateResetPoint);
    }

    protected virtual void Update()
    {
        if (isMoveDirty)
        {
            if(rb != null)
            {
                rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);

                if (moveDir == Vector3.zero)
                    isMoveDirty = false;
            }
            else isMoveDirty = false;
        }

        if (isMoveToDirty)
        {
            if(agent != null)
            {
                agent.ResetPath();
                agent.SetDestination(moveDest);

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    EventBus.Raise(EventType.OnMoveToArrived, ID);
                    isMoveToDirty = false;
                }
            }
            else isMoveToDirty = false;
        }

        if (isRotateToDirty)
        {
            Quaternion targetQuat = Quaternion.LookRotation(rotateTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuat, Time.deltaTime * turnSpeed);

            if (Vector3.Angle(rotateTarget, transform.forward) <= 1f)
                isRotateToDirty = false;
        }

        if (isTeleportDirty)
        {
            transform.position = moveDest;
            isTeleportDirty = false;
        }

        if (isResetPosDirty)
        {
            transform.position = resetPoint;
            isResetPosDirty = false;
        }
    }



    public void SetID(int newId)
    {
        if (ID != -1) return;

        ID = newId;
    }

    public void Move(int targetID, Vector3 direction)
    {
        if (targetID != ID) return;

        moveDir = direction;
        isMoveDirty = true;
    }

    public void MoveTo(int targetID, Vector3 destination)
    {
        if (targetID != ID) return;

        moveDest = destination;
        isMoveToDirty = true;
    }

    public void InterruptMoveTo(int targetID, bool interrupt)
    {
        if (targetID != ID) return;

        agent.isStopped = interrupt; 
        agent.updateRotation = !interrupt;

        if (interrupt)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
    }

    public void RotateTo(int targetID, Vector3 targetPos)
    {
        if (targetID != ID) return;

        rotateTarget = (targetPos - transform.position).normalized;
        rotateTarget.y = 0;

        isRotateToDirty = true;
    }

    public void Teleport(int targetID, Vector3 destination)
    {
        if (targetID != ID) return;

        moveDest = destination;
        isTeleportDirty = true;
    }

    public void ResetPos()
    {
        isResetPosDirty = true;
    }

    public void AnimationStart(int targetID, string animBoolName)
    {
        if (targetID != ID) return;

        animator.SetBool(animBoolName, true);
    }

    public void AnimationStop(int targetID, string animBoolName)
    {
        if (targetID != ID) return;

        animator.SetBool(animBoolName, false);
    }

    public void UpdateResetPoint(int targetID, Vector3 newResetPoint)
    {
        if (targetID != ID) return;

        resetPoint = newResetPoint;
    }
}
