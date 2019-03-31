/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGradientsForAsset : MonoBehaviour {

    [SerializeField]
    private string path;

    [SerializeField]
    private LoadMRTImages loadMrtImages;

    [SerializeField]
    private string textureName;

    private void Start()
    {

        Texture3D tex = LoadAssetData();
        float[,,] values = ConvertToFloatArray(tex);
        Texture3D newTex = new Texture3D(tex.width, tex.height, tex.depth, TextureFormat.ARGB32, true);
        Vector3[] gradients = loadMrtImages.SmoothGradients(loadMrtImages.CreateGradientValues(values));
        ApplyPixels(loadMrtImages.SaveGradientsAndIsoValues(gradients, values), newTex);
        CreateTexture3DAsset(newTex);
    }

    private Texture3D LoadAssetData()
    {
        return null; //(Texture3D)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Texture3D));
    }

    private float[,,] ConvertToFloatArray(Texture3D texture)
    {
        float[,,] textureValues = new float[texture.depth, texture.height,  texture.width];

        Color32[] pixels = texture.GetPixels32();

        int index = 0;
        for (int z = 0; z < texture.depth; z++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    textureValues[z, y, x] = pixels[index].r / 255f;
                    index++;
                }
            }
        }
        return textureValues;
    }

    public void CreateTexture3DAsset(Texture3D texture)
    {
        //UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/3DTextures/" + textureName + ".asset");
    }

    private void ApplyPixels(Color[] cols, Texture3D tex)
    {
        tex.SetPixels(cols);
        tex.Apply();
    }
}
