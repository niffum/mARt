using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MakeGradientTexture : MonoBehaviour {

    // TODO:
    // Check if code is from anywhere

    [SerializeField]
    private Texture3D texture3D;

    private Texture3D tex;
    public Vector3Int size;
    
	[SerializeField]
	private string textureName; 

	[SerializeField]
	private bool create3dTexture; 
 
    void Start ()
    {
        size = new Vector3Int(texture3D.width , texture3D.height , texture3D.depth);
        tex = new Texture3D (size.x, size.y, size.z, TextureFormat.ARGB32, true);
        
		
		//Create3DTexture();
		Create3DTextureWithGradients();
		
		if(create3dTexture)
		{
			CreateTexture3DAsset(tex);
		}
		
    }  

	private void Create3DTextureWithGradients()
	{
		float[,,] isoValues = Convert3dTexture(texture3D);
		Vector3[] gradients = SmoothGradients( CreateGradientValues(isoValues) );
		ApplyPixels(SaveGradientsAndIsoValues(gradients, isoValues));
		//ApplyPixels(DEBUGIsoValuesToColor(isoValues));
	}

    private float[,,] Convert3dTexture(Texture3D tex3D)
    {
        float[,,] isoValues = new float[size.z, size.y, size.x];

        for (var z = 0; z < size.z; ++z)
        {
            var fromPixels = tex.GetPixels32();
            for (var y = 0; y < size.y; ++y)
            {
                for (var x = 0; x < size.x; ++x)
                {
                    var from = fromPixels[x + (y * size.x)];
                    // We take r as isovalue
                    isoValues[z, y, x] = (float)(from.r);
                }
            }
        }

        return isoValues;
    }
	

	private Vector3[] CreateGradientValues(float[,,] isoValues)
	{
		// How to generate gradient value: GPU Gems 1:  39.4.1 
		//http://graphicsrunner.blogspot.com/2009/01/volume-rendering-102-transfer-functions.html

		Vector3[] gradients = new Vector3[size.x*size.y*size.z];

		int n = 1;
		Vector3 s1, s2;

		int index = 0;

		for (int z = n; z < size.z - n; z++)
		{
			for (int y = n; y < size.y - n; y++)
			{
				for (int x = n; x < size.x - n; x++)
				{
					// Check voxels before and after current one
					s1.x = isoValues[z, y, x - n];
					s2.x = isoValues[z, y, x + n];
					s1.y = isoValues[z, y - n, x];
					s2.y = isoValues[z, y + n, x];
					s1.z = isoValues[z - n, y, x];
					s2.z = isoValues[z + n, y, x];


					gradients[index++] = (s2 - s1)/(2f*n);
            
                    // Divide through distance on x axes 
                    // See: GPU gems
                    //gradients[index++] = Vector3.Normalize(s2 - s1)/ 2;
                    if (float.IsNaN(gradients[index - 1].x))
					{
						gradients[index - 1] = Vector3.zero;
					}
                    
				}
			}
		}

		return gradients;
	}


	private Vector3[] SmoothGradients(Vector3[] gradients)
	{
		double[] gaussKernel = Get1DGaussianKernel(5.5f, 5);
		
		for(int i= 0; i< gradients.Length; i++)
		{
			Vector3 value = Vector3.zero;
			for(int k = 0; k< gaussKernel.Length; k++)
			{
				// Multiply every value with the gaus kernel and add up the results 
				value += ( gradients[i] * (float)gaussKernel[k] ); 
			}

			// Calculate value for every dimension and add up the results
			gradients[i] = value * 3;
		}

		return gradients;
	}

	private double[] Get1DGaussianKernel(float sigma, int kernelSize)
	{
		double[] kernel = new double[kernelSize];

		int distance = (int)kernelSize/2;

		int x = distance;

		// we fill the kernel from both sides simultaniously (center is filled in twice)
		for(int i= 0; i <= distance; i++)
		{
			// Gaussian function
			float eulerExponent = -( Mathf.Pow(x ,2) / Mathf.Pow( 2*sigma ,2) );
			float divider = Mathf.Sqrt( Mathf.Pow( 2*Mathf.PI*sigma ,2) );
			float kernelValue = 1 / divider * Mathf.Exp(eulerExponent);

			//pixels to the right
			kernel[(kernelSize-1) - i] = kernelValue;
			// pixels to the left
			kernel[i] =  kernelValue;

			x--;
		}
 		return kernel;
	}

	private Color[] SaveGradientsAndIsoValues(Vector3[] gradients, float[,,] isoValues)
	{
		var cols = new Color[size.x*size.y*size.z];
        int index = 0;

		for (int z = 0; z < size.z; z++)
		{
			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
                    // Save gradient in color and isovalue in alpha channel
                    
					cols[index].r = gradients[index].x;
					cols[index].g = gradients[index].y;
					cols[index].b = gradients[index].z;
					cols[index].a = isoValues[z,y,x];
                    
                    //cols[index] = new Vector4(gradients[index].x, gradients[index].y, gradients[index].z, isoValues[z, y, x]);
                    index++;
				}
			}
		}

		return cols;
	}

	private Color[] DEBUGIsoValuesToColor(float[,,] isoValues)
	{
		var cols = new Color[size.x*size.y*size.z];
		int index = 0;

		for (int z = 0; z < size.z; z++)
		{
			for (int y = 0; y < size.y; y++)
			{
				for (int x = 0; x < size.x; x++)
				{
					// Save gradient in color and isovalue in alpha channel
					cols[index].r = isoValues[z,y,x];
					cols[index].g = isoValues[z,y,x];
					cols[index].b =isoValues[z,y,x];
					cols[index].a = 1f;
                    var col = cols[index];
                    index++;
				}
			}
		}

		return cols;
	}
	
	

	private void ApplyPixels(Color[] cols)
	{
        tex.SetPixels (cols);
        tex.Apply ();
        GetComponent<Renderer>().material.SetTexture ("_Volume", tex);
	}

	private void CreateTexture3DAsset(Texture3D texture)
	{
		UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/3DTextures/" + textureName + ".asset");
	}


	   
}
