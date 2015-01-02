using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ExitGames.Client.Photon;

public class NetworkManager : Photon.MonoBehaviour 
{
    public string version = "installation 01"; // string used by photon to seperate different builds of the game, so that old builds wont work with new ones.
    public Text connectionStateText;

    private void Awake ()
    {
        DontDestroyOnLoad (this.gameObject);
        this.ConnectToServer ();
    }

    private void Update ()
    {
        if (this.connectionStateText)
            this.connectionStateText.text = PhotonNetwork.connectionStateDetailed.ToString ();
    }

    /// <summary>
    /// Established initial connection to the photon server, returns if we are already connected to the server.
    /// </summary>
    public void ConnectToServer ()
    {
        if (PhotonNetwork.connected)
            return;

        PhotonNetwork.offlineMode = false;
        PhotonNetwork.autoCleanUpPlayerObjects = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = true;

        PhotonNetwork.ConnectToBestCloudServer (this.version);
    }

    /// <summary>
    /// Disconnects from the photon server, returns if we are not connected to the photon server.
    /// </summary>
    public static void DisconnectFromServer ()
    {
        if (!PhotonNetwork.connected)
            return;

        PhotonNetwork.LeaveLobby ();
    }

    /// <summary>
    /// Join room on the photon server.
    /// </summary>
    /// <param name="room"></param>
	public static void JoinRoom (string room)
    {
        PhotonNetwork.JoinRoom (room);
    }

    /// <summary>
    /// Joins first available room on the photon network.
    /// </summary>
    public static void JoinRandomRoom ()
    {
        PhotonNetwork.JoinRandomRoom ();
    }

    /// <summary>
    /// Leave the current room, returns if we are not in a room.
    /// </summary>
    public static void LeaveRoom ()
    {
        if (!PhotonNetwork.inRoom)
            return;

        PhotonNetwork.LeaveRoom ();
    }

    /// <summary>
    /// Creates a new room on the photon server.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxplayers"></param>
    /// <param name="open"></param>
    /// <param name="visible"></param>
    public static void CreateRoom (string name, int maxplayers, bool open, bool visible)
    {
        RoomOptions options = new RoomOptions() { maxPlayers = maxplayers, isOpen = open, isVisible = visible };
        PhotonNetwork.CreateRoom (name, options, TypedLobby.Default);
    }
}
