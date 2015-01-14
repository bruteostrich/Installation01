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

	public Transform player;

	public Texture2D crosshair;
	public Texture2D HUDOverlay;
	public Texture2D motionTracker; 

	public int[] spawnWeaponNum; 
	    
    private void Awake ()
    {
        // Keep this gameobject between all scenes (e.g. when a map change occurs we still want the players to exist)
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update ()
    {
        this.thirdPersonObject.transform.position = this.firstPersonObject.transform.position - new Vector3 (0,1,0);
        this.thirdPersonObject.transform.rotation = this.firstPersonObject.transform.rotation;

		if(Input.GetKeyDown(KeyCode.Y))
		{
			PhotonView photonview = transform.root.GetComponent<PhotonView>();
			photonview.RPC("GetHit", PhotonTargets.AllBufferedViaServer, 52.5f);
		}
    }

    private void Start ()
    {
		PhotonNetwork.sendRate = 25;
		PhotonNetwork.sendRateOnSerialize = 25;

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

		if(playerStats.shields < 0)
			playerStats.shields = 0;

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
		playerStats.isAlive = true ; 

		foreach (Weapon we in WeaponManager.instance.curWepList)
		{
			we.gameObject.SetActive(false);
		}

		WeaponManager.instance.curWeapon = 0;
		
		WeaponManager.instance.curWepList[0] = WeaponManager.instance.weaponsList[spawnWeaponNum[0]];
		WeaponManager.instance.curWepList[1] = WeaponManager.instance.weaponsList[spawnWeaponNum[1]];
		
		WeaponManager.instance.curWepList [0].weapon.bulletsPerMag = WeaponManager.instance.curWepList [0].weapon.bulletsPerMagStart;
		WeaponManager.instance.curWepList [1].weapon.bulletsPerMag = WeaponManager.instance.curWepList [1].weapon.bulletsPerMagStart;

		GrenadeManager.instance.grenadeCountGame = GrenadeManager.instance.grenadeCountStart; 

		int spawnPoints = Random.Range(0,SpawnPointManager.instance.spawnPoints.Length -1);
		this.firstPersonObject.transform.position = SpawnPointManager.instance.spawnPoints[spawnPoints].transform.position;
		WeaponManager.instance.curWepList [0].gameObject.SetActive (true);

		if(photonView.isMine)
		{
			this.firstPersonObject.SetActive (true);
		}
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
			if(playerStats.isAlive && !Application.isLoadingLevel)
			{
	            GUI.Box(new Rect(Screen.width / 3 - 5, 10, Screen.width / 3 + 10, 30), "");
	            GUI.Box(new Rect(Screen.width / 3, 15, (playerStats.shields / 100) * Screen.width / 3, 20), "");

	            GUI.DrawTexture(new Rect(Screen.width / 2 - 20, Screen.height / 2 - 20, 40, 40), crosshair);

				GUI.color = new Color(1,1,1,0.2f);
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), HUDOverlay);
				GUI.color = Color.white;
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), motionTracker);
			}

			if(!playerStats.isAlive && !Application.isLoadingLevel)
				GUI.Box (new Rect(Screen.width/2 - 100, 10, 200, 50), "Repawning");

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
