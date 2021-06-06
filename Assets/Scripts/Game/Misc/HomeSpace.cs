using UnityEngine;

namespace Frogger.Game.Misc
{

    /// <summary>
    /// The Home space class implementation.
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(AudioSource))]
    public class HomeSpace : MonoBehaviour
    {
        #region events

        /// <summary>
        /// The enter space event definition.
        /// </summary>
        public static event System.Action<Frog.FrogComponent, HomeSpace> EnterSpaceEvent
            = delegate { };

        #endregion

        #region properties

        /// <summary>
        /// Determines whether or not the home space
        /// is empty.
        /// </summary>
        public bool Empty
        {
            get
            {
                return this.renderer == null ? true : !this.renderer.enabled;
            }
            private set
            {
                if (this.renderer == null)
                {
                    return;
                }

                this.renderer.enabled = !value;
            }
        }

        #endregion

        #region fields

        /// <summary>
        /// The audio source.
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// The renderer reference.
        /// </summary>
        private new SpriteRenderer renderer;

        #endregion

        #region methods

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            // Adds the home to the home space manager.
            HomeSpaceManager.AddHome(this);

            this.renderer = this.GetComponent<SpriteRenderer>();
            this.audioSource = this.GetComponent<AudioSource>();

            this.Empty = true;
        }


        /// <summary>
        /// Called when the home space enters a new trigger.
        /// </summary>
        /// <param name="col">The collider that this object has collided with.</param>
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("player.frog"))
            {
                Frog.FrogComponent playerComponent = col.GetComponent<Frog.FrogComponent>();
                if (playerComponent == null
                    || playerComponent.FrogState != Frog.State.FrogState.STATE_ALIVE)
                {
                    return;
                }

                if (!playerComponent.OnEnterSpace(this))
                {
                    return;
                }
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                EnterSpaceEvent(playerComponent, this);
                this.Empty = false;
            }
        }

        #endregion
    }

}
