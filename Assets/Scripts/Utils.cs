using UnityEngine;
using UnityEngine.SceneManagement;

namespace Frogger.Utils
{

    /// <summary>
    /// The main screen utils class.
    /// </summary>
    public class ScreenUtils
    {

        #region fields

        /// <summary>
        /// The Minimum & Maximum Screen X.
        /// </summary>
        private static float __minScreenX = 0f, __maxScreenX = 0f;
        /// <summary>
        /// The Minimum & Maximum Screen Y.
        /// </summary>
        private static float __minScreenY = 0f, __maxScreenY = 0f;


        #endregion

        #region properties

        /// <summary>
        /// The minimum screen x size.
        /// </summary>
        public static float MinScreenX
        {
            get
            {
                Initialize();
                return __minScreenX;
            }
        }

        /// <summary>
        /// The maximum screen x size.
        /// </summary>
        public static float MaxScreenX
        {
            get
            {
                Initialize();
                return __maxScreenX;
            }
        }


        /// <summary>
        /// The minimum screen y size.
        /// </summary>
        public static float MinScreenY
        {
            get
            {
                Initialize();
                return __minScreenY;
            }
        }

        /// <summary>
        /// The maximum screen y size.
        /// </summary>
        public static float MaxScreenY
        {
            get
            {
                Initialize();
                return __maxScreenY;
            }
        }

        #endregion

        #region functions

        /// <summary>
        /// Initializes the variables for the screen.
        /// </summary>
        public static void Initialize()
        {
            if (Camera.main == null)
            {
                return;
            }

            Vector2 maxScreen = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));
            Vector2 minScreen = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

            __maxScreenX = maxScreen.x;
            __maxScreenY = maxScreen.y;
            __minScreenX = minScreen.x;
            __minScreenY = minScreen.y;
        }

        /// <summary>
        /// Determines if the point is within the screen.
        /// </summary>
        /// <param name="point">The point that we are accessing.</param>
        /// <returns>true if within screen, false otherwise</returns>
        public static bool IsWithinScreen(Vector2 point)
        {
            // Initializes the screen variables.
            Initialize();

            return point.x >= MinScreenX && point.x <= MaxScreenX
                && point.y >= MinScreenY && point.y <= MaxScreenY;
        }

        #endregion
    }

    /// <summary>
    /// Contains the functions used for the game.
    /// </summary>
    public class GameUtils
    {
        /// <summary>
        /// Loads the scene from the name asynchronously
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        public static void LoadScene(string name)
        {
            SceneManager.LoadSceneAsync(name);
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public static void QuitGame()
        {
            Application.Quit(0);
        }
    }

    /// <summary>
    /// Updates the timer.
    /// </summary>
    public class BasicTimer
    {

        #region fields

        // The maximum time of the timer.
        private System.TimeSpan _maxTime;

        // The time span.
        private System.DateTime beginTime;

        #endregion

        #region properties

        /// <summary>
        /// Determines whether the timer has been completed.
        /// </summary>
        public bool Completed
        {
            get
            {
                System.TimeSpan timeSpan = System.DateTime.Now.Subtract(beginTime);
                return timeSpan.CompareTo(this._maxTime) > 0;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// The Timer constructor.
        /// </summary>
        /// <param name="maxTime"></param>
        public BasicTimer(System.TimeSpan maxTime)
        {
            this._maxTime = maxTime;
            this.beginTime = System.DateTime.Now;
        }

        #endregion

        #region methods

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset()
        {
            this.beginTime = System.DateTime.Now;
        }

        #endregion
    }
}

