using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

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
    public float startDelay = 1f; 
    public float endDelay = 3f;

    private bool isScrolling = false;
    private bool isFinished = false;
    private float startTimer = 0f;
    private float endTimer = 0f;

    private InputsTypes _input;

    private void Awake()
    {
        _input = new InputsTypes();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.UI.Exit.performed += OnExit;
    }

    private void OnDisable()
    {
        _input.UI.Exit.performed -= OnExit;
        _input.Disable();
    }
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
            SoundManager.Instance.StopAllSounds();
            EventBus.Raise(EventType.ResetGameState);
            SceneManager.LoadScene(mainMenu);
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
        float viewportBottomY = scrollRect.viewport.rect.yMin;

        if (contentBottomY >= viewportBottomY)
        {
            isScrolling = false;
            isFinished = true;
        }
    }

    private void OnExit(InputAction.CallbackContext ctx)
    {
        SkipCredits();
    }

    private void SetTextToStartPosition()
    {
        float targetY = scrollRect.viewport.rect.yMin - scrollRect.content.rect.yMax - 10;
        Vector3 pos = scrollRect.content.localPosition;
        pos.y = targetY;
        scrollRect.content.localPosition = pos;
    }
}
