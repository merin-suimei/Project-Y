using UnityEngine;

public class LightCirclePosition : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _emissionIntensity;
    [SerializeField] private float _maxBrightness;
    [SerializeField] private float _circleSoftness;
    [SerializeField] private float _circleShadowRadius;
    [SerializeField] private float _circleShadowSoftness;
    [SerializeField] private Vector2 _circleShadowOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!Player)
        {
            Debug.LogWarning("Player Transform not set in LightCirclePosition.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            return;
        }

        var p = Player.position;
        Vector3 _vector3ShadowOffset = new Vector3(_circleShadowOffset.x, 0.0f, _circleShadowOffset.y);
        Shader.SetGlobalVector("_PositionPlayerLight", new Vector4(p.x, p.y, p.z, 1f));
        Shader.SetGlobalFloat("_LightCircleRadius", _circleRadius);
        Shader.SetGlobalFloat("_LightCircleIntensity", _emissionIntensity);
        Shader.SetGlobalFloat("_LightCircleMaxBrightness", _maxBrightness);
        Shader.SetGlobalFloat("_LightCircleSoftness", _circleSoftness);
        Shader.SetGlobalFloat("_LightCircleShadowRadius", _circleShadowRadius);
        Shader.SetGlobalFloat("_LightCircleShadowSoftness", _circleShadowSoftness);
        Shader.SetGlobalVector("_LightCircleShadowOffset", _vector3ShadowOffset);
    }
}
