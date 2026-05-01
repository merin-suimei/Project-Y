using System.Collections;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class Screamer : MonoBehaviour
{
    [SerializeField] private ScreamerSO screamerSO;
    [SerializeField] private float framesPerSecond = 12f;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        AudioSource.PlayClipAtPoint(screamerSO.audioClip, transform.position);
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        for (int i = 0; i < screamerSO.sprites.Length; i++)
        {
            spriteRenderer.sprite = screamerSO.sprites[i];
            yield return new WaitForSeconds(1/framesPerSecond);
        }
        gameObject.SetActive(false);
    }
}
