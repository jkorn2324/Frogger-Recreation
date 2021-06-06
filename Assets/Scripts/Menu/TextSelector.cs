using Frogger.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Frogger.Menu
{
    /// <summary>
    /// The text selector class.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TextSelector : MonoBehaviour
    {

        #region fields

        /// <summary>
        /// The event that gets called when the button is selected.
        /// </summary>
        [SerializeField]
        private UnityEngine.Events.UnityEvent whenSelected;

        // The text reference of this selector.
        private Text _text;

        // Saves the old text color.
        private Color _oldTextColor;

        // The flashing text reference.
        private FlashingText _flashingText;

        #endregion

        #region methods

        /// <summary>
        /// Called when the script is awakened.
        /// </summary>
        private void Start()
        {
            this._text = this.GetComponent<Text>();
            this._flashingText = this.GetComponent<FlashingText>();

            if (this._flashingText != null)
            {
                this._flashingText.enabled = false;
            }

            this._oldTextColor = _text.color;
        }

        /// <summary>
        /// Called when the arrow has moved next to this selector.
        /// </summary>
        public void OnHighlighted(Color color)
        {
            this._text.color = color;

            if (this._flashingText != null)
            {
                this._flashingText.enabled = true;
            }
        }

        /// <summary>
        /// Called when the arrow has been deselected.
        /// </summary>
        public void OnUnhighlighted()
        {
            this._text.color = this._oldTextColor;

            if (this._flashingText != null)
            {
                this._flashingText.enabled = false;
            }
        }

        /// <summary>
        /// Called when the selector has been selected.
        /// </summary>
        public void OnSelected()
        {
            this.whenSelected?.Invoke();
        }

        #endregion
    }
}