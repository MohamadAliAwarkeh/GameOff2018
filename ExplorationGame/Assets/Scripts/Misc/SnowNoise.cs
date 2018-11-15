using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowNoise : MonoBehaviour
{
    //Public Variables
    public Shader snowFallShader;
    [Range(0.001f, 0.1f)]public float flakeAmount;
    [Range(0, 1)]public float flakeOpacity;

    //Private Variable
    private Material snowFallMat;
    private MeshRenderer meshRend;

	void Start ()
    {
        meshRend = GetComponent<MeshRenderer>();
        snowFallMat = new Material(snowFallShader);
	}
	
	void Update ()
    {
        //Setting the floats to public variables
        snowFallMat.SetFloat("_FlakeAmount", flakeAmount);
        snowFallMat.SetFloat("_FlakeOpacity", flakeOpacity);

        //Getting a reference to the splat map
        RenderTexture snow = (RenderTexture)meshRend.material.GetTexture("_Splat");
        RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height, 0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(snow, temp, snowFallMat);
        Graphics.Blit(temp, snow);
        meshRend.material.SetTexture("_Splat", snow);
        RenderTexture.ReleaseTemporary(temp);
	}
}
