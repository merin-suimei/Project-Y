using System;
using UnityEngine;

[Serializable]
public class SwitchModel
{
    [Header("Data")]
    [SerializeField] private string id = "Object_01";
    [SerializeField] private bool isOn = false;

    [Header("Rules")]
    [Tooltip("Если true, объект нельзя выключить после включения")]
    [SerializeField] private bool preventTurningOff = false;
    
    public event Action<bool> OnStateChanged;
    
    public string Id => id;
    public bool IsOn => isOn;
    public bool IsPreventTurningOff => preventTurningOff;

    public SwitchModel(string id, bool isOn, bool preventTurningOff = false)
    {
        this.id = id;
        this.isOn = isOn;
        this.preventTurningOff = preventTurningOff;
    }
    
    public bool CanToggle()
    {
        // Если запрещено выключать И объект уже включен — запрещаем действие
        if (preventTurningOff && isOn)
        {
            return false;
        }
        return true;
    }
    
    public bool TryToggle()
    {
        if (!CanToggle())
        {
            return false;
        }
        isOn = !isOn;
        OnStateChanged?.Invoke(isOn);
        return true;
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;
    }
}