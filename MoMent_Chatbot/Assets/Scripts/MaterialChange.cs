using UnityEngine;
using System.Collections;

public class MaterialChange : MonoBehaviour {


	public Material[] mats;
	public int index = 0;

	void start(){

		this.gameObject.GetComponent<Renderer> ().material = mats [index];

	}

	public void custom(){

		this.gameObject.GetComponent<Renderer> ().material = mats [0];

	}

	public void unlit(){

		this.gameObject.GetComponent<Renderer> ().material = mats [1];

	}

}