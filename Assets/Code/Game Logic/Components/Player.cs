using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using Helpers;

namespace GameLogic
{
    public class Player : MonoBehaviour
    {
        private GameManager Manager;
		public PlayerStats playerStats; 

		public CharacterController charCont;
		public CharacterMotor charMotor;

		public Transform WalkAnimationHolder;

		public float velocityMag;

        void Start()
        {
            InitializeInternals();
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
			velocityMag = charCont.velocity.magnitude;

			SpeedController ();
        }

		void SpeedController()
		{
			if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 && charCont.isGrounded)
			{
				charMotor.movement.maxForwardSpeed = playerStats.walkSpeed;
				charMotor.movement.maxSidewaysSpeed = playerStats.walkSpeed;
				charMotor.movement.maxBackwardsSpeed = playerStats.walkSpeed - 1;

				WalkAnimationHolder.animation["Walk"].speed = velocityMag / playerStats.walkSpeed ;
				WalkAnimationHolder.animation.CrossFade("Walk",0.2f);
			}
			else
			{
				WalkAnimationHolder.animation.CrossFade("Idle",0.2f);
			}
		}

        private void InitializeInternals()
        {
            AcquireGameManager();
        }

        private void AcquireGameManager()
        {
            Manager = GameManagerLocator.Manager;
        }
    }

	[System.Serializable]
	public class PlayerStats
	{
		public float health;
		public float shields;
		public float stamina;

		public int walkSpeed;
		public int runSpeed;

		public int kills;
		public int deaths;
		public int score; 

		public bool isAlive;
	}
}