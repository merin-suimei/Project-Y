using UnityEngine;
using UnityEngine.AI;

public class VirtualPatrolTracker : Avatar
{
    readonly EnemyWalkPoint[] points;
    readonly bool isPathClosed;

    public Vector3 idealPosition { get; private set; }
    public EnemyWalkPoint currentPoint { get; private set; }
    public bool isWaiting {  get; private set; }

    private int pointIndex;
    private int directionIndex;
    private float currentWaitTime;
    public VirtualPatrolTracker(EnemyWalkPoint[] patrolPoints, NavMeshAgent navAgent, float rotateSpeed, 
        bool isPathClosed, Vector3 initPos)
    {
        agent.speed = navAgent.speed;
        agent.acceleration = navAgent.acceleration;
        agent.angularSpeed = navAgent.angularSpeed;
        agent.stoppingDistance = navAgent.stoppingDistance;
        points = patrolPoints;
        turnSpeed = rotateSpeed;
        this.isPathClosed = isPathClosed;

        idealPosition = initPos;
        currentPoint = points[0];

    }

    public void Tick()
    {
        if (points.Length <= 0) return;

        currentPoint = points[pointIndex];

        if (isWaiting)
        {
            
        }
    }
    private void CalculatePointIndex()
    {
        if (isPathClosed)
        {
            pointIndex++;
            if (pointIndex >= points.Length)
            {
                pointIndex = 0;
            }
        }
        else
        {
            pointIndex += directionIndex;
            if (pointIndex >= points.Length - 1)
            {
                pointIndex = points.Length - 1;
                directionIndex = -1;
            }
            else if (pointIndex <= 0)
            {
                pointIndex = 0;
                directionIndex = 1;
            }
        }
    }
}
