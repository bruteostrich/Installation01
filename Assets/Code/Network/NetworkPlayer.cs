using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour 
{
    public GameObject PlayerModel;

    private void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start ()
    {
        if (photonView.isMine == true)
        {
            PlayerModel.SetActive(false);

            foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                childCompnent.enabled = true;
        }
        else
        {
            PlayerModel.SetActive(true);

            foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                childCompnent.enabled = false;
        }
    }

    private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // local player, send our position info
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // remote player, receive position info
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
