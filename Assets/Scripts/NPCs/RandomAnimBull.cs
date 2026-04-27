using UnityEngine;

public class RandomAnimBull : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string stateName = "Armature_002|eatChertAnim";
    [SerializeField] private float minSpeed = 0.9f;
    [SerializeField] private float maxSpeed = 1.1f;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        float randomOffset = Random.value; // 0..1
        animator.Play(stateName, 0, randomOffset);
        animator.SetFloat("EatSpeed", Random.Range(minSpeed, maxSpeed));
    }
}