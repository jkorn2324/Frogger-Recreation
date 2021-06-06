using UnityEngine;

namespace Frogger.Menu
{

    /// <summary>
    /// The input for the user.
    /// </summary>
    public class UserMenuInput
    {
        #region properties

        /// <summary>
        /// The input of the user.
        /// </summary>
        public float InputValue
        {
            get
            {
                return this._input;
            }
        }

        /// <summary>
        /// Determines whether it was selected.
        /// </summary>
        public bool Selected
        {
            get
            {
                return this._selected;
            }
        }

        #endregion

        #region fields

        /// <summary>
        /// The input of the user.
        /// </summary>
        private float _input = 0f, _prevInput = 0f;

        /// <summary>
        /// Determines whether the option was selected.
        /// </summary>
        private bool _selected = false;

        #endregion

        #region methods

        /// <summary>
        /// Updates the main menu inputs.
        /// </summary>
        public void Update()
        {
            this._prevInput = this._input;
            this._input = Input.GetAxisRaw("Vertical");

            this._selected = Input.GetKeyDown(KeyCode.Return);
        }

        /// <summary>
        /// Determines whether the user input is valid.
        /// </summary>
        /// <returns>True if it is, false otherwise.</returns>
        public bool IsValidInput()
        {
            return Mathf.Abs(this._input) > 0
                && this._prevInput != this._input;
        }

        #endregion
    }

    /// <summary>
    /// Stores the arrow value inputs.
    /// </summary>

    [System.Serializable]
    public struct ArrowInputs
    {

        #region fields

        [SerializeField]
        private Color color;

        [SerializeField]
        private Vector2 offset;

        [SerializeField]
        private GameObject arrowReference;

        #endregion

        #region properties

        public Vector2 Offset
            => this.offset;

        public Color Color
            => this.color;

        public GameObject ArrowReference
            => this.arrowReference;

        #endregion
    }


    /// <summary>
    /// Audio inputs used for the menu.
    /// </summary>
    [System.Serializable]
    public struct AudioInputs
    {
        #region fields

        [SerializeField]
        private bool enableAudio;

        [SerializeField]
        private AudioClip sound;

        #endregion

        #region properties

        public bool EnableAudio
            => this.enableAudio;

        public AudioClip Sound
            => this.sound;

        #endregion
    }
}