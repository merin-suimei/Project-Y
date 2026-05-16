using System.Collections.Generic;
using UnityEngine;

public class SimpleToggle : InteractableReciever
{
    [SerializeField] private List<GameObject> objectsToEnable;
    [SerializeField] private List<GameObject> objectsToDisable;

    protected override void Action(bool state)
    {
        foreach (GameObject gameObject in objectsToEnable)
        {
            gameObject.SetActive(state);
        }

        foreach (GameObject gameObject in objectsToDisable)
        {
            gameObject.SetActive(!state);
        }
    }
}
