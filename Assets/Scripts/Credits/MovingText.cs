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
    public float endDelay = 3f;

    private bool isScrolling = true;
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

        if (scrollRect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            scrollRect.verticalNormalizedPosition = 1f;
        }

        creditsText.text = creditsTextAsset.text;
    }

    void EndCredits()
    {
        if (loop)
        {
            scrollRect.content.anchoredPosition -= new Vector2(0, scrollRect.content.rect.height);
        }
        else
        {
            EventBus.Raise(EventType.ResetGameState);
            SceneManager.LoadScene(mainMenu);
        }
    }

    public void SkipCredits()
    {
        EventBus.Raise(EventType.ResetGameState);
        SceneManager.LoadScene(mainMenu);
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

    private void OnExit(InputAction.CallbackContext ctx)
    {
        SkipCredits();
    }
}
