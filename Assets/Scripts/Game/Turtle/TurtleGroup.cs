using System.Collections.Generic;

namespace Frogger.Game.Turtle
{

    /// <summary>
    /// The Turtle Group Monobehavior implementation.
    /// </summary>
    public class TurtleGroup : Misc.SpawnableObject
    {

        #region fields

        /// <summary>
        /// The list of turtle components.
        /// </summary>
        private List<TurtleComponent> _turtleComponents;

        #endregion

        #region methods

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            // Sets the turtle components.
            this._turtleComponents = new List<TurtleComponent>(
                this.GetComponentsInChildren<TurtleComponent>());

            this._spawner.OnObjectLeftSpawn();
        }

        /// <summary>
        /// Determines whether this is underwater.
        /// </summary>
        /// <returns>True if any tturtle is underwater, false otherwise.</returns>
        public bool IsUnderwater()
        {
            foreach (TurtleComponent component in this._turtleComponents)
            {
                if (component.Underwater)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
