using UnityEngine;
using System.Collections;

public class SpawnPointManager : MonoBehaviour 
{
	public static SpawnPointManager instance;
	public GameObject[] spawnPoints;

	void Start () 
	{
		instance = this;
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
	}

	void Update () 
	{
	
	}
}
