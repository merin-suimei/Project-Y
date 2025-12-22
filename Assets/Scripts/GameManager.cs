using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerStateMachine player;
    public Enemy enemy;
    private void Awake()
    {
        instance = this;
    }
}
