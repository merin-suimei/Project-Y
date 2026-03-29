using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TempWallHider : MonoBehaviour
{
    public bool inverted = false;

    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();

        mesh.enabled = !inverted;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошел именно Игрок
        if (other.CompareTag("Player"))
        {
            mesh.enabled = inverted;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, что вошел именно Игрок
        if (other.CompareTag("Player"))
        {
            mesh.enabled = !inverted;
        }
    }
}
