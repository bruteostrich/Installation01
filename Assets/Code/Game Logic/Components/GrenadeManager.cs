using UnityEngine;
using System.Collections;

public class GrenadeManager : MonoBehaviour 
{
	public static GrenadeManager instance;

	public int grenadeCountStart;
	[HideInInspector]
	public int grenadeCountGame;

	public float throwLength = WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation.clip.length;

	[HideInInspector]
	public float timer;
	public Rigidbody grenade;
	public Transform spawn;

	void Start () 
	{
		instance = this; 
		grenadeCountGame = grenadeCountStart;
		timer = 0;
	}

	void Update () 
	{
		if(timer != 0)
			timer -= Time.deltaTime;

		if(timer < 0)
			timer = 0;

		if(Input.GetMouseButtonDown (1) && timer == 0 && grenadeCountGame > 0 && !WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isMelee && !WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isReloading)
		{
			Debug.Log ("Throw");
			WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation.Rewind ("Throw");
			WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation.Play ("Throw");
			Rigidbody grenadeThrow = Instantiate (grenade, spawn.position, spawn.rotation) as Rigidbody;
			grenadeThrow.rigidbody.AddRelativeForce (Vector3.forward * 1000);

			grenadeCountGame--; 
			timer = 1; 
		}
	}
}
