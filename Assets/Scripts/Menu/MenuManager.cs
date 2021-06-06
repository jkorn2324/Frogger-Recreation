using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Frogger.Menu
{
    /// <summary>
    /// The main menu class.
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        #region fields

        /// <summary>
        /// The arrow settings.
        /// </summary>
        [SerializeField]
        private ArrowInputs arrowSettings;

        /// <summary>
        /// The audio settings.
        /// </summary>
        [SerializeField]
        private AudioInputs audioSettings;

        // The index of the first button selected.
        [SerializeField]
        private int firstButtonIndex;

        /// <summary>
        /// The arrow game object.
        /// </summary>
        private GameObject _arrow;

        /// <summary>
        /// Determines the selected button.
        /// </summary>
        private int _selectedButton = 0;

        /// <summary>
        /// References to the buttons.
        /// </summary>
        private List<TextSelector> buttons;

        /// <summary>
        /// The input for the user menu.
        /// </summary>
        private UserMenuInput _input;

        /// <summary>
        /// The audio source.
        /// </summary>
        private AudioSource _audioSource = null;

        #endregion

        #region methods

        /// <summary>
        /// Called when the script has been started.
        /// </summary>
        private void Start()
        {
            this._input = new UserMenuInput();

            this._selectedButton = this.firstButtonIndex;

            // Adds an audio source to the current object if enabled.
            if (this.audioSettings.EnableAudio && this.audioSettings.Sound != null)
            {
                this._audioSource = this.gameObject.AddComponent<AudioSource>();
                this._audioSource.clip = this.audioSettings.Sound;
                this._audioSource.playOnAwake = false;
            }

            // Initializes the butons.
            this.buttons = new List<TextSelector>(
                this.GetComponentsInChildren<TextSelector>());

            // Initializes the arrow.
            this._arrow = this.arrowSettings.ArrowReference;
            Text arrowText = this._arrow.GetComponent<Text>();
            if (arrowText != null)
            {
                arrowText.color = this.arrowSettings.Color;
            }

            // Doesn't do anything except change the selection to original.
            this.ChangeSelection(this._selectedButton);
        }

        /// <summary>
        /// Changes the selection.
        /// </summary>
        /// <param name="selection">The button we are selecting.</param>
        private void ChangeSelection(int selection)
        {
            if (this.buttons.Count <= 0)
            {
                return;
            }

            if (selection < 0)
            {
                selection = this.buttons.Count - 1;
            }

            selection %= this.buttons.Count;
            int previousSelected = this._selectedButton;
            this._selectedButton = selection;

            // Plays the audio is the selected aren't equivalent.
            if (previousSelected != this._selectedButton)
            {
                // Plays the audio.
                this._audioSource?.Play();
            }


            TextSelector previousSelector = this.buttons[previousSelected];
            previousSelector.OnUnhighlighted();

            TextSelector currentSelected = this.buttons[this._selectedButton];
            RectTransform transform = (RectTransform)currentSelected.transform;

            Vector2 offset = this.arrowSettings.Offset;
            offset.x -= transform.rect.width / 3;

            Vector2 newPosition = (Vector2)currentSelected.transform.localPosition + offset;
            this._arrow.transform.localPosition = newPosition;

            currentSelected.OnHighlighted(this.arrowSettings.Color);
        }

        /// <summary>
        /// Updates the main menu script.
        /// </summary>
        private void Update()
        {
            // Updates the input.
            this._input.Update();

            // Calls whether or not the input was selected, if it is, invoke the event.
            if (this._input.Selected)
            {
                TextSelector currentSelector = this.buttons[this._selectedButton];
                currentSelector.OnSelected();
            }
            else if (this._input.IsValidInput())
            {
                int nextSelection = this._selectedButton + (int)this._input.InputValue;
                this.ChangeSelection(nextSelection);
            }
        }

        #endregion
    }
}
