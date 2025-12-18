using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerStateMachine player;

    private void Awake()
    {
        instance = this;
    }

}
