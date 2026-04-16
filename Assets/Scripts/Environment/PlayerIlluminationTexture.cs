using Unity.VisualScripting;
using UnityEngine;

public class PlayerIlluminationTexture : MonoBehaviour {
    public Avatar player;
    public Material material;
    public Texture lightTexture;
    public Texture darkTexture;
    private bool textureIsLit = true;

    public void Update()
    {
        if(!textureIsLit && player.IsIlluminated > 0)
        {
            textureIsLit = true;
            material.SetTexture("_BaseMap", lightTexture);
        } else if (textureIsLit && player.IsIlluminated == 0)
        {
            textureIsLit = false;
            material.SetTexture("_BaseMap", darkTexture);
        }
    }
};