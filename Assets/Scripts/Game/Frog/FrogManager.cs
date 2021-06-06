using System;
using UnityEngine;

namespace Frogger.Game.Frog
{

    /// <summary>
    /// The frog manager singleton.
    /// </summary>
    public class FrogManager
    {

        #region structs

        /// <summary>
        /// The Frog Manager Settings.
        /// </summary>
        [System.Serializable]
        public struct FrogManagerSettings
        {

            #region fields

            [SerializeField]
            private GameObject frogPlayerPrefab;

            /// <summary>
            /// Reference to the lady frog prefab.
            /// </summary>
            [SerializeField]
            private GameObject ladyFrogPrefab;

            [SerializeField]
            private Vector2 spawnPosition;

            [SerializeField]
            private int maxLives;

            #endregion

            #region properties

            public GameObject PlayerFrogPrefab
                => this.frogPlayerPrefab;

            public GameObject LadyFrogPrefab
                => this.ladyFrogPrefab;

            public Vector2 SpawnPos
                => this.spawnPosition;

            public int MaxLives
                => this.maxLives;

            #endregion
        }

        /// <summary>
        /// The Mod Lives event struct definition.
        /// </summary>
        public struct ModLivesEvent
        {
            public readonly int previousNumLives;

            public readonly int currentNumLives;

            public ModLivesEvent(int previousData, int currentData)
            {
                this.previousNumLives = previousData;
                this.currentNumLives = currentData;
            }
        }

        #endregion

        #region events

        /// <summary>
        /// Called when the number of lives changed.
        /// </summary>
        public static event Action<ModLivesEvent> NumLivesChanged
            = delegate { };

        #endregion

        #region fields

        /// <summary>
        /// The Frog Manager Settings
        /// </summary>
        private static Nullable<FrogManagerSettings> _settings;

        /// <summary>
        /// Gets the current frog lives.
        /// </summary>
        private static int _frogLives = 3;

        #endregion

        #region properties

        /// <summary>
        /// Gets the current lives.
        /// </summary>
        public static int Lives
        {
            get
            {
                return _frogLives;
            }
        }

        #endregion

        #region methods


        /// <summary>
        /// Initializes the frog manager.
        /// </summary>
        public static void Init(FrogManagerSettings settings)
        {
            if (_settings.HasValue)
            {
                _frogLives = settings.MaxLives;
                return;
            }

            _settings = settings;
            _frogLives = settings.MaxLives;
        }

        /// <summary>
        /// Spawns the frog.
        /// </summary>
        /// <returns>Null if not initialized, otherwise return a new frog component instance.</returns>
        public static FrogComponent SpawnPlayerFrog()
        {
            if (!_settings.HasValue)
            {
                return null;
            }

            GameObject prefab = GameObject.Instantiate(
                _settings.Value.PlayerFrogPrefab, _settings.Value.SpawnPos, Quaternion.identity);
            return prefab.GetComponent<FrogComponent>();
        }

        /// <summary>
        /// Spawns a lady frog.
        /// </summary>
        /// <param name="spawnPosition">The position the lady frog is spawned.</param>
        /// <returns>Null if its not initialized, otherwise returns a new frog component instance.</returns>
        public static FrogComponent SpawnLadyFrog(Vector2 spawnPosition)
        {
            if (!_settings.HasValue)
            {
                return null;
            }

            GameObject prefab = GameObject.Instantiate
                (_settings.Value.LadyFrogPrefab, spawnPosition, Quaternion.identity);
            return prefab.GetComponent<FrogComponent>();
        }

        /// <summary>
        /// Called when the frog player has died.
        /// </summary>
        /// <returns>True if the game is over, false otherwise.</returns>
        public static bool OnPlayerDeath()
        {
            if (_frogLives <= 0)
            {
                return true;
            }

            NumLivesChanged(new ModLivesEvent(_frogLives, --_frogLives));
            return _frogLives <= 0;
        }


        #endregion
    }
}

