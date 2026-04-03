using UnityEngine;

public class SwitchController
{
    private SwitchModel _model;
    
    
    public SwitchController(SwitchModel model)
    {
        _model = model;
    }

    public void Interact()
    {
        bool success = _model.TryToggle();

        if (success)
        {
            NotifyGameManager();
        }
        else
        {
            Debug.Log("Switch back is not allowed");
            // Тут можно добавить звук ошибки, анимацию "заело" или исчезновения самого объекта
        }
    }

    private void NotifyGameManager()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnSwitchStateChanged(_model.Id, _model.IsOn);
        }
        else
        {
            Debug.LogWarning("GameManager not found!");
        }
    }
}