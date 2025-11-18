using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MovingText : MonoBehaviour
{
    public string linkToMainMenu = "Assets/Scenes/Prototype2.unity";
    public string pathToCreditsText = "Assets/Scripts/creditsPlaceholder.txt";
    private Scene mainMenuScene;

    public void Start()
    {
        mainMenuScene = SceneManager.GetSceneByPath(linkToMainMenu);

        if (!File.Exists(pathToCreditsText))
        {
            Debug.LogError("Credits were not loaded!");
            return;
        }
        string creditsText = File.ReadAllText(pathToCreditsText);

        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found!");
            return;
        }

        ScrollView scrollView = uiDocument.rootVisualElement.Q<ScrollView>();
        if (scrollView == null)
        {
            Debug.LogError("ScrollView not found!");
            return;
        }
        scrollView.Clear();
        Label textLabel = new Label(creditsText);
        // TODO: better to just add style to it written in uss but whatever
        textLabel.style.whiteSpace = WhiteSpace.Normal;
        textLabel.style.color = Color.white;
        textLabel.style.fontSize = 14;
        textLabel.style.unityTextAlign = TextAnchor.UpperLeft;
        textLabel.style.marginLeft = 10f;
        textLabel.style.marginRight = 10f;
        textLabel.style.marginTop = 10f;
        textLabel.style.marginBottom = 10f;

        scrollView.Add(textLabel);
    }

    private void onDestroy()
    {
        SceneManager.UnloadSceneAsync(mainMenuScene);
    }

}
