using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class photonRoomController : Photon.MonoBehaviour {

	public List<Transform> spawnPoints = new List<Transform>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button (new Rect (110, 70, 100, 30), "Spawn me motherfucker") && !staticData.alive) {
			spawnMe();
		}
	}

	public void spawnMe(){
		staticData.alive = true;
		Transform sPoint = spawnPoints [Random.Range (0, spawnPoints.Count)];
		PhotonNetwork.Instantiate ("testtank", sPoint.position, sPoint.rotation, 0); 
	}
}
