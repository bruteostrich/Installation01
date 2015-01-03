using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour 
{
    private void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start ()
    {
        if (photonView.isMine == true)
        {
            foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                childCompnent.enabled = true;

            /*this.gameObject.GetComponentsInChildren(System.Type ty)
            this.GetComponent<CharacterController>().enabled = true;
            this.GetComponent<vp_FPController>().enabled = true;
            this.GetComponent<vp_FPInput>().enabled = true;
            this.GetComponent<vp_FPPlayerEventHandler>().enabled = true;
            this.GetComponent<vp_FPWeaponHandler>().enabled = true;
            this.GetComponent<vp_ItemIdentifier>().enabled = true;

            GameObject FPSCamera = this.gameObject.transform.FindChild("FPSCamera").gameObject;
            FPSCamera.SetActive(true);
            FPSCamera.GetComponent<AudioListener>().enabled = true;
            FPSCamera.GetComponent<Camera>().enabled = true;
            FPSCamera.GetComponent<vp_FPCamera>().enabled = true;

            GameObject WeaponCamera = FPSCamera.gameObject.transform.FindChild("WeaponCamera").gameObject;
            WeaponCamera.SetActive(true);
            WeaponCamera.GetComponent<Camera>().enabled = true;

            GameObject AR = FPSCamera.gameObject.transform.FindChild("AR").gameObject;
            AR.SetActive(true);
            AR.GetComponent<AudioSource>().enabled = true;
            AR.GetComponent<vp_FPWeapon>().enabled = true;
            AR.GetComponent<vp_FPWeaponShooter>().enabled = true;
            AR.GetComponent<vp_FPWeaponReloader>().enabled = true;
            AR.GetComponent<vp_ItemIdentifier>().enabled = true;

            GameObject MuzzleFlash = FPSCamera.gameObject.transform.FindChild("ARMuzzleFlash").gameObject;
            MuzzleFlash.SetActive(true);
            MuzzleFlash.GetComponent<vp_MuzzleFlash>().enabled = true;*/
        }
        else
        {
            foreach (Behaviour childCompnent in this.gameObject.GetComponentsInChildren<Behaviour>())
                childCompnent.enabled = false;
        }
    }
}
