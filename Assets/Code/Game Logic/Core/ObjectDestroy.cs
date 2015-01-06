using UnityEngine;
using System.Collections;

public class ObjectDestroy : MonoBehaviour 
{
	public float destroyTime; 

	void Start () 
	{
		Invoke ("Destroy", destroyTime);
	}
	

	void Destroy () 
	{
		DestroyObject (gameObject);
	}
}
