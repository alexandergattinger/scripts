using UnityEngine;
using System.Collections;

public class tankController : Photon.MonoBehaviour {

	Camera myCam;
	public GameObject turret;
	RaycastHit mousehit;
	public Transform shootFrom;

	// Use this for initialization
	void Start () {
		myCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		controls ();
		turretControls ();
		fireControls ();
		cameraControl ();
	}


	void cameraControl(){
		myCam.transform.position = transform.position + Vector3.up * 40 
			+ Vector3.left*-(Input.mousePosition.x - Screen.width/2)/Screen.width*20
		+ Vector3.forward*(Input.mousePosition.y - Screen.height/2)/Screen.height*20;
	}

	void controls(){
		if (Input.GetKey ("w")) {
			transform.Translate (Vector3.forward*Time.deltaTime*10);
		}else if (Input.GetKey ("s")) {
			transform.Translate (-Vector3.forward*Time.deltaTime*10	);
		}

		if (Input.GetKey ("a")) {
			transform.Rotate (-Vector3.up);
		}else if (Input.GetKey ("d")) {
			transform.Rotate (Vector3.up);
		}
	}

	void turretControls() {
		Ray ray = myCam.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast(ray, out mousehit)) {
			staticData.mouseWorldPosition = mousehit.point;
		}
		turret.transform.LookAt (mousehit.point);
	}

	void fireControls(){
		if (Input.GetMouseButtonDown (0)) {
			PhotonNetwork.Instantiate ("proj", shootFrom.position, shootFrom.rotation, 0);
		}
	}
}
