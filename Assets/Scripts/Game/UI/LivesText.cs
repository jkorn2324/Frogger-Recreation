using UnityEngine;
using UnityEngine.UI;

namespace Frogger.Game.UI
{

    /// <summary>
    /// The Lives Text Script Implementation.
    /// </summary>
    public class LivesText : MonoBehaviour
    {

        #region fields

        /// <summary>
        /// The lives text.
        /// </summary>
        private Text _livesText;

        #endregion

        #region methods

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        private void Start()
        {
            this._livesText = GetComponent<Text>();
            this._livesText.text = Game.Frog.FrogManager.Lives.ToString();
        }

        /// <summary>
        /// Called when the lives text is enabled.
        /// </summary>
        private void OnEnable()
        {
            Game.Frog.FrogManager.NumLivesChanged += this.OnLivesChanged;
        }

        /// <summary>
        /// Called when the lives text is disabled.
        /// </summary>
        private void OnDisable()
        {
            Game.Frog.FrogManager.NumLivesChanged -= this.OnLivesChanged;
        }

        /// <summary>
        /// Called when the lives have updated.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        private void OnLivesChanged(Game.Frog.FrogManager.ModLivesEvent eventData)
        {
            this._livesText.text = eventData.currentNumLives.ToString();
        }

        #endregion
    }
}
