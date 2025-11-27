using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MovingText : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneField mainMenu;
    
    [Header("Text")]
    public TMP_Text creditsText;
    public TextAsset creditsTextAsset;
    public ScrollRect scrollRect;

    [Header("Scroll Settings")]
    public float scrollSpeed = 50f;
    public bool loop = false;
    public float endDelay = 3f;

    private bool isScrolling = true;
    private float endTimer = 0f;

    void Start()
    {

        if (scrollRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    void EndCredits()
    {
        if (loop)
        {
            scrollRect.content.anchoredPosition -= new Vector2(0, scrollRect.content.rect.height);
        }
        else
        {
            SceneManager.LoadScene(mainMenu);
        }
    }

    void Update()
    {
        if (!isScrolling)
        {
            endTimer += Time.deltaTime;
            if (endTimer >= endDelay)
                EndCredits();
            return;
        }

        scrollRect.content.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        if (scrollRect.verticalNormalizedPosition <= 0f)
        {
            EndCredits();
        }
    }
}
