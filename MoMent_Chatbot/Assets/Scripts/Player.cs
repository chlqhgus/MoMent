using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Animator anim;
	//public Animator anim2;
	//public Animator anim3;
	//public Animator anim4;
	public Rigidbody rbody;
	//public Button Text;
	//public Canvas yourcanvas;

	private float inputH;
	private float inputV;
	private bool run;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		//anim2 = GetComponent<Animator>();
		//anim3 = GetComponent<Animator>();
		//anim4 = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
		run = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("1")) 
		{
			anim.Play ("idle", -1, 0f);
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			run = true;
		} else {
			run = false;
		}

		inputH = Input.GetAxis ("Horizontal");
		inputV = Input.GetAxis ("Vertical");

		anim.SetFloat ("inputH", inputH);
		anim.SetFloat ("inputV", inputV);
		anim.SetBool ("run", run);

		float moveX = inputH * 20f * Time.deltaTime;
		float moveZ = inputV * 50F * Time.deltaTime;

		if (moveZ <= 0f) {
			moveX = 0f;
		}
		else if(run)
		{
			moveX*=3f;
			moveZ*=3f;
		}

		rbody.velocity = new Vector3 (moveX, 0f, moveZ);
	}
}
