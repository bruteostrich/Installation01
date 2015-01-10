using UnityEngine;
//using System;
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
		public static Player instance;

		//For controlling the playerSpeed 
		public CharacterController charCont;
		public CharacterMotor charMotor;

		public int walkSpeed = 7;
		public int runSpeed = 9;

		//For holding the animations as well as getting the speed to play them at. 
		public Transform WalkAnimationHolder;
		public float velocityMag;

        void Start()
        {
	    	instance = this; 
            InitializeInternals();
            Screen.lockCursor = true;
            Screen.showCursor = false;
        }

        void FixedUpdate()
        {
			velocityMag = charCont.velocity.magnitude;
			SpeedController ();
        }

		void Update()
		{

		}

		void SpeedController()
		{
			if(Input.GetAxis("Horizontal") != 0 && charCont.isGrounded || Input.GetAxis("Vertical") != 0 && charCont.isGrounded)
			{
				if(Input.GetKey (KeyCode.LeftShift) && charCont.isGrounded)
				{
					charMotor.movement.maxForwardSpeed = runSpeed;
					charMotor.movement.maxSidewaysSpeed = runSpeed;
					charMotor.movement.maxBackwardsSpeed = runSpeed - 1;
					charMotor.movement.maxGroundAcceleration = 25;
					
					WalkAnimationHolder.animation["Run"].speed = velocityMag / runSpeed ;
					WalkAnimationHolder.animation.CrossFade("Run",0.2f);
				}
				else
				{
					charMotor.movement.maxForwardSpeed = walkSpeed;
					charMotor.movement.maxSidewaysSpeed = walkSpeed;
					charMotor.movement.maxBackwardsSpeed = walkSpeed - 1;
					charMotor.movement.maxGroundAcceleration = 50;

					WalkAnimationHolder.animation["Walk"].speed = velocityMag / walkSpeed ;
					WalkAnimationHolder.animation.CrossFade("Walk",0.2f);
				}
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
           // Manager = GameManagerLocator.Manager;
        }
    }
}
