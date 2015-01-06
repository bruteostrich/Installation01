using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class NetworkManager : Photon.MonoBehaviour
{
    [Header("Network Information")]
    public string version = "Installation 01 v01";      // string used by photon to seperate different builds of the game, so that old builds wont work with new ones.
    public Color infoColor;                             // color of network debug text in the top right corner of screen (debug only)

    public GameObject PlayerPrefab;                     // The player prefab from the resources folder

    // Default / Fallback room properties
    private string name = "Installation 01 room";       // The default room name
    private int maxPlayers = 16;                        // The default max players
    private bool visible = true;                        // Whether or not the room is visible on the network
    private bool open = true;                           // If the room is visible, is it open or private
    public bool showBrowser = false;

    private void Awake()
    {
        // Keep gameobject between scenes to handle network events
        DontDestroyOnLoad(this.gameObject);
        this.ConnectToServer();
    }

    #region For Testing / Debugging Only

    private void OnGUI()
    {
        // This is strictly for network debugging (do not ship)
        GUI.color = this.infoColor;
        GUILayout.Label(" " + PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.inRoom)
        {
            GUILayout.Space(10);
            GUILayout.Label(" Playerlist: ");

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                GUI.color = Color.white;
                GUILayout.Label(" Installation 01 Team Member " + player.name, "box");
                GUI.color = this.infoColor;
            }

        }
        else
        {
            GUILayout.Label(" Players Online: " + PhotonNetwork.countOfPlayers);
            GUILayout.Label(" Players in Rooms: " + PhotonNetwork.countOfPlayersInRooms);
        }

        if (this.showBrowser == true && PhotonNetwork.insideLobby)
        {
            GUI.color = Color.white;
            GUILayout.BeginArea (new Rect (Screen.width / 2, 0, Screen.width / 2, Screen.height));

            GUILayout.Label("Roomlist (this ugly interface is temporary, ignore its ugliness)", "Box");

            if (PhotonNetwork.GetRoomList().Length < 1)
            {
                GUILayout.Label("There are currently no rooms, check back later", "Box");

                if (GUILayout.Button("Create new room"))
                    this.JoinRandomRoom();
            }
            else
            {
                if (GUILayout.Button("Join random room"))
                    this.JoinRandomRoom();
            }

            foreach (RoomInfo room in PhotonNetwork.GetRoomList())
            {
                if (GUILayout.Button (room.name.ToString()))
                    PhotonNetwork.JoinRoom (room.name);
            }

            GUILayout.EndArea ();
        }
        // This is strictly for network debugging (do not ship)
    }

    public void EnableServerList ()
    {
        // This is strictly for network debugging (do not ship)
        if (this.showBrowser == false)
            this.showBrowser = true;
        else
            this.showBrowser = false;
        // This is strictly for network debugging (do not ship)
    }

    public void DisableServerList ()
    {
        // This is strictly for network debugging (do not ship)
        this.showBrowser = false;
        // This is strictly for network debugging (do not ship)
    }

    #endregion

    public void ConnectToServer()
    {
        // If a connection already exists do nothing
        if (PhotonNetwork.connected)
            return;

        // set basic photon network parameters
        PhotonNetwork.offlineMode = false;
        PhotonNetwork.autoCleanUpPlayerObjects = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = true;

        // Connect to best available server using the version variable creaed earlier
        PhotonNetwork.ConnectToBestCloudServer(this.version);
    }

    public void DisconnectFromServer()
    {
        // If we are not connected to the server, do nothing
        if (!PhotonNetwork.connected)
            return;

        // Disconnect from the photon server
        PhotonNetwork.LeaveLobby();
    }

    public void JoinRoom(string room)
    {
        // Join a specific room
        PhotonNetwork.JoinRoom(room);
    }

    public void JoinRandomRoom()
    {
        // Join a random room on the network
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        // If we are not in a room, do nothing
        if (!PhotonNetwork.inRoom)
            return;

        // Leave the network room
        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom(string name, int maxplayers, bool open, bool visible)
    {
        // Create a new room using the given parameters
        RoomOptions options = new RoomOptions() { maxPlayers = maxplayers, isOpen = open, isVisible = visible };
        PhotonNetwork.CreateRoom(name, options, TypedLobby.Default);
    }

    private void OnPhotonRandomJoinFailed()
    {
        // If we fail to join a random room, create a new room
        this.CreateRoom(this.name, this.maxPlayers, this.open, this.visible);
    }

    private void OnJoinedRoom()
    {
        //GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("GameEntity");
        //int spawnpoint = Random.Range(0, spawnpoints.Length);
        //GameObject player = PhotonNetwork.Instantiate("Player", spawnpoints[spawnpoint].transform.position, Quaternion.identity, 0);
        
        this.SpawnPlayer();
    }

    private void Loadlevel ()
    {
        Application.LoadLevel("DevelopmentScene");
    }

    private void SpawnPlayer ()
    {
        GameObject player = PhotonNetwork.Instantiate("Player Controller", new Vector3(0, 10, 0), Quaternion.identity, 0);

        this.Loadlevel();
    }
}