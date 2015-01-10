using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using Helpers;


namespace GameLogic
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Grenade : MonoBehaviour
    {
		private GameManager Manager;
		public GameObject Explosion;

		public float timer = 3; 
	
        void Start()
        {

        }

        void Update()
        {
			if(timer > 0)
				timer -= Time.deltaTime; 

			if(timer <= 0)
			{
				Instantiate(Explosion, transform.position,transform.rotation);

				Collider[] hits = Physics.OverlapSphere(transform.position, 10);
				foreach (Collider co in hits)
				{
					if (co.gameObject.transform.root.tag == "Player")
					{
						Debug.Log ("DAMAGE");
						PhotonView photonview = co.gameObject.transform.root.GetComponent<PhotonView>();
						if (photonview.isMine)
							return;

						photonview.RPC("GetHit", PhotonTargets.AllBufferedViaServer, 100.0f);
					}
				}
				DestroyObject(gameObject);
			}
        }

        void FixedUpdate()
        {

        }
    }
}