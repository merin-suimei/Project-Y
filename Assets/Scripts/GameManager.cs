using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public Enemy enemy;

    public EnemyWalkPoint[] enemyWalkPoints;
    private void Awake()
    {
        instance = this;
    }
}
