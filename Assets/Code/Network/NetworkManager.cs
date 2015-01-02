using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExitGames.Client.Photon;

public class NetworkManager : Photon.MonoBehaviour
{
    [Header("Network Information")]
    public string version = "installation 01"; // string used by photon to seperate different builds of the game, so that old builds wont work with new ones.
    public Text connectionStateText;
    public Color infoColor;

    [Header("Default Room Properties")]
    public string name = "Installation 01 room";
    public int maxPlayers = 16;
    public bool visible = true;
    public bool open = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        this.ConnectToServer();
    }

    /*private void Update ()
    {
        if (this.connectionStateText)
            this.connectionStateText.text = PhotonNetwork.connectionStateDetailed.ToString ();
    }*/

    private void OnGUI()
    {
        GUI.color = this.infoColor;
        GUILayout.Label(" " + PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label(" Players Online: " + PhotonNetwork.countOfPlayers);
        GUILayout.Label(" Players in Rooms: " + PhotonNetwork.countOfPlayersInRooms);

        if (PhotonNetwork.inRoom)
        {
            GUILayout.Space(10);
            GUILayout.Label(" Playerlist: ");

            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.name.Length < 1)
                    player.name = "Installation 01 Player " + player.ID.ToString();

                GUILayout.Label(player.name);
            }

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
        this.CreateRoom(this.name, this.maxPlayers, this.open, this.visible);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnJoinedRoom()
    {
        Application.LoadLevel("DevelopmentScene");
    }
}
