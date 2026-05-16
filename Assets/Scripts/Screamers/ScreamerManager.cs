using UnityEngine;
using System.Collections.Generic;
public class ScreamerManager : MonoBehaviour
{
    [SerializeField] private List<Screamer> screamers;
    [SerializeField] float coolDownTimer = 20f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("if screamer chance is bigger than value, a screamer will appear")] float screamerSuccessChance;

    [SerializeField]
    [Tooltip("Screamer wiil appear if player eneters the collider N times and there were no screamers before")] int eneterTriggerAmountForScreamer = 10;
    private float timeToNextScreamer;
    private int availableScreamersAmount;
    private int eneterColliderAmount;
    private void Start()
    {
        timeToNextScreamer = 0f;
        eneterColliderAmount = 0;
    }
    private void Update()
    {
        timeToNextScreamer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null && timeToNextScreamer <= 0f)
        {
            timeToNextScreamer = coolDownTimer;
            float screamerChance = UnityEngine.Random.Range(0f, 1f);
            int screamerIndex = UnityEngine.Random.Range(0, screamers.Count);
            if ((screamers[screamerIndex] != null && screamerChance >= screamerSuccessChance) || eneterColliderAmount >= eneterTriggerAmountForScreamer)
            {
                screamers[screamerIndex].gameObject.SetActive(true);
                screamers.RemoveAt(screamerIndex);
                eneterColliderAmount = 0;
                if (screamers.Count <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                eneterColliderAmount++;
            }
        }
    }
}


