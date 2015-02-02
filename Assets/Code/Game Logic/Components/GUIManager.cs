using UnityEngine;
using System.Collections;

public class GUIManager : Photon.MonoBehaviour 
{
	public static GUIManager instance;

	void Start()
	{
		instance = this; 
	}


}
