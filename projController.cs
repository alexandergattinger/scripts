using UnityEngine;
using System.Collections;

public class projController : Photon.MonoBehaviour {

	Vector3 lastPosition;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		control ();
		collisionDetection ();
	}

	void control(){
		transform.Translate (Vector3.forward * Time.deltaTime * 5);
	}

	void collisionDetection(){
		if(Physics.Raycast(lastPosition, (transform.position-lastPosition).normalized,out hit,Vector3.Distance(transform.position,lastPosition))){
			hit.collider.GetComponent<PhotonView> ().RPC ("receiveDamage", PhotonTargets.All,"50_" + PhotonNetwork.player.ID.ToString());
			PhotonNetwork.Destroy (GetComponent<PhotonView> ());
		}
		lastPosition = transform.position;
	}
}
