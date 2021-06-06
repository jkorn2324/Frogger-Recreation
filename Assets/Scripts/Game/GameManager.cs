using UnityEngine;

namespace Frogger.Game
{
    /// <summary>
    /// The GameManager implementation class.
    /// </summary>
    public class GameManager : MonoBehaviour
    {

        #region enums

        /// <summary>
        /// The Game Result.
        /// </summary>
        [System.Serializable]
        public enum GameResult
        {
            RESULT_WIN,
            RESULT_LOSE
        }

        #endregion

        #region events

        /// <summary>
        /// Called when the game is over.
        /// </summary>
        public static event System.Action<GameResult> OnGameOver
            = delegate { };

        #endregion

        #region properties

        /// <summary>
        /// Gets the instance of the GameManager.
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// The result of the game.
        /// </summary>
        public static GameResult? Result
        {
            get
            {
                return _gameResult;
            }
        }

        #endregion

        #region fields

        /// <summary>
        /// Gets the game result.
        /// </summary>
        private static GameResult? _gameResult = null;

        /// <summary>
        /// Instance reference of the GameManager.
        /// </summary>
        private static GameManager _instance;

        /// <summary>
        /// Determines the frog manager settings.
        /// </summary>
        [SerializeField]
        private Frog.FrogManager.FrogManagerSettings frogManagerSettings;

        /// <summary>
        /// The active player frog.
        /// </summary>
        private Frog.FrogComponent _playerFrog;

        /// <summary>
        /// The active lady frog.
        /// </summary>
        private Frog.FrogComponent _ladyFrog;

        /// <summary>
        /// Determines whether the game has ended.
        /// </summary>
        private bool _ended = false;

        #endregion

        #region methods

        /// <summary>
        /// Called when the script is first activated.
        /// </summary>
        private void Awake()
        {
            _instance = this;

            // Resets the game result.
            _gameResult = null;

            // Resets the home space manager.
            Misc.HomeSpaceManager.Reset();
            // Initializes the frog manager settings.
            Frog.FrogManager.Init(this.frogManagerSettings);
        }

        /// <summary>
        /// Called when the game manager is enabled.
        /// </summary>
        private void OnEnable()
        {
            this._playerFrog = Frog.FrogManager.SpawnPlayerFrog();

            this.HookEvents();
        }

        /// <summary>
        /// Called when the game manager is disabled.
        /// </summary>
        private void OnDisable()
        {
            this.UnHookEvents();
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <param name="result">The win/lose result of the game.</param>
        public async void EndGame(GameResult result)
        {
            _gameResult = result;
            OnGameOver(result);

            // Sets the game as over.
            this._ended = true;
            // Delay 0.5 seconds.
            await System.Threading.Tasks.Task.Delay(500);

            // Loads the end game scene.
            Utils.GameUtils.LoadScene("EndGame");
        }

        /// <summary>
        /// Called to hook the events.
        /// </summary>
        private void HookEvents()
        {
            Misc.HomeSpaceManager.HookEvents();

            Frog.PlayerFrogInstance.GlobalDeathEvent += this.OnFrogDeath;
            Misc.HomeSpace.EnterSpaceEvent += this.OnFrogEnterSpace;
            Misc.Spawner.SpawnEvent += this.OnObjectSpawned;
        }

        /// <summary>
        /// Called to unhook the events.
        /// </summary>
        private void UnHookEvents()
        {
            Misc.HomeSpaceManager.UnHookEvents();

            Frog.PlayerFrogInstance.GlobalDeathEvent -= this.OnFrogDeath;
            Misc.HomeSpace.EnterSpaceEvent -= this.OnFrogEnterSpace;
            Misc.Spawner.SpawnEvent -= this.OnObjectSpawned;
        }

        /// <summary>
        /// Called when a player frog has died.
        /// </summary>
        /// <param name="instance">The player frog instance.</param>
        private void OnFrogDeath(Frog.PlayerFrogInstance instance)
        {
            if (!instance.FrogType.Equals(Frog.FrogType.FROG_PLAYER))
            {
                return;
            }

            if (this._playerFrog != null && this._playerFrog.gameObject != null)
            {
                // Destroys the player frog.
                Destroy(this._playerFrog.gameObject);
            }

            this._playerFrog = null;

            if (Frog.FrogManager.OnPlayerDeath())
            {
                this.EndGame(GameResult.RESULT_LOSE);
                return;
            }

            this._playerFrog = Frog.FrogManager.SpawnPlayerFrog();
        }

        /// <summary>
        /// Called when the frog has entered a new space.
        /// </summary>
        /// <param name="player">The frog player.</param>
        /// <param name="space">The home space.</param>
        private async void OnFrogEnterSpace(Frog.FrogComponent player, Misc.HomeSpace space)
        {
            Destroy(player.gameObject);
            // Delays the spawned frog by 0.5 seconds.
            await System.Threading.Tasks.Task.Delay(500);
            this._playerFrog = Frog.FrogManager.SpawnPlayerFrog();
        }

        /// <summary>
        /// Called when an object has been spawned from a Spawner.
        /// </summary>
        /// <param name="spawnedObject">The game object spawned.</param>
        private void OnObjectSpawned(GameObject spawnedObject)
        {
            // If the spawned object is a log.
            if (spawnedObject.CompareTag("Log") && !this._ended)
            {
                // Determines whether or not to spawn a lady frog.
                int randomNumber = Random.Range(0, 10);
                Frog.State.PlayerFrogStateController stateController =
                    (Frog.State.PlayerFrogStateController)this._playerFrog?.Instance.StateController;

                // Determines when a lady frog will be spawned.
                if (randomNumber < 5 && (this._ladyFrog == null || this._ladyFrog.gameObject == null)
                    && !stateController.IsLadyFrog)
                {
                    // Spawn the lady frog & set its parent to the log.
                    Frog.FrogComponent ladyFrog = Frog.FrogManager.SpawnLadyFrog(spawnedObject.transform.position);
                    if (ladyFrog != null)
                    {
                        this._ladyFrog = ladyFrog;
                        ladyFrog.transform.SetParent(spawnedObject.transform);
                    }
                }
            }
        }

        #endregion
    }
}
