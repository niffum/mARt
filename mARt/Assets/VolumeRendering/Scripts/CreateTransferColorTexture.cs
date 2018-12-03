using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTransferColorTexture : MonoBehaviour {

	private  List<TransferControlPoint> colorKnots;

	private  List<TransferControlPoint> alphaKnots;
	void Start () {
		
	}

	private void Create2DTransferColorTexture()
	{
		Texture2D texture = new Texture2D(256, 1);


		for (int x = 0; x < texture.width; x++)
		{
		}

		texture.Apply();
	}
	
	private void CreateKnots()
	{
		colorKnots = new List<TransferControlPoint> {
                        new TransferControlPoint(.91f, .7f, .61f, 0),
                        new TransferControlPoint(.91f, .7f, .61f, 80),
                        new TransferControlPoint(1.0f, 1.0f, .85f, 82),
                        new TransferControlPoint(1.0f, 1.0f, .85f, 256)
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
