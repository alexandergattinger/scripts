using UnityEngine;
using System.Collections;

public class weaponController : MonoBehaviour {

	public float cd;
	bool allowedToShoot = true;
	public Transform shootFrom;
	public GameObject proj;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		shooting ();
	}

	IEnumerator shootCD() {
		yield return new WaitForSeconds(.2f);
		allowedToShoot = true;
	}

	void shooting(){
		if (Input.GetMouseButton (0)) {
			if (allowedToShoot) {
				allowedToShoot = false;
				StartCoroutine ("shootCD");
				PhotonNetwork.Instantiate ("proj", shootFrom.transform.position, shootFrom.transform.rotation, 0);
			}
		}
	}
}
