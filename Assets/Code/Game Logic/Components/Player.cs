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
            AnimSpeedController();
        }
        
        void AnimSpeedController()
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
        public float health;
        public float shields;
        public float stamina;
        
        public int kills;
        public int deaths;
        public int scores;
        
        public bool isAlive;
    }
}
