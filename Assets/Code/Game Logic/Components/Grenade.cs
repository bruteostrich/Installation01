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

		public float Fuse = 3;
        public float DamageRadius;

        public GameObject ExplosionGameObject;
	
        void Start()
        {

        }

        void Update()
        {
            if (Fuse > 0)
            {
                Fuse -= Time.deltaTime;
            }

			if(Fuse <= 0)
			{
				Instantiate(ExplosionGameObject, transform.position,transform.rotation);

				Collider[] hits = Physics.OverlapSphere(transform.position, 10);
				foreach (Collider co in hits)
				{
                    if (co.gameObject.transform.root.tag == "Player")
                    {
                        Debug.Log("DAMAGE");
                        PhotonView photonview = co.gameObject.transform.root.GetComponent<PhotonView>();
                        if (photonview.isMine)
                            return;

                        photonview.RPC("GetHit", PhotonTargets.AllBufferedViaServer, 100.0f);
                        DestroyObject(gameObject);

                    }
                    else
                    {
                        DestroyObject(gameObject);
                    }

				}
				DestroyObject(gameObject);
			}
        }
    }
}