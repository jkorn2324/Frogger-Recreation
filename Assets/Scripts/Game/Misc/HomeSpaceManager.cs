using System.Collections.Generic;

namespace Frogger.Game.Misc
{

    /// <summary>
    /// The home space manager singleton.
    /// </summary>
    public class HomeSpaceManager
    {

        #region properties

        /// <summary>
        /// The number of filled spaces.
        /// </summary>
        public static int FilledSpaces
        {
            get
            {
                return _filledSpaces;
            }
        }

        #endregion

        #region fields

        // Determines whether the home space manager was initialized.
        private static bool _initialized = false;

        // The minimum value of filled spaces.
        private static int _filledSpaces = 0;

        /// <summary>
        /// The list of spaces in the scene.
        /// </summary>
        private static List<HomeSpace> _spaces = new List<HomeSpace>();

        #endregion

        #region methods

        /// <summary>
        /// Resets the home space manager data.
        /// </summary>
        public static void Reset()
        {
            _filledSpaces = 0;
            _spaces.Clear();
        }

        /// <summary>
        /// Initializes the Home space manager.
        /// </summary>
        public static void HookEvents()
        {
            HomeSpace.EnterSpaceEvent += OnHomeSpaceFilled;
        }

        /// <summary>
        /// Unhooks the events from the home space.
        /// </summary>
        public static void UnHookEvents()
        {
            HomeSpace.EnterSpaceEvent -= OnHomeSpaceFilled;
        }

        /// <summary>
        /// Adds the home space to the list of spaces.
        /// </summary>
        /// <param name="space">The space being added.</param>
        public static void AddHome(HomeSpace space)
        {
            if (!_spaces.Contains(space))
            {
                _spaces.Add(space);
            }
        }

        /// <summary>
        /// Called when the home space was filled by a frog.
        /// </summary>
        /// <param name="component">The frog component reference.</param>
        /// <param name="space">The home space reference.</param>
        private static void OnHomeSpaceFilled(Frog.FrogComponent component, HomeSpace space)
        {
            if (_spaces.Contains(space))
            {
                _filledSpaces++;

                if (_filledSpaces >= _spaces.Count)
                {
                    // Ends the game.
                    GameManager.Instance.EndGame(GameManager.GameResult.RESULT_WIN);
                }
            }
        }

        #endregion
    }
}

