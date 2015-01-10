using UnityEngine;
using System.Collections;

public class GroundWeapon : MonoBehaviour 
{
	public static GroundWeapon instance;

	public string weaponName; 
	public int weaponNumber;

	void Start () 
	{
		instance = this; 
	}

	public void Destroy () 
	{
		DestroyObject (gameObject);
	}
}
