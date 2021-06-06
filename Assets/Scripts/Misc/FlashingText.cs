using UnityEngine;
using UnityEngine.UI;

namespace Frogger.Misc
{
    /// <summary>
    /// The Flashing Text Implementation.
    /// </summary>
    public class FlashingText : MonoBehaviour
    {
        #region fields

        /// <summary>
        /// The time in-between each flash.
        /// </summary>
        [SerializeField]
        private float flashingTime;

        // Gets the maximum flsahing time.
        private float _maxFlashingTime = 0f;

        // The current time of the flashing text.
        private float _currentTime = 0;

        // Gets the text of the gameObject.
        private Text _text;

        #endregion

        #region methods

        /// <summary>
        ///  Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            this._text = this.GetComponent<Text>();
            this._maxFlashingTime = this.flashingTime;
        }

        /// <summary>
        /// Called when the text is disabled.
        /// </summary>
        private void OnDisable()
        {
            if (this._text != null)
            {
                this._text.enabled = true;
            }
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            // Determines whether the text reference exists.
            if (this._text == null)
            {
                return;
            }

            if (this._currentTime >= this._maxFlashingTime)
            {
                this._text.enabled = !this._text.enabled;

                // Updates the flashing time dynamically.
                if (!this._text.enabled)
                {
                    this._maxFlashingTime = this.flashingTime * 0.5f;
                }
                else
                {
                    this._maxFlashingTime = this.flashingTime;
                }

                this._currentTime = 0f;
                return;
            }

            this._currentTime += Time.deltaTime;
        }

        #endregion
    }
}
