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
        
        //Vars for controlling the playerSpeed/animationSpeed
        public CharacterController charCont;
		public CharacterMotor charMotor; 
        
        //Layer to hold the animations for the walking/running/idle
        public Transform animationHolder;
        
        //Var that takes the speed of the player so I can control the player animation speed based upon that of the player.
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
            //Setting the velocity magnitude to that of the character controller
            velocityMag = charCont.velocity.magnitude;
            AnimSpeedController();
        }
        
        void AnimSpeedController()
        {
            if(Input.GetAxis("Horizontal") && charMotor.isGrounded || Input.GetAxis("Vertical") && charMotor.isGrounded)
            {
                if (Input.GetKey(KeyCode.LeftShift) && charMotor.isGrounded)
                {
                    //Controls walking speed/animation
                    animationHolder.animation["Run"].speed = velocityMag / playerStats.runSpeed * 1.2f;
                    animationHolder.animation.Crossfade("Run");
                    charMotor.movement.maxForwardSpeed = playerStats.walkSpeed;
                    charMotor.movement.maxBackwardsSpeed = playerStats.walkSpeed - 1;
                    charMotor.movement.maxSidewaysSpeed = playerStats.walkSpeed;
                }
                else
                {
                    //Controls running speed/animation
                    animationHolder.animation["Walk"].speed = velocityMag / playerStats.walkSpeed;
                    animationHolder.animation.Crossfade("Walk");
                    charMotor.movement.maxForwardSpeed = playerStats.runSpeed;
                    charMotor.movement.maxBackwardsSpeed = playerStats.runSpeed - 1;
                    charMotor.movement.maxSidewaysSpeed = playerStats.runSpeed;
                }
            }
            else
            {
                //Plays the idle animation when just standing
                animationHolder.animation.Crossfade("Idle");
            }
        }
        
        public void Spawn()
        {
            playerStats.health = 20; 
            playerStats.shields = 100; 
        }
        
        [RPC]
        public void Server_TakeDamage(float damage)
        {
			Client_TakeDamage(damage);
        }
        
        void Client_TakeDamage(float damage)
        {
            if(playerStats.shields > 0)
                playerStats.shields -= damage;
            else
                playerStats.health -= damage; 
            
            if(playerStats.health <= 0)
                Die();
        }
        
        void Die()
        {
            
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
        //Information for player physical stats
        public float health;
        public float shields;
        public float stamina;
        
        //Score stuff, needs to be synced with the networkmanager
        public int kills;
        public int deaths;
        public int scores;
        
        //Speed value holders
        public int walkSpeed;
        public int runSpeed;
        
        //Helpful bool for displaying GUI when dead. Set to false when dead and true when alive
        public bool isAlive;
    }
}
