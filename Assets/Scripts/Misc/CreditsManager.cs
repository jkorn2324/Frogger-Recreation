using UnityEngine;

namespace Frogger.Misc
{
    /// <summary>
    /// The Manager for the credits.
    /// </summary>
    public class CreditsManager : MonoBehaviour
    {

        #region methods

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Utils.GameUtils.LoadScene("MainMenu");
            }
        }

        #endregion
    }
}

