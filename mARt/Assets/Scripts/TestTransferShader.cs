using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTransferShader : MonoBehaviour {

	protected Material material;

	[SerializeField]
    public Texture2D transferColor;

	[SerializeField]
        protected Shader shader;

	protected virtual void Start () {
            //material = new Material(GetComponent<MeshRenderer>().material.shader);
			material = new Material(shader);
			GetComponent<MeshRenderer>().material =material;
            //GetComponent<MeshFilter>().sharedMesh = Build();
            //GetComponent<MeshRenderer>().sharedMaterial = material;
        }
	
	// Update is called once per frame
	void Update () {
		material.SetTexture("_TransferColor", transferColor);
	}
}
