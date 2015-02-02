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
		public static Player instance;

		//For controlling the playerSpeed 
		public CharacterController charCont;
		public CharacterMotor charMotor;

		public float walkSpeed = 5.2f;
		public float runSpeed = 6.7f;

		//For holding the animations as well as getting the speed to play them at. 
		public Transform WalkAnimationHolder;
		public Transform CamAnimationHolder;
		public float velocityMag;

		private bool left; 
		private bool right;

        void Start()
        {
	    	instance = this; 
            InitializeInternals();
            Screen.lockCursor = true;
            Screen.showCursor = false;

			left = true;
			right = false;
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
					
					if(!WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isReloading && !WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isMelee && WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].weapon.fireRateCooler == 0 && GrenadeManager.instance.timer == 0)
					{
						WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation["Walk"].speed = velocityMag / runSpeed ;
						WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation.CrossFade("Walk",0.2f);
					}

					if(left == true)
					{
						if(!CamAnimationHolder.animation.isPlaying)
						{
							CamAnimationHolder.animation.Play("walkLeft1");
							left = false;
							right = true;
						}
					}
					if(right == true)
					{
						if(!CamAnimationHolder.animation.isPlaying)
						{
							CamAnimationHolder.animation.Play("walkRight1");
							right = false;
							left = true;
						}
					}
				}
				else
				{
					charMotor.movement.maxForwardSpeed = walkSpeed;
					charMotor.movement.maxSidewaysSpeed = walkSpeed;
					charMotor.movement.maxBackwardsSpeed = walkSpeed - 1;
					charMotor.movement.maxGroundAcceleration = 50;

					if(!WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isReloading && !WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isMelee && WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].weapon.fireRateCooler == 0 && GrenadeManager.instance.timer == 0)
					{
						WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation["Walk"].speed = velocityMag / walkSpeed ;
						WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation.CrossFade("Walk",0.2f);
					}
				}
			}
			else
			{
				if(!WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isReloading && !WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].isMelee && WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].weapon.fireRateCooler == 0 && GrenadeManager.instance.timer == 0)
					WeaponManager.instance.curWepList[WeaponManager.instance.curWeapon].animation.CrossFade("Idle",0.2f);
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
