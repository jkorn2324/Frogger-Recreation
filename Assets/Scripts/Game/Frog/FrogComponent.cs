using System.Collections.Generic;
using UnityEngine;

namespace Frogger.Game.Frog
{

    /// <summary>
    /// Frog full implementation cleaned up.
    /// This applies the programming principles,
    /// only used for testing purposes.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class FrogComponent : MonoBehaviour
    {
        #region properties

        /// <summary>
        /// Gets the current Frog State.
        /// </summary>
        public Frog.State.FrogState FrogState
            => this._frogInstance.StateController.FrogState;

        /// <summary>
        /// Gets the instance of the frog.
        /// </summary>
        public Frog.AFrogInstance Instance
            => this._frogInstance;

        #endregion

        #region fields

        /// <summary>
        /// The current frog settings.
        /// </summary>
        [SerializeField]
        private FrogSettings frogSettings;

        /// <summary>
        /// References the player mask used.
        /// </summary>
        [SerializeField]
        private LayerMask playerMask;

        /// <summary>
        /// The frog instance.
        /// </summary>
        private Frog.AFrogInstance _frogInstance;

        /// <summary>
        /// The current collider of the frog.
        /// </summary>
        private Collider2D _collider;

        #endregion

        #region methods

        /// <summary>
        /// Called when the script is first awakened.
        /// </summary>
        private void Awake()
        {
            // Creates a new instance based on the provided settings.
            this._frogInstance = Frog.AFrogInstance.CreateInstance(this, this.frogSettings);
        }

        /// <summary>
        /// Called to start the frog.
        /// </summary>
        private void Start()
        {
            this._collider = this.GetComponent<Collider2D>();
        }

        /// <summary>
        /// Updates the frog.
        /// </summary>
        public void Update()
        {
            this._frogInstance?.Update();
        }

        /// <summary>
        /// Called when the frog enters a 2d Collision with a trigger.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            this._frogInstance?.OnTriggerEnter(collision);
        }

        /// <summary>
        /// Called when the frog enters & stays in a 2d collision with a trigger.
        /// </summary>
        /// <param name="collision">The collision.</param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (this._frogInstance is Frog.PlayerFrogInstance)
            {
                ((Frog.PlayerFrogInstance)this._frogInstance).OnTriggerStay(collision);
            }
        }

        /// <summary>
        /// Called when the frog exits a 2d Collision with a trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            this._frogInstance?.OnTriggerExit(collision);
        }

        /// <summary>
        /// Called when the frog has entered a space.
        /// </summary>
        /// <param name="space">The space the frog has entered.</param>
        /// <returns>True if frog successfully entered space, false otherwise.</returns>
        public bool OnEnterSpace(Misc.HomeSpace space)
        {
            if (this._frogInstance != null)
            {
                return this._frogInstance.OnEnterHomeSpace(space);
            }

            return false;
        }

        /// <summary>
        /// Called when the frog finished dying.
        /// </summary>
        private void OnDeathAnimEnded()
        {
            Invoke("CompleteDeathSequence", 0.7f);
        }

        /// <summary>
        /// Called when the death sequence has been
        /// completed.
        /// </summary>
        private void CompleteDeathSequence()
        {
            this._frogInstance?.OnDeathSequenceCompleted();
        }

        /// <summary>
        /// Called when the component is enabled.
        /// </summary>
        private void OnEnable()
        {
            this._frogInstance?.HookEvents();
        }

        /// <summary>
        /// Called when the component is disabled.
        /// </summary>
        private void OnDisable()
        {
            this._frogInstance?.UnHookEvents();
        }

        /// <summary>
        /// Determines whether or not the frog is standing on an object
        /// with the given tag.
        /// </summary>
        /// <param name="tag">The string tag given.</param>
        /// <returns>True if the object is standing on an object tag.</returns>
        public bool IsStandingOnObjectOfTag(string tag)
        {
            GameObject standing = this.StandingOnObjectOfTag(tag);
            return standing != null;
        }

        /// <summary>
        /// Gets the object the frog is currently standing on.
        /// </summary>
        /// <param name="tag">The tag of the game object.</param>
        /// <returns>A GameObject that the object is standing on of a given tag, null otherwise.</returns>
        public GameObject StandingOnObjectOfTag(string tag)
        {
            // Filters out the overlapped colliders.
            ContactFilter2D filter = new ContactFilter2D();
            filter.layerMask = this.playerMask;

            List<Collider2D> overlappedColliders = new List<Collider2D>();
            this._collider.OverlapCollider(filter, overlappedColliders);

            if (overlappedColliders.Count <= 0)
            {
                return null;
            }

            // Loops through each overlapped collider to determine 
            // if the object is standing on the current object.
            foreach (Collider2D c in overlappedColliders)
            {
                if (c.CompareTag(tag))
                {
                    return c.gameObject;
                }
            }

            return null;
        }

        #endregion
    }
}
