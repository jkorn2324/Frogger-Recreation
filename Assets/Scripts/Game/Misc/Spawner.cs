using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frogger.Game.Misc
{
    public class Spawner : MonoBehaviour
    {
        #region structs

        /// <summary>
        /// The log prefab structure definition.
        /// </summary>
        [System.Serializable]
        private struct SpawnablePrefab
        {

            #region fields

            /// <summary>
            /// The reference to the prefab.
            /// </summary>
            [SerializeField]
            private GameObject prefab;

            /// <summary>
            /// Gets the spawn offset of this particular log.
            /// </summary>
            [SerializeField]
            private Vector2 spawnOffset;

            #endregion

            #region properties

            /// <summary>
            /// Property reference to the Prefab.
            /// </summary>
            public GameObject Prefab
            {
                get
                {
                    return this.prefab;
                }
            }

            /// <summary>
            /// Property reference to the Offset.
            /// </summary>
            public Vector2 Offset
            {
                get
                {
                    return this.spawnOffset;
                }
            }

            /// <summary>
            /// Determines whether or not the given log has a prefab.
            /// </summary>
            public bool HasPrefab
            {
                get
                {
                    return this.prefab != null;
                }
            }

            #endregion

            #region methods

            /// <summary>
            /// The Log prefab struct constructor.
            /// </summary>
            /// <param name="prefab">The prefab we are referencing.</param>
            /// <param name="spawnOffset">The spawn offset.</param>
            public SpawnablePrefab(GameObject prefab, Vector2 spawnOffset)
            {
                this.prefab = prefab;
                this.spawnOffset = spawnOffset;
            }

            #endregion
        }


        #endregion

        #region properties

        /// <summary>
        /// Gets the Log Speed.
        /// </summary>
        public float Speed
        {
            get
            {
                // If reverse, the speed is negative, otherwise speed is positive.
                return this.speed;
            }
        }

        /// <summary>
        /// Gets the direction of the log.
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                return this.reverse ? Vector2.left : Vector2.right;
            }
        }

        #endregion

        #region events


        /// <summary>
        /// Definition for the Spawn Event.
        /// </summary>
        public static event System.Action<GameObject> SpawnEvent
            = delegate { };

        #endregion

        #region fields

        /// <summary>
        /// Gets the Minimum Spawn Delay & Maximum Spawn Delay.
        /// Default Values:
        /// -> minSpawnDelay = 2f
        /// -> maxSpawnDelay = 4f
        /// </summary>
        [Range(0.1f, 10f)]
        [SerializeField]
        private float minSpawnDelay, maxSpawnDelay;

        /// <summary>
        /// Gets the list of log prefabs.
        /// </summary>
        [SerializeField]
        private List<SpawnablePrefab> prefabs;

        /// <summary>
        /// The log speed. By default was 1f.
        /// </summary>
        [Range(0f, 20f)]
        [SerializeField]
        private float speed;

        /// <summary>
        /// Determines whether or not a log
        /// has contacted/left the spawn area.
        /// </summary>
        private bool _objectLeftSpawn = true;

        /// <summary>
        /// Determines whether or not to reverse
        /// the direction of the logs.
        /// </summary>
        [SerializeField]
        private bool reverse;

        #endregion

        #region methods

        /// <summary>
        /// The Start Function of the LogSpawner.
        /// </summary>
        private void Start()
        {
            // Starts the Log Spawn Coroutine.
            StartCoroutine(this.SpawnObjectCoroutine());
        }

        /// <summary>
        /// Coroutine to spawn logs, etc...
        /// </summary>
        /// <returns>A yield return or null.</returns>
        private IEnumerator SpawnObjectCoroutine()
        {
            while (true)
            {
                this.SpawnObject();
                yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            }
        }

        /// <summary>
        /// Spawns a new object into the Game.
        /// </summary>
        private void SpawnObject()
        {
            // Do nothing with the logs.
            if (this.prefabs.Count <= 0)
            {
                return;
            }

            // The log choice randomized.
            int logChoice = Random.Range(0, this.prefabs.Count);
            SpawnablePrefab prefab = this.prefabs[logChoice];

            // If the LogPrefabStruct has a prefab than we spawn it.
            if (this._objectLeftSpawn && this.SpawnAtPosition(prefab, this.transform.position))
            {
                this._objectLeftSpawn = false;
            }
        }

        /// <summary>
        /// Spawns the object at a position.
        /// </summary>
        /// <param name="prefab">The prefab for the spawnable.</param>
        /// <param name="position">Position where to spawn the object.</param>
        /// <returns>True if the spawn was successful, false otherwise.</returns>
        private bool SpawnAtPosition(SpawnablePrefab prefab, Vector2 position)
        {
            if (prefab.HasPrefab)
            {
                Vector3 offset = (Vector3)prefab.Offset * (this.reverse ? -1 : 1);
                GameObject spawned = Instantiate(
                    prefab.Prefab, position + (Vector2)offset,
                    Quaternion.identity, this.transform);
                SpawnEvent(spawned);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called when the object has left the spawner collision.
        /// </summary>
        public void OnObjectLeftSpawn()
        {
            this._objectLeftSpawn = true;
        }

        #endregion
    }
}
