using UnityEngine;

namespace Frogger.Game.Misc
{

    /// <summary>
    /// The Spawnable object script.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class SpawnableObject : MonoBehaviour
    {
        #region fields

        /// <summary>
        /// The Spawner reference.
        /// </summary>
        protected Spawner _spawner;

        #endregion

        #region methods

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        protected virtual void Start()
        {
            this._spawner = this.GetComponentInParent<Spawner>();
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        protected virtual void Update()
        {
            if (this._spawner == null)
            {
                return;
            }

            this.transform.Translate(this._spawner.Direction * Time.deltaTime * this._spawner.Speed);
        }

        /// <summary>
        /// Called when the object leaves the collision.
        /// </summary>
        /// <param name="collision">The collision object.</param>
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == this._spawner.gameObject)
            {
                this._spawner.OnObjectLeftSpawn();
            }
            else if (collision.CompareTag("KillPlane"))
            {
                Destroy(this.gameObject);
            }
        }

        #endregion
    }

}
