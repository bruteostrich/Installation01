using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour 
{
    public bool offline = false;
    // The third person character model seen by other players across the network.
    public GameObject firstPersonObject;
    public GameObject thirdPersonObject;

    public float health = 100;
    
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
    private void GetHit ()
    {
        this.health -= 10;

        if (this.health <= 0)
        {
            Debug.Log("You Have Died!!");
            this.health = 100;
            this.firstPersonObject.transform.position = new Vector3(0, 5, 0);
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
}
