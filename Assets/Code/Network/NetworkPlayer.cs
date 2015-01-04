using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour 
{
    // The third person character model seen by other players across the network.
    public GameObject PlayerModel;

    private void Awake ()
    {
        // Keep this gameobject between all scenes (e.g. when a map change occurs we still want the players to exist)
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start ()
    {
        // if this is the local player
        if (photonView.isMine == true) 
        {
            // disable the third person model
            PlayerModel.SetActive(false); 

            // make sure all other player componenents are enabled
            foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                childCompnent.enabled = true;
        }
        // if this is the remote player
        else
        {
            // enable the third person model
            PlayerModel.SetActive(true);

            // make sure all other player components are disabled
            foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                childCompnent.enabled = false;
        }
    }

    private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
    {
        // if this is the local player
        if (stream.isWriting) 
        {
            // local player, send our position info
            stream.SendNext(transform.position);
            // local player, send our rotation info
            stream.SendNext(transform.rotation);
        }
        // if this is the remote player
        else 
        {
            // remote player, receive position info
            this.transform.position = (Vector3)stream.ReceiveNext();
            // remote player, receive rotation info
            this.transform.rotation = (Quaternion)stream.ReceiveNext(); 
        }
    }
}
