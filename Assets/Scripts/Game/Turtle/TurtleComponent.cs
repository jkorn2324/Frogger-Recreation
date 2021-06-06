using UnityEngine;

namespace Frogger.Game.Turtle
{

    /// <summary>
    /// The Turtle Component.
    /// </summary>
    public class TurtleComponent : MonoBehaviour
    {

        #region properties

        /// <summary>
        /// Determines whether the turtle component is underwater.
        /// </summary>
        public bool Underwater
        {
            get
            {
                if (this._turtleInstance != null)
                {
                    return this._turtleInstance.StateController.State
                        == Turtle.State.TurtleState.STATE_BELOW_WATER;
                }
                return false;
            }
        }

        #endregion

        #region fields

        /// <summary>
        /// The turtle settings.
        /// </summary>
        [SerializeField]
        private TurtleSettings turtleSettings;

        /// <summary>
        /// The instance reference.
        /// </summary>
        private Turtle.ATurtleInstance _turtleInstance;

        #endregion

        #region methods

        /// <summary>
        /// Called when the turtle component has awakened.
        /// </summary>
        private void Awake()
        {
            this._turtleInstance = Turtle.ATurtleInstance.CreateInstance(this, turtleSettings);
        }

        /// <summary>
        /// Called when the turtle component is enabled.
        /// </summary>
        private void OnEnable()
        {
            this._turtleInstance?.HookEvents();
        }

        /// <summary>
        /// Called when the turtle component is disabled.
        /// </summary>
        private void OnDisable()
        {
            this._turtleInstance?.UnHookEvents();
        }

        /// <summary>
        /// Updates the turtle component.
        /// </summary>

        private void Update()
        {
            this._turtleInstance?.Update();
        }

        /// <summary>
        /// Called when the turtle has disappeared.
        /// </summary>
        private void OnTurtleDisappeared()
        {
            this._turtleInstance?.OnTurtleDisappeared();
        }

        #endregion
    }
}
