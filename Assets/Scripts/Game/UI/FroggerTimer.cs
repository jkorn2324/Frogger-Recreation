using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Frogger.Game.UI
{
    /// <summary>
    /// The Frogger Timer class definition.
    /// </summary>
    public class FroggerTimer : MonoBehaviour
    {

        #region events

        /// <summary>
        /// The time run out event.
        /// </summary>
        public static event Action TimeRunOutEvent
            = delegate { };

        #endregion

        #region fields

        // The time limit of the player.
        [Range(0, 50), SerializeField]
        private float timeLimit;

        // The reference to the time text.
        [SerializeField] Text timeText;

        // The current time of the timer.
        private float _currentTime = 0f;

        #endregion

        #region properties

        // The time remaining.
        private float TimeRemaining
        {
            get
            {
                return this.timeLimit - _currentTime;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private async void Update()
        {
            this._currentTime += Time.deltaTime;
            Mathf.Clamp(_currentTime, 0, timeLimit);

            if (this.timeText != null)
            {
                this.timeText.text = "Time: " + ((int)TimeRemaining).ToString();
            }

            if (this.TimeRemaining <= 0)
            {
                TimeRunOutEvent();
                // More accurately does this.
                await System.Threading.Tasks.Task.Delay(500);
                this.ResetTime();
            }
        }

        /// <summary>
        /// Resets the time of the timer.
        /// </summary>
        private void ResetTime()
        {
            this._currentTime = 0f;
        }

        /// <summary>
        /// Called when the timer is enabled.
        /// </summary>
        private void OnEnable()
        {
            Misc.HomeSpace.EnterSpaceEvent += OnEnterSpace;
            Frog.PlayerFrogInstance.GlobalDeathEvent += this.OnFrogDeath;
        }

        /// <summary>
        /// Called when the timer is disabled.
        /// </summary>
        private void OnDisable()
        {
            Misc.HomeSpace.EnterSpaceEvent -= OnEnterSpace;
            Frog.PlayerFrogInstance.GlobalDeathEvent -= this.OnFrogDeath;
        }

        /// <summary>
        /// Called when the player has entered a space.
        /// </summary>
        /// <param name="component">The FrogComponent reference.</param>
        /// <param name="homeSpace">The home space reference.</param>
        private void OnEnterSpace(Frog.FrogComponent component, Game.Misc.HomeSpace homeSpace)
        {
            this.ResetTime();
        }

        /// <summary>
        /// Called when the frog dies.
        /// </summary>
        /// <param name="instance">The instance of the frog that died.</param>
        private void OnFrogDeath(Frog.PlayerFrogInstance instance)
        {
            this.ResetTime();
        }

        #endregion
    }
}

