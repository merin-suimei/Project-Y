using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MovingText : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private SceneField mainMenu;

    [SerializeField] private CreditSceneManager creditSceneManager;

    [Header("Text")]
    public TMP_Text creditsText;
    public TextAsset creditsTextAsset;
    public ScrollRect scrollRect;

    [Header("Scroll Settings")]
    public float scrollSpeed = 50f;
    public bool loop = false;
    public float startDelay = 1f; 
    public float endDelay = 3f;

    private bool isScrolling = false;
    private bool isFinished = false;
    private float startTimer = 0f;
    private float endTimer = 0f;


    void Start()
    {
        creditsText.text = creditsTextAsset.text;

        if (scrollRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            //scrollRect.verticalNormalizedPosition = 1f;
            SetTextToStartPosition();
        }

    }

    void EndCredits()
    {
        if (loop)
        {
            //scrollRect.content.anchoredPosition -= new Vector2(0, scrollRect.content.rect.height);

            SetTextToStartPosition();
            isScrolling = false;
            isFinished = false;
            startTimer = 0f;
            endTimer = 0f;
        }
         else
        {
            creditSceneManager.EndFinal();
        }

    }

    public void SkipCredits()
    {
        loop = false;
        EndCredits();
    }

    void Update()
    {
        if (!isScrolling && !isFinished)
        {
            startTimer += Time.deltaTime;
            if (startTimer >= startDelay)
            {
                isScrolling = true;
            }
            return;
        }

        if (isFinished)
        {
            endTimer += Time.deltaTime;
            if (endTimer >= endDelay)
                EndCredits();
            return;
        }

        scrollRect.content.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        float contentBottomY = scrollRect.content.localPosition.y + scrollRect.content.rect.yMin;
        float viewportTopY = scrollRect.viewport.rect.yMax;

        if (contentBottomY >= viewportTopY)
        {
            isScrolling = false;
            isFinished = true;
        }
    }

    private void SetTextToStartPosition()
    {
        float targetY = scrollRect.viewport.rect.yMin - scrollRect.content.rect.yMax - 10;
        Vector3 pos = scrollRect.content.localPosition;
        pos.y = targetY;
        scrollRect.content.localPosition = pos;
    }
}
