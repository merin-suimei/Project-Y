using UnityEngine;

public class LightCirclePosition : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _emissionIntensity;
    [SerializeField] private float _maxBrightness;
    [SerializeField] private float _circleSoftness;

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
        Shader.SetGlobalVector("_PositionPlayerLight", new Vector4(p.x, p.y, p.z, 1f));
        Shader.SetGlobalFloat("_LightCircleRadius", _circleRadius);
        Shader.SetGlobalFloat("_LightCircleIntensity", _emissionIntensity);
        Shader.SetGlobalFloat("_LightCircleMaxBrightness", _maxBrightness);
        Shader.SetGlobalFloat("_LightCircleSoftness", _circleSoftness);
    }
}
