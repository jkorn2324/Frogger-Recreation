using UnityEngine;

namespace Frogger.Menu
{
    /// <summary>
    /// Handles the events for the script.
    /// </summary>
    public class MenuEvents : MonoBehaviour
    {

        #region events

        /// <summary>
        /// Called when the quit game text was selected.
        /// </summary>
        public void OnQuitSelected()
        {
            Utils.GameUtils.QuitGame();
        }

        /// <summary>
        /// Called when the start game text was selected.
        /// </summary>
        public void OnStartSelected()
        {
            Utils.GameUtils.LoadScene("Game");
        }

        /// <summary>
        /// Called when the credits were selected.
        /// </summary>
        public void OnCreditsSelected()
        {
            Utils.GameUtils.LoadScene("Credits");
        }

        /// <summary>
        /// Called when the new game text was selected.
        /// </summary>
        public void OnNewGameSelected()
        {
            Utils.GameUtils.LoadScene("Game");
        }

        /// <summary>
        /// Called when the main menu text were selected.
        /// </summary>
        public void OnMainMenuSelected()
        {
            Utils.GameUtils.LoadScene("MainMenu");
        }

        #endregion
    }
}


