using UnityEngine;

public class ListenerRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }
    void LateUpdate()
    {
        transform.rotation = cameraTransform.rotation;
    }
}