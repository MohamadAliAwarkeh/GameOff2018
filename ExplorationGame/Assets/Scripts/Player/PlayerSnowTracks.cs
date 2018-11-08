using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnowTracks : MonoBehaviour
{

    //Public Variables
    [Header("Snow Properties")]
    public GameObject outsideTerrain;
    public Shader drawShader;
    [Range(0, 200)] public float brushSize;
    [Range(0, 1)] public float brushStrength;
    public Transform[] feet;

    //Private Variables
    private Material drawMaterial;
    private Material myMaterial;
    private RenderTexture splatMap;
    RaycastHit groundHit;
    int layerMask;

    void Start ()
    {
        layerMask = LayerMask.GetMask("OutsideGround");

        outsideTerrain = GameObject.FindGameObjectWithTag("FloorOutside");

        //Setting the draw material so that it can be affected by the shader
        drawMaterial = new Material(drawShader);
        myMaterial = outsideTerrain.GetComponent<MeshRenderer>().material;
        myMaterial.SetTexture("_Splat", splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }
	
	void Update () {
        for (int i = 0; i < feet.Length; i++)
        {
            if (Physics.Raycast(feet[i].position, -Vector3.up, out groundHit, 1f, layerMask))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", brushStrength);
                drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
        }
	}
}
