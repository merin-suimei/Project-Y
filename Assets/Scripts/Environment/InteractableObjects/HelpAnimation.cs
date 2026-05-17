using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class HelpAnimation : MonoBehaviour
{
    [SerializeField] private float framesPerSecond = 12f;
    [SerializeField] private Sprite[] sprites;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(Play());
    }


    private IEnumerator Play()
    {
        while (true)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                image.sprite = sprites[i];
                yield return new WaitForSeconds(1 / framesPerSecond);
            }
        }
        
        
    }
}
