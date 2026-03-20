using UnityEngine;

public class DetectionService : IModel
{
    private readonly Avatar player;
    private readonly Enemy[] enemies;

    public DetectionService(Avatar player, Enemy[] enemies)
    {
        this.player = player;
        this.enemies = enemies;
    }

    public void Tick()
    {
        foreach (Enemy enemy in enemies)
            EventBus.Raise(EventType.OnPlayerVisible, enemy.ID, IsPlayerVisible(enemy));
    }

    public bool IsPlayerVisible(Enemy enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

        if (distance < enemy.catchThreshold) 
        {
            EventBus.Raise(EventType.OnEnemyCatchPlayer);
            return false;
        }

        if (distance > enemy.nearbyDetectionRange)
        {
            if (distance > enemy.detectionRange)
                return false;
            if (Vector3.Angle(enemy.transform.forward, (player.transform.position - enemy.transform.position).normalized) > enemy.detectionSemiconeAngle)
                return false;
        }

        return enemy.HasLineOfSight(player.transform);
    }
}
