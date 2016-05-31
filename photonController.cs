using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class photonController : MonoBehaviour {

	public string networkStatus;
	AsyncOperation levelSync;
	public bool levelLoaded = false;
	public string levelName = "loadleveltest";


	// Use this for initialization
	void Start () {
		
		networkStatus = "Connecting to Network";
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	public void setupPlayerStats(){
			ExitGames.Client.Photon.Hashtable stats = new ExitGames.Client.Photon.Hashtable ();  // using PUN's implementation of Hashtable
			stats ["k"] = 0; //kills
			stats ["d"] = 0; //deaths
			stats ["a"] = 0; //assists
			stats ["s"] = 0; //score
			PhotonNetwork.player.SetCustomProperties (stats, null, false);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (PhotonNetwork.player.customProperties["d"]);
	}

	public void joinRandomGame(){
		networkStatus = "Connecting to Random Game";
		PhotonNetwork.JoinRandomRoom();
	}

	public void OnConnectedToMaster(){
		networkStatus = "masterConnected";
		PhotonNetwork.playerName = "KaizerKing88";
	}

	void OnPhotonRandomJoinFailed()
	{
		
	}

	void OnLeftRoom(){
		destroyLevel ();
	}

	void OnJoinedRoom(){
		setupPlayerStats ();
		StartCoroutine ("loadLevel");
		PhotonNetwork.Instantiate ("Cube", Vector3.zero, Quaternion.identity,0);
	}

	IEnumerator loadLevel() {//load level-props when player is in a room
		levelSync = Application.LoadLevelAdditiveAsync(levelName);
		//Debug.Log("sync: "+levelSync.progress.ToString());
		yield return levelSync;
		Debug.Log("Loading complete");
		levelLoaded = true;
	}

	public void leaveRoom(){
		PhotonNetwork.LeaveRoom ();
		levelLoaded = false;
	}

	public void destroyLevel(){ //remove all level-props when player left the room
		GameObject.Destroy(GameObject.Find("level"));
	}

	public RoomInfo[] getAllRooms(){ //Returns list array
		 return PhotonNetwork.GetRoomList ();
	}

	public void createRoom(string roomName, byte maxPlayers){
		ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable() { { "map", "loadleveltest" } };
		string[] roomPropsInLobby = { "map", "loadleveltest" };
		RoomOptions roomOptions = new RoomOptions () {
			customRoomProperties = roomProps,
			customRoomPropertiesForLobby = roomPropsInLobby,
			maxPlayers = maxPlayers
		};
		PhotonNetwork.CreateRoom (roomName, roomOptions, TypedLobby.Default);

	}



	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		if (GUI.Button (new Rect (10, 70, 100, 30), "join game")) {
			createRoom ("test",4);
		}

		if (GUI.Button (new Rect (10, 130, 100, 30), "leave game")) {
			leaveRoom ();
		}


		if (GUI.Button (new Rect (10, 180, 100, 30), "random game")) {
			joinRandomGame ();
		}
	}
}
