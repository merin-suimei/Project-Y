using UnityEngine;

public class LightCircleFullscreenController : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Material lightCircleMaterial;

    [SerializeField] private Camera targetCamera;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (lightCircleMaterial == null || body == null || targetCamera == null) return;

        Vector3 vp = targetCamera.WorldToViewportPoint(body.position);
        float playerDepth01 = vp.z / targetCamera.farClipPlane;
        Shader.SetGlobalFloat("_PlayerDepth", playerDepth01); // не понимаю, почему с материалаом не работает

        //Debug.Log($"vp: {vp}");

        if (vp.z <= 0f)
            return;

        lightCircleMaterial.SetVector("_PlayerPosSS", new Vector4(vp.x, vp.y, vp.z, 0f));
    }
}
