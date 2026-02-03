using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public Enemy enemy;
    private void Awake()
    {
        instance = this;
    }
}
