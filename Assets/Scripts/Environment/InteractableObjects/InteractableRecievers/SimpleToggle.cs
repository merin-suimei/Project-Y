using System.Collections.Generic;
using UnityEngine;

public class SimpleToggle : InteractableReciever
{
    [SerializeField] private List<GameObject> objectsToEnable;
    [SerializeField] private List<GameObject> objectsToDisable;

    [SerializeField] private int triggersRequired;
    private int triggers = 0;

    protected override void Action(bool state)
    {
        triggers += state ? 1 : -1;
        bool thresholdMet = triggers >= triggersRequired;

        foreach (GameObject gameObject in objectsToEnable)
            gameObject.SetActive(thresholdMet);

        foreach (GameObject gameObject in objectsToDisable)
            gameObject.SetActive(!thresholdMet);
    }
}
