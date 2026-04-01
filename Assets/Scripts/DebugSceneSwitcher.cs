using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugSceneSwitcher : MonoBehaviour
{
    private InputsTypes _input;
    private bool showMenu = false;
    private Vector2 scrollPosition;

    private void Awake()
    {
        _input = ObjectResolver.Resolve<InputsTypes>();

        if (_input == null)
        {
            _input = new InputsTypes();
            ObjectResolver.RegisterInstance(_input);
        }
        _input.UI.Enable();
        _input.UI.ToggleDebugSceneSwitcher.performed += ToggleDebugSceneSwitcher;
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.UI.ToggleDebugSceneSwitcher.performed -= ToggleDebugSceneSwitcher;
        }
    }

    private void ToggleDebugSceneSwitcher(InputAction.CallbackContext context)
    {
        showMenu = !showMenu;
    }

    private void OnGUI()
    {
        if (!showMenu) return;

        GUILayout.BeginArea(new Rect(20, 20, 250, Screen.height - 40));
        GUI.Box(new Rect(0, 0, 250, Screen.height - 40), "ТЕСТОВОЕ МЕНЮ УРОВНЕЙ");
        GUILayout.Space(30);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (i == SceneManager.GetActiveScene().buildIndex)
            {
                GUILayout.Label($"[{i}] {sceneName} (Текущая)");
            }
            else
            {
                if (GUILayout.Button($"Загрузить: [{i}] {sceneName}", GUILayout.Height(30)))
                {
                    SceneManager.LoadScene(i);
                    showMenu = false; 
                }
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}