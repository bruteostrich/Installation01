using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using Helpers;

using UnityDebug  = UnityEngine.Debug;
using UnityRandom = UnityEngine.Random;

namespace GameLogic
{
    public class GameManager : MonoBehaviour
    {
        public GameMode InitialMode;
        private GameMode Mode;

        public string[] StartupAssets;
        private SpawnPointCollection SpawnPoints;

        private bool _AssetsLoaded;
        /// <summary>
        /// Gets a value indicating wether the manager has loaded all assets they are required for gameplay to start.
        /// </summary>
        public bool AssetsLoaded
        {
            get
            {
                return _AssetsLoaded;
            }
        }
        /// <summary>
        /// Gets a value indicating wether the GameManager has initialized all of the internal objects required for the game to start
        /// </summary>
        public bool InternalsInitialized
        {
            get
            {
                return _InternalsInitialized;
            }
        }
        private bool _InternalsInitialized;
        /// <summary>
        /// Gets a value indicating wether the game is ready to start
        /// </summary>
        public bool GameReadyToStart
        {
            get
            {
                if (_AssetsLoaded && _InternalsInitialized)
                    return true;

                return false;
            }
        }

        private const string PrefabsDirectory = "Prefabs/"; // Game's prefabs directory

        void Awake()
        {
            InitializeInternals();

            GameObject.DontDestroyOnLoad(this); // the game manager is persistant and needs to survive loading and unloading scenes
        }
        
        void Start()
        {
            
        }

        void Update()
        {

        }

        private void UpdateState()
        {

        }

        /// <summary>
        /// Initializes all objects/resources that GameManager needs to use
        /// </summary>
        private void InitializeInternals()
        {

            SpawnPoints = new SpawnPointCollection();
            SpawnPoints.CacheAll();

            LoadAssets();

            _InternalsInitialized = true;
        }
        /// <summary>
        /// Loads all assets that are critical and required before gameplay can start
        /// </summary>
        private void LoadAssets()
        {
            if(StartupAssets.Length > 0)
            {
                for(int i = 0; i < StartupAssets.Length; ++i)
                {
                    string AssetPath = PrefabsDirectory + StartupAssets[i];
                    LoadGameObject(AssetPath, StartupAssets[i]);
                }
            }
            _AssetsLoaded = true;
        }

        /// <summary>
        /// Loads a GameObject from the ResourcesFolder and adds it to the Resource Cache
        /// </summary>
        /// <param name="GameObjectFile">Name of game object files</param>
        /// <param name="GameObjectHandle">Name of the resource entry in the resource cache</param>
        public void LoadGameObject(string GameObjectFile,string GameObjectHandle)
        {
            //if(ResourceCache.ContainsKey(GameObjectHandle))
            //{
            //    UnityDebug.Log("GameManager is attempting to load: " + GameObjectHandle + " Twice. Blocked.");
            //    return;
            //}
            GameObject LoadedObject = Resources.Load(GameObjectFile) as GameObject;
            //AddObjectToResourceCache(GameObjectHandle, LoadedObject);
        }

        /// <summary>
        /// Caches all game entities in the world
        /// </summary>
        private void CacheAllGameEntities()
        {
            GameObject[] Entities = GameObject.FindGameObjectsWithTag("GameEntity");

            if(Entities.Length == 0)
            {
                Debug.LogError("GAME MANAGER MESSAGE: FATAL ERROR: NO GAME ENTITIES IN SCENE. HALTING");
                Debug.Break();
                return;
            }

            for (int i = 0; i < Entities.Length; ++i )
            {
                //AddGameEntityCacheEntry(Entities[i]);
            }
        }

        private void RemoveGameEntityCacheEntry(GameEntityCacheEntry Entry)
        {
           // GameEntityCache.Remove(Entry);
        }
        public void RemoveGameEntityCacheEntry(int EntityHash)
        {
            //for(int i = 0; i < GameEntityCache.Count; ++i)
            //{
            //    if(GameEntityCache[i].EntityHash == EntityHash)
            //    {
            //        GameEntityCache.Remove(i);

            //        break;
            //    }
            //}
        }
        /// <summary>
        /// Destroys game entity, removed it from the world and de-caches it in the GameEntityCache
        /// </summary>
        /// <param name="EntityHash">Hash code of entity to destroy</param>
        private void DestroyGameEntity(int EntityHash)
        {
            //for(int i = 0; i < GameEntityCache.Count; ++i)
            //{
            //    if(GameEntityCache[i].EntityHash == EntityHash)
            //    {
            //        GameObject.Destroy(GameEntityCache[i].Entity);

            //        RemoveGameEntityCacheEntry(GameEntityCache[i].EntityHash);
            //    }
            //}
        }

        public List<GameObject> GetAllEntitesTagged(Tag EntityTag)
        {
            List<GameObject> GameObjects = new List<GameObject>();
            
            //for(int i = 0; i < GameEntityCache.Count; ++i)
            //{
            //    if(GameEntityCache[i].Tag.Is(EntityTag))
            //    {
            //        GameObjects.Add(GameEntityCache[i].Entity);
            //    }
            //}

            return GameObjects;
        }
    }
}