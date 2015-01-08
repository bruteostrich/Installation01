using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour 
{
	public static NetworkPlayer instance; 
    public bool offline = false;
	public PlayerStats playerStats; 
    // The third person character model seen by other players across the network.
    public GameObject firstPersonObject;
    public GameObject thirdPersonObject;
	    
    private void Awake ()
    {
        // Keep this gameobject between all scenes (e.g. when a map change occurs we still want the players to exist)
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update ()
    {
        this.thirdPersonObject.transform.position = this.firstPersonObject.transform.position - new Vector3 (0,1,0);
        this.thirdPersonObject.transform.rotation = this.firstPersonObject.transform.rotation;
    }

    private void Start ()
    {
		instance = this; 
        if (offline == true)
            return;

        // if this is the local player
        if (photonView.isMine == true) 
        {
            // disable the third person model
            firstPersonObject.SetActive(true);
            this.thirdPersonObject.SetActive(false);
            this.GetComponent<GameLogic.Player>().enabled = true;
			// make sure all other player componenents are enabled
            //foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                //childCompnent.enabled = true;
        }
        // if this is the remote player
        else
        {
            // enable the third person model
            firstPersonObject.SetActive(false);
            this.thirdPersonObject.SetActive(true);
            this.GetComponent<GameLogic.Player>().enabled = false;
            // make sure all other player components are disabled
            //foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
              //  childCompnent.enabled = false;
        }
    }

    [RPC]
    private void GetHit (float damage)
    {
        if(playerStats.shields > 0)
			playerStats.shields -= damage;
        else
			playerStats.health -= damage; 

		if (playerStats.health <= 0)
        {
			Spawn ();
        }
    }

	void Spawn()
	{
		Debug.Log("You Have Died!!");
		playerStats.shields = 100;
		playerStats.health = 20;
		this.firstPersonObject.transform.position = new Vector3(0, 5, 0);
	}

    private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
    {
        if (offline == true)
            return;

        // if this is the local player
        if (stream.isWriting) 
        {
            // local player, send our position info
            stream.SendNext(firstPersonObject.transform.position);
            // local player, send our rotation info
            stream.SendNext(firstPersonObject.transform.rotation);
        }
        // if this is the remote player
        else 
        {
            // remote player, receive position info
            this.firstPersonObject.transform.position = (Vector3)stream.ReceiveNext();
            // remote player, receive rotation info
            this.firstPersonObject.transform.rotation = (Quaternion)stream.ReceiveNext(); 
        }
    }

	void OnGUI()
	{
        if (photonView.isMine)
        {
            GUI.Box(new Rect(Screen.width / 3 - 5, 10, Screen.width / 3 + 10, 30), "");
            GUI.Box(new Rect(Screen.width / 3, 15, (playerStats.shields / 100) * Screen.width / 3, 20), "");

            GUI.Box(new Rect(Screen.width / 2 - 8, Screen.height / 2 - 8, 16, 16), "");
        }
	}
}

[System.Serializable]
public class PlayerStats
{
	public float health;
	public float shields;
	public float stamina;
	
	public int kills;
	public int deaths;
	public int score; 
	
	public bool isAlive;
}
