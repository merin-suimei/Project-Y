using UnityEngine;
using UnityEngine.AI;

public class BabaYaga : Avatar
{
    private bool isHidden;

    private void Start()
    {
        EventBus.Subscribe(EventType.OnTimerIsUP, Spawn);
        isHidden = true;
        gameObject.SetActive(false);
    }

    private void Spawn()
    {
        gameObject.SetActive(true);
        isHidden = false;
    }


}
