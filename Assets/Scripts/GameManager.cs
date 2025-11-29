using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public Player player;
    public GameObject player;
    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

}
