using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public GameObject enemy;
    private void Awake()
    {
        instance = this;

    }

}
