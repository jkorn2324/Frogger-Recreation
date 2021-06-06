using UnityEngine;

namespace Frogger.Game.Frog
{

    /// <summary>
    /// Frog settings scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "Frog Settings", menuName = "Frog Settings")]
    public class FrogSettings : ScriptableObject
    {

        // The Frog Type for the settings.
        [SerializeField]
        private FrogType type;

        // Defines the jump data for the frog component.
        [SerializeField]
        private Frog.Movement.JumpData jumpData;

        // References the animator controller.
        [SerializeField]
        private RuntimeAnimatorController animatorController;

        // Defines the  audio data for the frog settings.
        [SerializeField]
        private Frog.Audio.AudioData audioData;

        // Gets the current frog type.
        public FrogType CurrentFrogType
            => this.type;

        /// <summary>
        /// Gets the current Jump Data.
        /// </summary>
        public Frog.Movement.JumpData JumpData
            => this.jumpData;

        /// <summary>
        /// Gets the audio data.
        /// </summary>
        public Frog.Audio.AudioData AudioData
            => this.audioData;

        // Gets the animator controller.
        public RuntimeAnimatorController AnimatorController
            => this.animatorController;
    }
}