using UnityEngine;

public class LightCircleFullscreenController : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Material lightCircleMaterial;

    // Какая камера должна проецировать позицию игрока
    [SerializeField] private Camera targetCamera;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (lightCircleMaterial == null || body == null || targetCamera == null) return;

        // 0..1 по экрану (Viewport). z — глубина, можно оставить для отладки
        Vector3 vp = targetCamera.WorldToViewportPoint(body.position);

        //Debug.Log($"vp: {vp}");
        //lightCircleMaterial.SetFloat("_Intensity", Mathf.PingPong(Time.time, 2f) + 1f);

        // Если игрок за камерой, можно выключить эффект или зажать координаты
        // (иначе маска может "улетать")
        if (vp.z <= 0f)
            return;

        lightCircleMaterial.SetVector("_PlayerPosSS", new Vector4(vp.x, vp.y, vp.z, 0f));
    }
}
