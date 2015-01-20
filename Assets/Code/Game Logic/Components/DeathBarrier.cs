using UnityEngine;
using System.Collections;

public class DeathBarrier : MonoBehaviour 
{
    private void OnTriggerEnter (Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.position = new Vector3(0, 5, 0);
        }
    }
}