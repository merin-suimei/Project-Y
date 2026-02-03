using System;
using UnityEngine;

public class SwitchView : MonoBehaviour
{
    [Header("Model Settings")] 
    [SerializeField] private SwitchModel modelData;
    
    [SerializeField] private Animator animator;
    [SerializeField] private string animationParamName = "IsOn";
    private SwitchController _controller;
    private SwitchModel _model;

    // // Метод, вызываемый Контроллером или Моделью при изменении состояния
    // public void UpdateVisuals(bool isOn)
    // {
    //     // if (animator != null)
    //     // {
    //     //     animator.SetBool(animationParamName, isOn);
    //     // }
    // }
    
    private void Awake()
    {
        _model = new SwitchModel(modelData.Id, modelData.IsOn, modelData.IsPreventTurningOff);
        _model.OnStateChanged += HandleStateChanged;
        _controller = new SwitchController(_model);
    }

    private void Start()
    {
        HandleStateChanged(_model.IsOn);
    }

    private void OnDestroy()
    {
        if (_model != null)
        {
            _model.OnStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(bool isOn)
    {
        if (animator != null)
        {
            //animator.SetBool(animationParamName, isOn);
            Debug.Log("View: _model is changed");
        }
    }
    
    public void Interact()
    {
        //Debug.Log("View: interact");
        _controller.Interact();
    }
}