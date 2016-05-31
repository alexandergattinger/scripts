using UnityEngine;
using System.Collections;

public class projController : Photon.MonoBehaviour {

	Vector3 lastPosition;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
		StartCoroutine ("destroyAfter");
	}
	IEnumerator destroyAfter(){
		yield return new WaitForSeconds (5);
		PhotonNetwork.Destroy (photonView);
	}


	// Update is called once per frame
	void Update () {
		control ();
		collisionDetection ();
	}

	void control(){
		transform.Translate (Vector3.forward * Time.deltaTime * 50);
	}

	void collisionDetection(){
		if(Physics.Raycast(lastPosition, (transform.position-lastPosition).normalized,out hit,Vector3.Distance(transform.position,lastPosition))){
			if (hit.collider.GetComponent<PhotonView> ()) {
				hit.collider.GetComponent<PhotonView> ().RPC ("receiveDamage", PhotonTargets.All, "50_" + PhotonNetwork.player.ID.ToString ());
			}
			Debug.Log (hit.collider.name);
			PhotonNetwork.Destroy (GetComponent<PhotonView> ());
		}
		lastPosition = transform.position;
	}
}
