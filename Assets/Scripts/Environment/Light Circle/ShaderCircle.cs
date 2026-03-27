using UnityEngine;
using UnityEngine.Rendering;

public class ShaderCircle : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private float _radius;
    [SerializeField] private float _intens;
    [SerializeField] private Color _color;
    [SerializeField] private float _maxLight;
    [SerializeField] private float _minLight;
    [SerializeField] private float _softness;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        var p = body.position;
        Shader.SetGlobalVector("_PlayerPosWS", new Vector4(p.x, p.y, p.z, 1f));
        Shader.SetGlobalFloat("_ShaderCircleRadius", _radius);
        Shader.SetGlobalFloat("_ShaderCircleIntensity", _intens);
        Shader.SetGlobalColor("_ShaderCircleColor", _color);
        //Shader.SetGlobalFloat("_MaxLight", _maxLight);
        Shader.SetGlobalFloat("_MinLight", _minLight);
        Shader.SetGlobalFloat("_ShadowCircleSoftness", _softness);

        /*
        var gp = Shader.GetGlobalVector("_PlayerPosWS");
        var gr = Shader.GetGlobalFloat("_ShaderCircleRadius");
        var gi = Shader.GetGlobalFloat("_ShaderCircleIntensity");
        var gc = Shader.GetGlobalColor("_ShaderCircleColor");
        //var gl = Shader.GetGlobalFloat("_MaxLight");
        var gml = Shader.GetGlobalFloat("_MinLight");
        var gs = Shader.GetGlobalFloat("_ShadowCircleSoftness");
        Debug.Log($"Globals: Pos={gp} R={gr} I={gi} C={gc} L={gml} S={gs}", this);
        */
    }
}
