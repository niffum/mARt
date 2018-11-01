using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadMRTImages : MonoBehaviour {

	 private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png" };

	private Texture3D tex;
    public Vector3Int size;

	[SerializeField]
	private string folder; 
 
    void Start ()
    {
		size = GetSizeOfVolumeFolder(folder);
        tex = new Texture3D (size.x, size.y, size.z, TextureFormat.ARGB32, true);
        
		
		ApplyPixels(ConvertFolderToVolume(true));
		//CreateTexture3DAsset(tex);
    }  

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

	private void ApplyPixels(Color[] cols)
	{
        tex.SetPixels (cols);
        tex.Apply ();
        GetComponent<Renderer>().material.SetTexture ("_Volume", tex);
	}

	private void CreateTexture3DAsset(Texture3D texture)
	{
		//UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/3DTextures/brain.asset");
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
