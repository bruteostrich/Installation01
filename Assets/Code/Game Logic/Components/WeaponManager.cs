using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour 
{
	public List<Weapon> weaponsList = new List<Weapon>(); 
	public int curWeapon; 

	void Start () 
	{
		curWeapon = 0;
		weaponsList [curWeapon].gameObject.SetActive (true);
		audio.PlayOneShot(weaponsList[curWeapon].weapon.draw);
		weaponsList[curWeapon].DrawGun ();
	}
	

	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
			if(curWeapon == 0)
				curWeapon = 1;
			else
				curWeapon = 0;

			foreach (Weapon we in weaponsList)
			{
				we.isReloading = false;
				we.isMelee = false; 
				if(we != weaponsList[curWeapon])
					we.gameObject.SetActive(false);
				else
				{
					we.gameObject.SetActive (true);
					StartCoroutine(we.DrawGun());
					if(we.weapon.bulletsPerMag == 0 && we.isReloading == false && we.weapon.spareBullets != 0)
					{
						StartCoroutine(weaponsList[curWeapon].Reload());
						we.ammoHolder = we.weapon.bulletsPerMag;
						we.fullReload = true;
						we.isReloading = true;
					}
				}
			}
		}
	}
}
