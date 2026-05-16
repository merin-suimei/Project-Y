using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugSceneSwitcher : MonoBehaviour
{
    private InputsTypes _input;
    private bool _showMenu = false;
    private Vector2 _scrollPosition;
    private DetectionService _detectionService;

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

    public void InitDetectionService(DetectionService detectionService)
    {
        _detectionService = detectionService;
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
        _showMenu = !_showMenu;
    }

    private void OnGUI()
    {
        if (!_showMenu) return;

        GUILayout.BeginArea(new Rect(20, 20, 250, Screen.height - 40));
        GUI.Box(new Rect(0, 0, 250, Screen.height - 40), "ЧИТ-МЕНЮ");
        GUILayout.Space(30);

        if (_detectionService != null)
        {
            bool currentState = _detectionService.IsDetectionDisabled();
            
            string cheatText = currentState ? "Невидимость: ВКЛ" : "Невидимость: ВЫКЛ";
            GUI.backgroundColor = currentState ? Color.green : Color.white;
            
            if (GUILayout.Button(cheatText, GUILayout.Height(30)))
            {
                _detectionService.SetDetectionDisabled(!currentState);
            }
            GUI.backgroundColor = Color.white; 
            GUILayout.Space(10); 
        }
        else
        {
            GUILayout.Label("DetectionService не подключен!", GUILayout.Height(30));
            GUILayout.Space(10);
        }

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

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
                    _showMenu = false; 
                }
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

}