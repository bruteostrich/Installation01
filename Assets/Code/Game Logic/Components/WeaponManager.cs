using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour 
{
	public static WeaponManager instance; 
	public List<Weapon> weaponsList = new List<Weapon>();
	public List<Weapon> curWepList = new List<Weapon>(); 
	public int curWeapon; 

	void Start () 
	{
		instance = this; 
		curWeapon = 0;
		curWepList [curWeapon].gameObject.SetActive (true);
		audio.PlayOneShot(curWepList[curWeapon].weapon.draw);
		curWepList[curWeapon].DrawGun ();
	}
	

	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
			if(curWeapon == 0)
				curWeapon = 1;
			else
				curWeapon = 0;

			foreach (Weapon we in curWepList)
			{
				we.isReloading = false;
				we.isMelee = false; 
				if(we != curWepList[curWeapon])
					we.gameObject.SetActive(false);
				else
				{
					we.gameObject.SetActive (true);
					StartCoroutine(we.DrawGun());
				}
			}
		}
	}
}
