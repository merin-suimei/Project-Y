using UnityEngine;

public class LightCirclePosition : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float _circleRadius;
    [SerializeField] private float _emissionIntensity;
    [SerializeField] private Color _emissionColor;
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
        Shader.SetGlobalFloat("_CircleRadius", _circleRadius);
        Shader.SetGlobalFloat("_EmissionIntensity", _emissionIntensity);
        Shader.SetGlobalColor("_EmissonColor", _emissionColor);
        Shader.SetGlobalFloat("_MaximumBrightness", _maxBrightness);
        Shader.SetGlobalFloat("_CircleSoftness", _circleSoftness);
    }
}
