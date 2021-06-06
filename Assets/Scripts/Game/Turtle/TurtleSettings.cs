using UnityEngine;

namespace Frogger.Game.Turtle
{

    /// <summary>
    /// Defines the scriptable object for the turtle settings.
    /// </summary>
    [CreateAssetMenu(fileName = "Turtle Settings", menuName = "Turtle Settings")]
    public class TurtleSettings : ScriptableObject
    {

        /// <summary>
        /// Determines the maximum seconds underwater.
        /// </summary>
        [SerializeField]
        private float maxSecondsUnderwater;

        /// <summary>
        /// Determines the turtle type.
        /// </summary>
        [SerializeField]
        private Turtle.TurtleType turtleType;

        /// <summary>
        /// Gets the maximum seconds underwater.
        /// </summary>
        public float MaxSecondsUnderwater
            => this.maxSecondsUnderwater;

        /// <summary>
        /// Gets the turtle type.
        /// </summary>
        public Turtle.TurtleType TurtleType
            => this.turtleType;

        /// <summary>
        /// Gets the animator controller from its type.
        /// </summary>
        /// <returns>The animator controller.</returns>
        public RuntimeAnimatorController GetAnimatorController()
        {
            switch (this.turtleType)
            {
                case Turtle.TurtleType.TURTLE_DISAPPEARING:
                    return Resources.Load<RuntimeAnimatorController>(
                        "Animations/Turtles/Turtle_Disappear");
                case Turtle.TurtleType.TURTLE_NORMAL:
                    return Resources.Load<RuntimeAnimatorController>(
                        "Animations/Turtles/Turtle_Normal");
            }

            return null;
        }
    }
}