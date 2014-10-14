﻿using UnityEngine;
using System.Collections;

namespace GameLogic
{
    public class Projectile : MonoBehaviour
    {
        #region Members
        public string TraitsFile;
        protected ProjectileTraits Traits;
        protected LineRenderer DebugPathRenderer;

        private Vector3 Direction;
        public Vector3 ProjectileDirection
        {
            get
            {
                return Direction;
            }
        }
        #endregion


        void Start()
        {
            InitializeDebugPathRenderer();

            InitializeTraits();
        }

        void Update()
        {
            Traits.Lifespan--;
            if(Traits.Lifespan == 0)
            {
                Destroy(this);
            }
            else
            {
                Vector3 VelocityVector = Direction * Traits.Velocity;
                transform.position += VelocityVector; // simple velocity per frame in Direction.
                //things like bullet drop will come later
            }
        }

        /// <summary>
        /// Sets Direction of this projectile
        /// </summary>
        /// <param name="Direction"></param>
        void SetDirection(Vector3 Direction)
        {
            this.Direction = Direction;
            this.Direction.Normalize(); // Pre-Normalize
        }


        private void InitializeDebugPathRenderer()
        {
#if DEBUG
            gameObject.AddComponent(typeof(LineRenderer));
            DebugPathRenderer = GetComponent<LineRenderer>();
            DebugPathRenderer.SetColors(Color.red, Color.red);
            DebugPathRenderer.SetWidth(2.0f, 2.0f);
#endif
        }

        private void InitializeTraits()
        {
            if(TraitsFile != string.Empty)
            {
                Traits = ProjectileTraits.LoadFromFile(TraitsFile);
            }
        }
    }
}