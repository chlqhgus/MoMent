using UnityEngine;
using System.Collections;

public class test_mera_ch : MonoBehaviour {

	GameObject cameraParent;

	Vector3		defaultPosition;	
	Quaternion	defaultRotation;	
	float		defaultZoom;		

	// Use this for initialization
	void Start () {


		cameraParent = GameObject.Find("CameraP");


		defaultPosition = Camera.main.transform.position;
		defaultRotation = cameraParent.transform.rotation;
		defaultZoom = Camera.main.fieldOfView;
	}

	// Update is called once per frame
	void Update () {





		if( Input.GetMouseButton(1) ){

			cameraParent.transform.Rotate(Input.GetAxisRaw("Mouse Y") * 0, Input.GetAxisRaw("Mouse X") * 10, 0);
		}


		Camera.main.fieldOfView += (20 * Input.GetAxis("Mouse ScrollWheel") );

		if(Camera.main.fieldOfView < 10){

			Camera.main.fieldOfView = 10;
		}


		if( Input.GetMouseButton(2) ){

			Camera.main.transform.position = defaultPosition;
			cameraParent.transform.rotation = defaultRotation;
			Camera.main.fieldOfView = defaultZoom;
		}
	}
}

