using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class PlayerIlluminationShader : MonoBehaviour {
    public Avatar player;
    public Material material;
    public float smoothStep = 0.1f;
    public float TimeBetweenUpdates = 0.1f;

    private float IsIlluminatedSmoothstep = 1.0f;
    private float TimeElapsed = 0;
    

    public void Update()
    {
        TimeElapsed += Time.deltaTime;
        if(TimeElapsed < TimeBetweenUpdates) return; 

        if(player.IsIlluminated > 0 && IsIlluminatedSmoothstep < 1.0)
        {
            IsIlluminatedSmoothstep += smoothStep;
            Math.Clamp(IsIlluminatedSmoothstep, 0.0f, 1.0f);
        } else if (player.IsIlluminated == 0 && IsIlluminatedSmoothstep > 0.0)
        {
            IsIlluminatedSmoothstep -= smoothStep;
            Math.Clamp(IsIlluminatedSmoothstep, 0.0f, 1.0f);
        }

        material.SetFloat("_IsIlluminatedSmoothStep", IsIlluminatedSmoothstep);
        TimeElapsed = 0;
    }
};