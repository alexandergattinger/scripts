using UnityEngine;
using System.Collections;

public class playerController2D : MonoBehaviour {

	Rigidbody2D rigid;

	Vector3 moveDir;
	float movSpeed = 2;

	public GameObject weapon;

	public Transform target;
	private Vector3 v_diff;
	private float atan2;

	Vector2 mousePos;
	Camera myCam;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		myCam = Camera.main;
	}

	void Update(){
		cameraController ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		controls ();
		rotateChar ();
	}

	void controls(){
		moveDir = Vector3.zero;
		if (Input.GetKey ("w")) {
			moveDir += Vector3.up * Time.fixedDeltaTime;
		}else if (Input.GetKey ("s")) {
			moveDir += Vector3.up * -Time.fixedDeltaTime;
		}

		if (Input.GetKey ("a")) {
			moveDir += Vector3.right * -Time.fixedDeltaTime;
		}else if (Input.GetKey ("d")) {
			moveDir += Vector3.right * Time.fixedDeltaTime;
		}

		rigid.MovePosition (transform.position + moveDir.normalized * Time.fixedDeltaTime * movSpeed);
	}


	void rotateChar() {
		v_diff = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);    
		atan2 = Mathf.Atan2 ( v_diff.y, v_diff.x );
		transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg+90 );
	}

	void cameraController(){
		myCam.transform.position = new Vector3 (transform.position.x+(Input.mousePosition.x/Screen.width),transform.position.y+(Input.mousePosition.y/Screen.height),-10);
	}



}
