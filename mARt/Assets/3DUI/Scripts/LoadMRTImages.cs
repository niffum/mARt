/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadMRTImages : MonoBehaviour {

	// TODO:
	// Check if code is from anywhere

	private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png", ".tif" };

	private Texture3D tex;
    public Vector3Int size;

	[SerializeField]
	private string folder; 

	[SerializeField]
	private string textureName; 

	[SerializeField]
	private bool save3dTexture;

    [SerializeField]
    private bool createTextureWithGradients;
 
    void Start ()
    {
		size = GetSizeOfVolumeFolder(folder);
        tex = new Texture3D (size.x, size.y, size.z, TextureFormat.ARGB32, true);


        if (createTextureWithGradients)
        {
            Create3DTextureWithGradients();
        }
        else
        {
            Create3DTexture();
        }
        
		if(save3dTexture)
		{
			CreateTexture3DAsset(tex);
		}
		
    }  

	private void Create3DTexture()
	{
		ApplyPixels(ConvertFolderToVolume(true));
	}

	public void Create3DTextureWithGradients()
	{
		float[,,] isoValues = GetImageValues();
		Vector3[] gradients = SmoothGradients( CreateGradientValues(isoValues) );
		ApplyPixels(SaveGradientsAndIsoValues(gradients, isoValues));
		//ApplyPixels(DEBUGIsoValuesToColor(isoValues));
	}

    // Source: MixedRealityToolkit-Unity-master/Assets/HoloToolkit-Examples/Medical/Scripts/VolumeImportImages.cs 
    // in old version of https://github.com/Microsoft/MixedRealityToolkit-Unity ----------------------------------------------
    public Color[] ConvertFolderToVolume(bool inferAlpha)	
	{
		var imageNames = GetImagesInFolder(folder);
		
		var cols = new Color[size.x*size.y*size.z];

		var tex = new Texture2D(2, 2);

		int z = 0;
		int index = 0;
		foreach (var imageFile in imageNames)
		{
			bool loaded = tex.LoadImage(ReadBytesFromLocalFile(imageFile));
			if (!loaded)
			{
				Debug.LogError("Couldn't load '" + imageFile + "'...");
				return null;
			}
			var fromPixels = tex.GetPixels32();
			for (var y = 0; y < size.y; ++y)
			{
				for (var x = 0; x < size.x; ++x)
				{
					var from = fromPixels[x + (y * size.x)];
					if (inferAlpha)
					{
						from.a = (byte)Mathf.Max(from.r, from.g, from.b);
					}
					cols[index] = from;
					cols[index].a *= from.r;
					index++;
				}
			}
			++z;
		}

		return cols;//Color32ArrayToByteArray(cols);
	}
// --------------------------------------------------------------------------------------------------------

    private float[,,] GetImageValues()
	{
		var imageNames = GetImagesInFolder(folder);

		float[,,] isoValues = new float[size.z,size.y,size.x];

		var tex = new Texture2D(2, 2);
		int z = 0;

		foreach (var imageFile in imageNames)
		{
			bool loaded = tex.LoadImage(ReadBytesFromLocalFile(imageFile));
			if (!loaded)
			{
				Debug.LogError("Couldn't load '" + imageFile + "'...");
				return null;
			}
			var fromPixels = tex.GetPixels32();
			for (var y = 0; y < size.y; ++y)
			{
				for (var x = 0; x < size.x; ++x)
				{
					var from = fromPixels[x + (y * size.x)];
                    // We take r as isovalue
                    isoValues[z, y, x] = (float)(from.r / 255f);
				}
			}
			++z;
		}

		return isoValues;
	}

//Gradient calculation based on: http://graphicsrunner.blogspot.com/2009/01/volume-rendering-102-transfer-functions.html ----------

    public Vector3[] CreateGradientValues(float[,,] isoValues)
	{

        //Vector3[] gradients = new Vector3[size.x*size.y*size.z];
        Vector3[] gradients = new Vector3[isoValues.GetLength(0) * isoValues.GetLength(1) * isoValues.GetLength(2)];
        int n = 1;
		Vector3 s1, s2;

		int index = 0;

		for (int z = 0; z < isoValues.GetLength(0); z++)
		{
			for (int y = 0; y < isoValues.GetLength(1); y++)
			{
				for (int x = 0; x < isoValues.GetLength(2); x++)
				{
					try{
						// Check voxels before and after current one
						s1.x = isoValues[z, y, x - n];
						s2.x = isoValues[z, y, x + n];
						s1.y = isoValues[z, y - n, x];
						s2.y = isoValues[z, y + n, x];
						s1.z = isoValues[z - n, y, x];
						s2.z = isoValues[z + n, y, x];

					}
					catch(IndexOutOfRangeException e)
					{
						// Check voxels before and after current one
						s1.x = isoValues[z, y, x];
						s2.x = isoValues[z, y, x];
						s1.y = isoValues[z, y, x];
						s2.y = isoValues[z, y, x];
						s1.z = isoValues[z, y, x];
						s2.z = isoValues[z, y, x];

                    }
					gradients[index++] = (s2 - s1)/(2f*n);
            
                   
                    
				}
			}
		}

		return gradients;
	}
// --------------------------------------------------------------------------------------------------------

    public Vector3[] SmoothGradients(Vector3[] gradients)
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

    public Color[] SaveGradientsAndIsoValues(Vector3[] gradients, float[,,] isoValues)
	{
		//var cols = new Color[size.x*size.y*size.z];
        Color[] cols = new Color[isoValues.GetLength(0) * isoValues.GetLength(1) * isoValues.GetLength(2)];
        int index = 0;

		for (int z = 0; z < isoValues.GetLength(0); z++)
		{
			for (int y = 0; y < isoValues.GetLength(1); y++)
			{
				for (int x = 0; x < isoValues.GetLength(2); x++)
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

	public void CreateTexture3DAsset(Texture3D texture)
	{
		UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/3DTextures/" + textureName + ".asset");
	}

	public static Vector3Int GetSizeOfVolumeFolder(string folder)
	{
		var images = GetImagesInFolder(folder);

		if (images.Length == 0)
		{
			return Vector3Int.zero;
		}


		var tex = new Texture2D(2, 2);
		bool loaded = tex.LoadImage(ReadBytesFromLocalFile(images.First()));
		Debug.Assert(loaded);
		return new Vector3Int(tex.width, tex.height, images.Length);
	}

	private static bool IsFileAnImage(string file)
	{
		var fileLower = file.ToLower();
		return ValidImageFileExtensions.Any(k => fileLower.EndsWith(k));
	}

	private static string[] GetImagesInFolder(string folder)
	{
		return Directory.GetFiles(folder)
						.Where(k => IsFileAnImage(k))
						.OrderBy(k => k.ToLower())
						.ToArray();
	}

	 public static byte[] ReadBytesFromLocalFile(string fullPath)
        {
            var path = fullPath;
            byte[] result = null;
            try
            {

				using (FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open))
				{
					using(BinaryReader br = new System.IO.BinaryReader(fs))
					{
						if (fs.Length > int.MaxValue)
						{
							throw new System.ArgumentOutOfRangeException();
						}

						result = br.ReadBytes((int)fs.Length);
					}
				}
                
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Read file exception: " + ex.ToString());
            }
            return result;
        }

		public static byte[] Color32ArrayToByteArray(Color32[] vals)
        {
            var result = new byte[vals.Length * 4];
            for (var i = 0; i < vals.Length; ++i)
            {
                var v = vals[i];
                var ndx = (i * 4);
                result[ndx + 0] = v.r;
                result[ndx + 1] = v.g;
                result[ndx + 2] = v.b;
                result[ndx + 3] = v.a;
            }
            return result;
        }
    
}
