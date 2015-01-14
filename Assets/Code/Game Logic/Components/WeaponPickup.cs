using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponPickup : Photon.MonoBehaviour 
{
	public Transform player; 
	public List<GroundWeapon> floorWeapons = new List<GroundWeapon>(); 

	void Start () 
	{

	}

	void Update () 
	{
		if(photonView.isMine)
		{
			if(Input.GetKeyDown (KeyCode.E))
			{
				Collider[] weapons = Physics.OverlapSphere(player.position, 2.0f);
				foreach (Collider Go in weapons)
				{
					if(Go.gameObject.tag == "Weapon")
					{
						GroundWeapon weapon = Go.gameObject.transform.GetComponent<GroundWeapon>();
						if(WeaponManager.instance.curWepList[0] != WeaponManager.instance.weaponsList[weapon.weaponNumber] && WeaponManager.instance.curWepList[1] != WeaponManager.instance.weaponsList[weapon.weaponNumber])
						{
							Instantiate(floorWeapons[WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].weaponNumber], player.position, Quaternion.Euler(Random.Range (0,30), Random.Range(0,30), Random.Range(0,30)));
							WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].gameObject.SetActive(false);
							WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon] = WeaponManager.instance.weaponsList[weapon.weaponNumber];
							WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].weapon.bulletsPerMag = WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].weapon.bulletsPerMagStart;
							WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].gameObject.SetActive(true);
							weapon.Destroy(); 
						}
					}
				}
			}
		}
	}
}
