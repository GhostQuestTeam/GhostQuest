using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPlaneTextureRenderer : MonoBehaviour {

	public string texturePath = "test";
	// Use this for initialization
	void Start()
	{
		Texture2D texture = Resources.Load(texturePath) as Texture2D; //No need to specify extension.
		Material material = new Material(Shader.Find("Diffuse"));
		material.mainTexture = texture;
		GetComponent<Renderer>().material = material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
