using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class photonPlayer : MonoBehaviour {

	const int healthOrigin=100;
	const float damagedByRemoveTimer = 6;
	public int health = 100;
	PhotonView view;
	List<string> damagedBy = new List<string>();

	// Use this for initialization
	void Start () {
		view = GetComponent<PhotonView> ();
		if (view.isMine) {
			//setupPlayerStats ();
			staticData.alive = true;
		}
		//PhotonNetwork.sendRate ();
		//PhotonNetwork.sendRateOnSerialize();
	}
	
	// Update is called once per frame
	void Update () {
		killMe ();
	}

	IEnumerator damagedByRemover() {
		yield return new WaitForSeconds(damagedByRemoveTimer);
		damagedBy.Clear ();
	}

	[PunRPC]
	public void receiveDamage(string data){ //data comes in form of damage_playerPhotonID
		if (view.isMine) {
			damagedBy.Add (data);
			string[] buffer = data.Split ('_');
			healthController (int.Parse (buffer [0]));
		}
	}

	public void killMe(){
		if (Input.GetKey ("k")) {
			health = 0;
		}
	}

	public void healthController(int dmg){
		if (view.isMine) {
			health -= dmg;
			if (health <= 0) {
				//DIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIEDIE
				deathController();
			}
		}
	}

	void deathController(){
		staticData.alive = false;
		ExitGames.Client.Photon.Hashtable stats = new ExitGames.Client.Photon.Hashtable ();  // using PUN's implementation of Hashtable
		stats ["k"] = 0; //kills
		stats ["d"] = ((int)PhotonNetwork.player.customProperties["d"] + 1); //deaths
		stats ["a"] = 0; //assists
		stats ["s"] = 0; //score
		PhotonNetwork.player.SetCustomProperties (stats, null, false);
		PhotonNetwork.Destroy (view);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(health);
		}
		else
		{
			// Network player, receive data
			this.transform.position = (Vector3) stream.ReceiveNext();
			this.transform.rotation = (Quaternion) stream.ReceiveNext();
			this.health = (int) stream.ReceiveNext();
		}
	}

	void OnGUI(){
		if (GUI.Button (new Rect (400, 70, 100, 30), "kill me now!")) {
			deathController ();
		}


		GUI.Button (new Rect (400, 700, health*2, 30), health.ToString() + " Life");
	}
}
