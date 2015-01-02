using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class NetworkManager : Photon.MonoBehaviour
{
    [Header("Network Information")]
    public string version = "blah blah"; // string used by photon to seperate different builds of the game, so that old builds wont work with new ones.
    public Color infoColor;

    public GameObject PlayerPrefab;

    //[Header("Default Room Properties")]
    private string name = "Installation 01 room";
    private int maxPlayers = 16;
    private bool visible = true;
    private bool open = true;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        this.ConnectToServer();
    }

    private void OnGUI()
    {
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
    }

    /// <summary>
    /// Established initial connection to the photon server, returns if we are already connected to the server.
    /// </summary>
    public void ConnectToServer()
    {
        if (PhotonNetwork.connected)
            return;

        PhotonNetwork.offlineMode = false;
        PhotonNetwork.autoCleanUpPlayerObjects = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = true;

        PhotonNetwork.ConnectToBestCloudServer(this.version);
    }

    /// <summary>
    /// Disconnects from the photon server, returns if we are not connected to the photon server.
    /// </summary>
    public void DisconnectFromServer()
    {
        if (!PhotonNetwork.connected)
            return;

        PhotonNetwork.LeaveLobby();
    }

    /// <summary>
    /// Join room on the photon server.
    /// </summary>
    /// <param name="room"></param>
    public void JoinRoom(string room)
    {
        PhotonNetwork.JoinRoom(room);
    }

    /// <summary>
    /// Joins first available room on the photon network.
    /// </summary>
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Leave the current room, returns if we are not in a room.
    /// </summary>
    public void LeaveRoom()
    {
        if (!PhotonNetwork.inRoom)
            return;

        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// Creates a new room on the photon server.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxplayers"></param>
    /// <param name="open"></param>
    /// <param name="visible"></param>
    public void CreateRoom(string name, int maxplayers, bool open, bool visible)
    {
        RoomOptions options = new RoomOptions() { maxPlayers = maxplayers, isOpen = open, isVisible = visible };
        PhotonNetwork.CreateRoom(name, options, TypedLobby.Default);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnPhotonRandomJoinFailed()
    {
        if (!PhotonNetwork.connected)
            return;

        this.CreateRoom(this.name, this.maxPlayers, this.open, this.visible);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnCreatedRoom ()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    private void SpawnPlayer ()
    {
        GameObject player = PhotonNetwork.Instantiate("Player Controller", new Vector3(0, 10, 0), Quaternion.identity, 0);

        

        this.Loadlevel();
    }
}
