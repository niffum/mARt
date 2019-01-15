using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateTransferColorTexture : MonoBehaviour {

	private  List<TransferControlPoint> colorKnots;

	private  List<TransferControlPoint> alphaKnots;
	void Start () {
        CreateKnots();
        CreateTexture3DAsset(Create2DTransferColorTexture());
        CreateTexturePNG(Create2DTransferColorTexture());
	}   

    private void CreateTexture3DAsset(Texture2D texture)
	{
		UnityEditor.AssetDatabase.CreateAsset(texture, "Assets/2DTransferTextures/TransferTexture.asset");
	}

    private void CreateTexturePNG(Texture2D texture)
    {
         byte[] bytes = texture.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/2DTransferTextures/TransferTexturePNG.png", bytes);

    }

	private Texture2D Create2DTransferColorTexture()
	{
		Texture2D texture = new Texture2D(256, 1);

        // Go over all pixels, each pixel represents an iso value
		for (int x = 0; x < texture.width; x++)
		{
            Color pixelColor = new Color(0,0,0,0);
            float pixelAlpha = 0f;


            for(int i = 0; i < alphaKnots.Count; i++)
            {

                //TODO:
                // Interpolate between alphas e.g with a cubic spline

                TransferControlPoint alphaPoint = alphaKnots[i];

                if(x >= alphaPoint.IsoValue)
                {
                    pixelAlpha = alphaPoint.Color.w;
                }
            }

            for(int i = 0; i < colorKnots.Count; i++)
            {

                //TODO:
                // Interpolate between colors e.g with a cubic spline

                TransferControlPoint colorPoint = colorKnots[i];

                if(x >= colorPoint.IsoValue)
                {
                    // color * opacity  (Wittenbrink)
                    pixelColor = new Color(colorPoint.Color.x * pixelAlpha,
                                            colorPoint.Color.y * pixelAlpha,
                                            colorPoint.Color.z* pixelAlpha,
                                            pixelAlpha);
                }
            }
            
            
            texture.SetPixel(x, 1, pixelColor);
		}

		texture.Apply();
        return texture;
	}
	
	private void CreateKnots()
	{
		colorKnots = new List<TransferControlPoint> {
                        new TransferControlPoint(.5f, 0, 0, 0),
                        new TransferControlPoint(.5f, 0, 0, 80),
                        new TransferControlPoint(0, .5f, .2f, 82),
                        new TransferControlPoint(0, .5f, .2f, 256)
                        };

		alphaKnots = new List<TransferControlPoint> {
								new TransferControlPoint(0.0f, 0),
								new TransferControlPoint(0.0f, 40),
								new TransferControlPoint(0.2f, 60),
								new TransferControlPoint(0.05f, 63),
								new TransferControlPoint(0.0f, 80),
								new TransferControlPoint(0.9f, 82),
								new TransferControlPoint(1f, 256)
								}; 
	}

}

// Reference: 
// http://graphicsrunner.blogspot.com/2009/01/volume-rendering-102-transfer-functions.html
public class TransferControlPoint
{
    public Vector4 Color;
    public int IsoValue;

    /// <summary>
    /// Constructor for color control points.
    /// Takes rgb color components that specify the color at the supplied isovalue.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="isovalue"></param>
    public TransferControlPoint(float r, float g, float b, int isovalue)
    {
        Color.x = r;
        Color.y = g;
        Color.z = b;
        Color.w = 1.0f;
        IsoValue = isovalue;
    }

    /// <summary>
    /// Constructor for alpha control points.
    /// Takes an alpha that specifies the aplpha at the supplied isovalue.
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="isovalue"></param>
    public TransferControlPoint(float alpha, int isovalue)
    {
        Color.x = 0.0f;
        Color.y = 0.0f;
        Color.z = 0.0f;
        Color.w = alpha;
        IsoValue = isovalue;
    }
}
