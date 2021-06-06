using System;
using UnityEngine;

/// <summary>
/// Namespace used for each frog component.
/// </summary>
namespace Frogger.Game.Frog
{

    /// <summary>
    /// Called to handle the audio of the player.
    /// </summary>
    namespace Audio
    {

        /// <summary>
        /// The audio data for the player.
        /// </summary>
        [System.Serializable]
        public struct AudioData
        {
            #region properties

            /// <summary>
            /// Gets the jump audio clip.
            /// </summary>
            public AudioClip Jump
            {
                get
                {
                    return this.jump;
                }
            }

            /// <summary>
            /// Gets the death audio clip.
            /// </summary>
            public AudioClip DeathSquash
            {
                get
                {
                    return this.deathSquash;
                }
            }
            public AudioClip DeathDrown
                => this.deathDrown;

            public AudioClip DeathTimer
                => this.deathTimer;

            #endregion

            #region fields

            /// <summary>
            /// Gets the jump audio clip.
            /// </summary>
            [SerializeField]
            private AudioClip jump;

            /// <summary>
            /// Gets the death audio clip.
            /// </summary>
            [SerializeField]
            private AudioClip deathSquash;
            [SerializeField]
            private AudioClip deathDrown;
            [SerializeField]
            private AudioClip deathTimer;

            #endregion
        }

        /// <summary>
        /// The Audio controller definition.
        /// </summary>
        public class PlayerFrogAudioController
        {
            #region fields

            /// <summary>
            /// The audio data for the player frog controller.
            /// </summary>
            private AudioData _data;

            /// <summary>
            /// Reference to the PlayerFrog instance.
            /// </summary>
            private PlayerFrogInstance _instance;

            // Reference to the audio source.
            private AudioSource _audioSource;

            #endregion

            #region constructor

            /// <summary>
            /// Constructor for the audio controller.
            /// </summary>
            /// <param name="instance">The instance of the PlayerFrog.</param>
            /// <param name="data">The Audio Data.</param>
            public PlayerFrogAudioController(PlayerFrogInstance instance, AudioData data)
            {
                this._instance = instance;
                this._data = data;
                this._audioSource = instance.Component.GetComponent<AudioSource>();
            }

            #endregion

            #region methods

            /// <summary>
            /// Used to hook the events to the instance.
            /// </summary>
            public void HookEvents()
            {
                this._instance.JumpBeginEvent += this.OnJump;
                this._instance.KillEvent += this.OnKill;
            }

            /// <summary>
            /// Used to unhook the events to the instance.
            /// </summary>
            public void UnHookEvents()
            {
                this._instance.JumpBeginEvent -= this.OnJump;
                this._instance.KillEvent -= this.OnKill;
            }

            /// <summary>
            /// Called when the frog has jumped.
            /// </summary>
            /// <param name="position">The position the frog has jumped to.</param>
            public void OnJump(Vector2 position)
            {
                if (this._audioSource != null && this._data.Jump != null)
                {
                    if (this._audioSource.isPlaying)
                    {
                        this._audioSource.Stop();
                    }

                    this._audioSource.clip = this._data.Jump;
                    this._audioSource.Play();
                }
            }

            /// <summary>
            /// Called when the frog has been killed.
            /// </summary>
            private void OnKill(Frog.State.DeathReason reason)
            {
                if (this._audioSource != null)
                {

                    if (this._audioSource.isPlaying)
                    {
                        this._audioSource.Stop();
                    }

                    AudioClip clip = null;
                    switch (reason)
                    {
                        case Frog.State.DeathReason.REASON_DROWN:
                            clip = this._data.DeathDrown;
                            break;
                        case Frog.State.DeathReason.REASON_SQUASH:
                            clip = this._data.DeathSquash;
                            break;
                        case Frog.State.DeathReason.REASON_TIMEUP:
                            clip = this._data.DeathTimer;
                            break;
                    }

                    if (clip != null)
                    {
                        this._audioSource.clip = clip;
                        this._audioSource.Play();
                    }
                }
            }

            #endregion
        }
    }

    /// <summary>
    /// The Movement namespace.
    /// </summary>
    namespace Movement
    {

        /// <summary>
        /// The jump data for the player.
        /// </summary>
        [System.Serializable]
        public struct JumpData
        {
            [Range(1f, 20f)]
            [SerializeField]
            private float jumpSpeed;

            [Range(1f, 10f)]
            [SerializeField]
            private float jumpDistance;

            public float Speed
                => this.jumpSpeed;

            public float Distance
                => this.jumpDistance;
        }

        /// <summary>
        /// Definition for the reason why frog stopped
        /// jumping.
        /// </summary>
        public enum JumpStopReason
        {
            REASON_ENDED,
            REASON_FORCED
        }

        /// <summary>
        /// Abstract class for the Input Controller.
        /// </summary>
        public abstract class AFrogInputController
        {

            #region properties

            #endregion

            #region fields

            // Definition for the horizontal & vertical inputs.
            protected float _horizontalInput = 0f, _verticalInput = 0f;

            // The given frog instance.
            protected AFrogInstance _instance;

            // Gets the parent jump controller.
            protected AFrogJumpController _parent;

            #endregion

            #region constructor

            /// <summary>
            /// The Frog input controller.
            /// </summary>
            /// <param name="instance">The instance of the frog.</param>
            /// <param name="jumpController">The jump controller.</param>
            public AFrogInputController(AFrogInstance instance, AFrogJumpController jumpController)
            {
                this._parent = jumpController;
                this._instance = instance;
            }

            #endregion

            #region methods

            /// <summary>
            /// Hooks the events to the frog instance.
            /// </summary>
            public virtual void HookEvents() { }

            /// <summary>
            /// Unhooks the events from the frog instance.
            /// </summary>
            public virtual void UnHookEvents() { }

            /// <summary>
            /// Used to update the movement controller.
            /// </summary>
            abstract public void Update();

            /// <summary>
            /// Gets the input to a direction.
            /// </summary>
            /// <returns></returns>
            public Vector2 InputToDirection()
            {
                if (Mathf.Abs(this._horizontalInput) > 0)
                {
                    return Vector2.right * this._horizontalInput;
                }
                else if (Mathf.Abs(this._verticalInput) > 0)
                {
                    return Vector2.up * this._verticalInput;
                }

                return Vector2.zero;
            }

            /// <summary>
            /// Determines if the input is valid.
            /// </summary>
            /// <returns>True if the frog input is valid.</returns>
            abstract public bool IsValidInput();

            #endregion
        }

        /// <summary>
        /// Implementation for the player frog input controller.
        /// </summary>
        public class PlayerFrogInputController : AFrogInputController
        {

            #region fields

            private float _prevHorizontal = 0f, _prevVertical = 0f;


            #endregion

            #region constructor

            /// <summary>
            /// The Player Frog constructor.
            /// </summary>
            /// <param name="instance">The instance of the player frog.</param>
            /// <param name="controller">The jump controller of the player frog.</param>
            public PlayerFrogInputController(PlayerFrogInstance instance, PlayerFrogJumpController controller)
                : base(instance, controller) { }

            #endregion

            #region methods

            public override bool IsValidInput()
            {
                return this._prevHorizontal == 0f
                    && this._prevVertical == 0f
                    && (this._verticalInput != 0f
                    || this._horizontalInput != 0f);
            }

            public override void Update()
            {
                float currHorizontal = Input.GetAxisRaw("Horizontal");
                float currVertical = Input.GetAxisRaw("Vertical");

                this._prevHorizontal = this._horizontalInput;
                this._prevVertical = this._verticalInput;

                this._horizontalInput = currHorizontal;
                this._verticalInput = currVertical;
            }

            #endregion
        }

        /// <summary>
        /// Implementation for the lady frog input controller.
        /// </summary>
        public class LadyFrogAIInputController : AFrogInputController
        {

            #region constants

            // The time in-between jumps.
            private const float TIME_BETWEEN_JUMPS = 1.5f;

            #endregion

            #region fields

            // Determines whether the frog has jumped before.
            private bool _jumpedBefore = false;

            // The time after jumping has finished.
            private float _timeAfterJumping = 0f;

            #endregion

            #region constructor

            /// <summary>
            /// The Lady frog AI input controller constructor.
            /// </summary>
            /// <param name="instance">The instance of the Lady frog.</param>
            /// <param name="controller">The controller of the lady frog.</param>
            public LadyFrogAIInputController(LadyFrogInstance instance, LadyFrogJumpController controller)
                : base(instance, controller)
            {
                this._horizontalInput = 1f;
            }

            #endregion

            #region methods

            /// <summary>
            /// Hooks the Events to the frog instance.
            /// </summary>
            public override void HookEvents()
            {
                this._instance.JumpBeginEvent += this.OnJumpBegin;
            }

            /// <summary>
            /// Unhooks the event from the frog instance.
            /// </summary>
            public override void UnHookEvents()
            {
                this._instance.JumpBeginEvent -= this.OnJumpBegin;
            }

            /// <summary>
            /// Used to begin the jump.
            /// </summary>
            /// <param name="direction">The direction of the jump.</param>
            private void OnJumpBegin(Vector2 direction)
            {
                this._jumpedBefore = true;
            }

            /// <summary>
            /// Switches the direction of the frog.
            /// </summary>
            public void SwitchDirection()
            {
                this._horizontalInput = -this._horizontalInput;
                this._verticalInput = -this._verticalInput;
            }

            public override bool IsValidInput()
            {
                return !this._jumpedBefore || (this._timeAfterJumping >= TIME_BETWEEN_JUMPS
                    && Mathf.Abs(this._horizontalInput) > 0);
            }

            public override void Update()
            {
                if (this._parent.Jumping)
                {
                    if (this._timeAfterJumping > 0)
                    {
                        this._timeAfterJumping = 0;
                    }
                    return;
                }

                this._timeAfterJumping += Time.deltaTime;
            }

            #endregion
        }

        /// <summary>
        /// Abstract class for the jump controller.
        /// </summary>
        public abstract class AFrogJumpController
        {

            #region events

            /// <summary>
            /// Called when the jump has begun.
            /// </summary>
            public event Action<Vector2> JumpBeginEvent
                = delegate { };

            /// <summary>
            /// Called when the jump has ended.
            /// </summary>
            public event Action<JumpStopReason> JumpStopEvent
                = delegate { };

            #endregion

            #region properties

            /// <summary>
            /// Determines whether the frog is jumping.
            /// </summary>
            public bool Jumping
            {
                get
                {
                    return this._jumpDirection.HasValue && this._finalJumpPos.HasValue;
                }
            }

            /// <summary>
            /// Gets the jump data of the frog.
            /// </summary>
            public JumpData JumpData
            {
                get
                {
                    return this._jumpData;
                }
            }

            /// <summary>
            /// The Movement controller of this frog.
            /// </summary>
            protected abstract AFrogInputController MovementController
            {
                get;
            }

            #endregion

            #region fields

            /// <summary>
            /// The Frog Instance.
            /// </summary>
            protected readonly AFrogInstance _frogInstance;

            /// <summary>
            /// The jump data of the frog.
            /// </summary>
            protected readonly JumpData _jumpData;

            // Current Jump Direction & final position.
            private Vector2? _jumpDirection = null, _finalJumpPos = null;
            // Lowest distance.
            private float lowestDistance = float.MaxValue;

            // Reference to the frog transform.
            protected Transform _frogTransform;

            #endregion

            #region constructor

            /// <summary>
            /// The Frog Jump controller constructor.
            /// </summary>
            /// <param name="frog">The frog instance.</param>
            /// <param name="data">The jump data.</param>
            public AFrogJumpController(AFrogInstance frog, JumpData data)
            {
                this._frogInstance = frog;
                this._jumpData = data;
                this._frogTransform = frog.Component.transform;
            }

            #endregion

            #region methods

            /// <summary>
            /// Used to hook the events from here.
            /// </summary>
            abstract public void HookEvents();

            /// <summary>
            /// Used to unhook the events.
            /// </summary>
            abstract public void UnHookEvents();

            /// <summary>
            /// Updates the frog jumping.
            /// </summary>
            public virtual void Update()
            {
                // Updates the movement input.
                this.MovementController.Update();

                // Updates the jump position of the frog.
                if (this.Jumping)
                {
                    Vector2 nextTranslation = this._jumpDirection.Value * this._jumpData.Speed * Time.deltaTime;
                    // Gets the current position of the frog.
                    Vector2 nextPosition = (Vector2)this._frogTransform.position + nextTranslation;
                    // Gets the absolute distance of the frog.
                    float distance = Vector2.Distance(nextPosition, this._finalJumpPos.Value);

                    // Updates the lowest distance.
                    if (distance < this.lowestDistance)
                    {
                        this.lowestDistance = distance;
                    }

                    // If the distance is greater than lowest distance, than
                    // we have found the lowest distance.
                    if (distance > this.lowestDistance)
                    {
                        this.OnJumpEnd();
                        return;
                    }

                    // Moves the frog to a certain position.
                    this._frogTransform.Translate(nextTranslation);
                    return;
                }

                if (this.CanJump())
                {
                    Vector2 direction = this.MovementController.InputToDirection();
                    this.JumpBeginEvent(direction);
                    this.OnJumpBegin(direction);
                    return;
                }
            }

            /// <summary>
            /// Determines whether or not the frog can jump.
            /// </summary>
            /// <returns></returns>
            abstract protected bool CanJump();

            /// <summary>
            /// Called when the jump has begun.
            /// </summary>
            /// <param name="direction">The direction of the jump.</param>
            protected virtual void OnJumpBegin(Vector2 direction)
            {
                this._jumpDirection = direction;
                this._finalJumpPos = (Vector2)this._frogTransform.position + (direction * this._jumpData.Distance);
            }

            /// <summary>
            /// Called when the jump has ended.
            /// </summary>
            protected virtual void OnJumpEnd()
            {
                this._frogTransform.position = this._finalJumpPos.Value;
                this.JumpStopEvent(JumpStopReason.REASON_ENDED);
                this.ForceStopJump();
            }

            /// <summary>
            /// Called to force the frog to stop jumping.
            /// </summary>
            protected virtual void ForceStopJump()
            {
                this._jumpDirection = null;
                this._finalJumpPos = null;
                this.lowestDistance = float.MaxValue;
            }

            #endregion
        }

        /// <summary>
        /// The Player Frog Jump controller.
        /// </summary>
        public class PlayerFrogJumpController : AFrogJumpController
        {

            /// <summary>
            /// The Movement controller property of the player frog.
            /// </summary>
            protected override AFrogInputController MovementController =>
                this._movementController;

            #region fields

            /// <summary>
            /// The local movement controller of the player frog.
            /// </summary>
            protected PlayerFrogInputController _movementController;

            #endregion

            #region constructor

            public PlayerFrogJumpController(PlayerFrogInstance frog, JumpData jumpData)
                : base(frog, jumpData)
            {
                this._movementController = new PlayerFrogInputController(frog, this);
            }

            #endregion

            #region methods

            public override void HookEvents()
            {
                ((PlayerFrogInstance)this._frogInstance).KillEvent += OnKill;
                this._movementController.HookEvents();
            }

            public override void UnHookEvents()
            {
                ((PlayerFrogInstance)this._frogInstance).KillEvent -= OnKill;
                this._movementController.UnHookEvents();
            }

            protected override bool CanJump()
            {
                if (!this._movementController.IsValidInput() ||
                    ((State.PlayerFrogStateController)this._frogInstance.StateController).FoundHome)
                {
                    return false;
                }

                Vector2 nextPosition = (this._jumpData.Distance * this._movementController.InputToDirection())
                    + (Vector2)this._frogTransform.position;
                RaycastHit2D[] hitRaycast = Physics2D.RaycastAll(nextPosition, Vector2.zero);

                if (hitRaycast.Length <= 0)
                {
                    return true;
                }

                foreach (RaycastHit2D raycastHit in hitRaycast)
                {
                    if (raycastHit.collider.tag == "border.frog.bound")
                    {
                        return false;
                    }
                }

                return true;
            }


            /// <summary>
            /// Called when frog is killed
            /// </summary>
            /// <param name="reason">Death Reason</param>
            private void OnKill(Frog.State.DeathReason reason)
            {
                this.ForceStopJump();
            }


            #endregion
        }

        /// <summary>
        /// The Lady frog jump controller.
        /// </summary>
        public class LadyFrogJumpController : AFrogJumpController
        {

            #region properties

            /// <summary>
            /// The property reference to the movement input.
            /// </summary>
            protected override AFrogInputController MovementController
                => this._movementInput;

            #endregion

            #region fields

            // The reference to the movement input.
            protected LadyFrogAIInputController _movementInput;

            #endregion

            #region constructor

            /// <summary>
            /// The Lady frog controller constructor.
            /// </summary>
            /// <param name="instance">The instance of the lady frog.</param>
            /// <param name="data">The jump input data of the lady frog.</param>
            public LadyFrogJumpController(LadyFrogInstance instance, JumpData data)
                : base(instance, data)
            {
                this._movementInput = new LadyFrogAIInputController(instance, this);
            }

            #endregion

            #region methods

            public override void HookEvents()
            {
                this._movementInput.HookEvents();
            }

            public override void UnHookEvents()
            {
                this._movementInput.UnHookEvents();
            }

            /// <summary>
            /// Determines whether the frog was able to move in the direction
            /// based on the current input.
            /// </summary>
            /// <returns>True if the direction is valid, false otherwise.</returns>
            private bool ValidDirection()
            {
                State.LadyFrogStateController stateController = (State.LadyFrogStateController)this._frogInstance.StateController;
                Vector2 nextPosition = (this._movementInput.InputToDirection() * this._jumpData.Distance)
                    + (Vector2)this._frogTransform.position;
                RaycastHit2D[] hitRaycasts = Physics2D.RaycastAll(nextPosition, Vector2.zero);

                if (stateController.Parent == null)
                {
                    if (hitRaycasts.Length <= 0)
                    {
                        return false;
                    }
                    else if (hitRaycasts.Length == 1 && hitRaycasts[0].collider.tag != "water.area")
                    {
                        return true;
                    }

                    foreach (RaycastHit2D hit in hitRaycasts)
                    {
                        if (hit.transform?.tag == "Log")
                        {
                            return true;
                        }
                    }

                    return false;
                }
                else if (hitRaycasts.Length <= 0)
                {
                    return false;
                }

                // Searches to see if the frog contains the parent.
                foreach (RaycastHit2D hit in hitRaycasts)
                {
                    if (hit.transform.gameObject == stateController.Parent.gameObject)
                    {
                        return true;
                    }
                }

                return false;
            }

            protected override bool CanJump()
            {
                if (!this._movementInput.IsValidInput())
                {
                    return false;
                }

                if (this.ValidDirection())
                {
                    return true;
                }

                // Switches the direction of the input.
                this._movementInput.SwitchDirection();
                return this.ValidDirection();
            }

            #endregion
        }
    }

    /// <summary>
    /// The animations namespace.
    /// </summary>
    namespace Animation
    {

        /// <summary>
        /// The abstract class for the animation controller.
        /// </summary>
        public class FrogAnimationController
        {

            #region fields

            // Reference to the frog instance.
            protected readonly AFrogInstance _instance;

            // Reference to the animator.
            protected readonly Animator _animator;

            // Reference to the frog component.
            protected readonly FrogComponent _component;

            #endregion

            #region constructor

            /// <summary>
            /// The frog animation controller.
            /// </summary>
            /// <param name="instance">The instance for the frog.</param>
            /// <param name="controller">The animation controller for this frog.</param>
            public FrogAnimationController(AFrogInstance instance, RuntimeAnimatorController controller)
            {
                this._instance = instance;
                this._component = instance.Component;

                this._animator = this._component.gameObject.AddComponent<Animator>();
                this._animator.runtimeAnimatorController = controller;
                this._animator.updateMode = AnimatorUpdateMode.Normal;
                this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }

            #endregion

            #region methods

            /// <summary>
            /// Hooks the events so that they fire on this controller.
            /// </summary>
            public virtual void HookEvents()
            {
                this._instance.JumpBeginEvent += this.OnJumpBegin;
                this._instance.JumpStopEvent += this.OnJumpStop;
            }

            /// <summary>
            /// Unhooks the events from this script.
            /// </summary>
            public virtual void UnHookEvents()
            {
                this._instance.JumpBeginEvent -= this.OnJumpBegin;
                this._instance.JumpStopEvent -= this.OnJumpStop;
            }

            /// <summary>
            /// Called when the jump has started.
            /// </summary>
            /// <param name="direction">The direction of the frog.</param>
            private void OnJumpBegin(Vector2 direction)
            {
                this._animator?.SetBool("Jumping", true);
                this._animator?.SetInteger("DirectionX", (int)direction.x);
                this._animator?.SetInteger("DirectionY", (int)direction.y);
            }

            /// <summary>
            /// called when the jump has stopped.
            /// </summary>
            /// <param name="reason">The reason why the jump has stopped.</param>
            private void OnJumpStop(Movement.JumpStopReason reason)
            {
                this._animator?.SetBool("Jumping", false);
            }

            #endregion
        }

        /// <summary>
        /// Definition for the player frog animation controller.
        /// </summary>
        public class PlayerFrogAnimationController : FrogAnimationController
        {

            /// <summary>
            /// The constructor for the frog animation controller.
            /// </summary>
            /// <param name="instance">The instance of the frog.</param>
            /// <param name="controller">The controller of the frog.</param>
            public PlayerFrogAnimationController(PlayerFrogInstance instance, RuntimeAnimatorController controller)
                : base(instance, controller) { }


            /// <summary>
            /// Sets the animation controller.
            /// </summary>
            /// <param name="controller">The animation controller that's switched.</param>
            private void SetAnimationController(RuntimeAnimatorController controller)
            {
                if (controller == null)
                {
                    return;
                }

                this._animator.runtimeAnimatorController = controller;
            }

            /// <summary>
            /// Called to override the events from the previous animation controller. 
            /// </summary>
            public override void HookEvents()
            {
                base.HookEvents();

                ((PlayerFrogInstance)this._instance).KillEvent += this.OnKill;
                ((PlayerFrogInstance)this._instance).LadyFrogCollideEvent += this.OnCollideWithLadyFrog;
            }

            /// <summary>
            /// Called to override the events from animation controller.
            /// </summary>
            public override void UnHookEvents()
            {
                base.UnHookEvents();

                ((PlayerFrogInstance)this._instance).KillEvent -= this.OnKill;
                ((PlayerFrogInstance)this._instance).LadyFrogCollideEvent -= this.OnCollideWithLadyFrog;
            }

            /// <summary>
            /// Called when the player frog has collided with another lady frog.
            /// </summary>
            /// <param name="instance">The lady frog instance.</param>
            private void OnCollideWithLadyFrog(LadyFrogInstance instance)
            {
                Animator lfAnimator = instance.Component?.GetComponent<Animator>();
                this.SetAnimationController(lfAnimator?.runtimeAnimatorController);
            }

            /// <summary>
            /// Called when the frog was killed.
            /// </summary>
            private void OnKill(State.DeathReason reason)
            {
                this._animator?.SetBool("Jumping", false);
                this._animator?.SetTrigger("DeathAnimation");
            }
        }
    }

    /// <summary>
    /// The frog state namespace.
    /// </summary>
    namespace State
    {
        /// <summary>
        /// Names the different death arguments for the player.
        /// </summary>
        public enum DeathReason
        {
            REASON_SQUASH,
            REASON_DROWN,
            REASON_TIMEUP
        }

        /// <summary>
        /// Handles the frog state.
        /// </summary>
        public enum FrogState
        {
            STATE_ALIVE,
            STATE_DYING,
            STATE_DEAD
        }

        /// <summary>
        /// Abstract class definition for handling the state of a frog.
        /// </summary>
        public abstract class AFrogStateController
        {

            #region properties


            /// <summary>
            /// Gets the current frog state.
            /// </summary>
            public FrogState FrogState
            {
                get
                {
                    return this._frogState;
                }
            }

            /// <summary>
            /// Gets the frog instance.
            /// </summary>
            public AFrogInstance FrogInstance
            {
                get
                {
                    return this._frogInstance;
                }
            }

            #endregion

            #region fields


            /// <summary>
            /// The current frog state.
            /// </summary>
            protected FrogState _frogState;

            /// <summary>
            /// The current frog instance.
            /// </summary>
            protected readonly AFrogInstance _frogInstance;

            #endregion

            #region constructor

            public AFrogStateController(AFrogInstance _instance)
            {
                this._frogInstance = _instance;
                this._frogState = FrogState.STATE_ALIVE;
            }

            #endregion

            #region methods

            /// <summary>
            /// Updates the frog state controller.
            /// </summary>
            public virtual void Update() { }

            /// <summary>
            /// Used to hook any events this class may need.
            /// </summary>
            abstract public void HookEvents();

            /// <summary>
            /// Used to remove connections to the events.
            /// </summary>
            abstract public void UnHookEvents();

            #endregion
        }

        /// <summary>
        /// Player Frog State Controller class.
        /// </summary>
        public class PlayerFrogStateController : AFrogStateController
        {

            #region events

            /// <summary>
            /// The collide with lady frog event.
            /// </summary>
            public event Action<LadyFrogInstance> LadyFrogCollideEvent
                = delegate { };

            /// <summary>
            /// The Kill Event.
            /// </summary>
            public event Action<DeathReason> KillEvent
                = delegate { };

            /// <summary>
            /// The Death Event.
            /// </summary>
            public event Action DeathEvent
                = delegate { };

            #endregion

            #region properties

            /// <summary>
            /// Determines whether or not the frog has found
            /// a home.
            /// </summary>
            public bool FoundHome
            {
                get
                {
                    return this._foundHome;
                }
            }

            /// <summary>
            /// Determines whether or not the frog is a lady frog.
            /// </summary>
            public bool IsLadyFrog
            {
                get
                {
                    return this._ladyFrog;
                }
            }

            #endregion

            #region fields

            /// <summary>
            /// Determines whether frog is safe or not.
            /// </summary>
            private bool _safe = true;

            /// <summary>
            /// Determines whether the frog has found
            /// a home or not.
            /// </summary>
            private bool _foundHome = false;

            /// <summary>
            /// Determines whether the frog has collided with a lady frog.
            /// </summary>
            private bool _ladyFrog = false;

            // References the turtleGroup the frog is standing on.
            private Turtle.TurtleGroup _turtleGroup = null;

            #endregion

            #region constructor

            /// <summary>
            /// The Player Frog State Controller.
            /// </summary>
            /// <param name="instance">the Player Frog instance base.</param>
            public PlayerFrogStateController(PlayerFrogInstance instance)
                : base(instance) { }

            #endregion

            #region methods

            /// <summary>
            /// Used to hook the events to the frog.
            /// </summary>
            public override void HookEvents()
            {
                this._frogInstance.JumpBeginEvent += this.OnJumpBegin;
                this._frogInstance.JumpStopEvent += this.OnJumpEnd;
                this._frogInstance.TriggerEnterEvent += this.OnTriggerEnter;
                this._frogInstance.TriggerExitEvent += this.OnTriggerExit;
                Game.UI.FroggerTimer.TimeRunOutEvent += this.OnTimeOut;
                ((PlayerFrogInstance)this._frogInstance).TriggerStayEvent += this.OnTriggerStay;
            }

            /// <summary>
            /// Used to remove the connections from our events.
            /// </summary>
            public override void UnHookEvents()
            {
                this._frogInstance.JumpBeginEvent -= this.OnJumpBegin;
                this._frogInstance.JumpStopEvent -= this.OnJumpEnd;
                this._frogInstance.TriggerEnterEvent -= this.OnTriggerEnter;
                this._frogInstance.TriggerExitEvent -= this.OnTriggerExit;
                Game.UI.FroggerTimer.TimeRunOutEvent -= this.OnTimeOut;
                ((PlayerFrogInstance)this._frogInstance).TriggerStayEvent -= this.OnTriggerStay;
            }

            /// <summary>
            /// Updates the frog every frame.
            /// </summary>
            public override void Update()
            {
                // Checks whether the turtle group is underwater, if it is, kill the frog.
                if (this.FrogState == FrogState.STATE_ALIVE
                    && this._turtleGroup != null && this._turtleGroup.IsUnderwater())
                {
                    this.Kill(State.DeathReason.REASON_DROWN);
                }
            }

            /// <summary>
            /// Called when the frog has began to jump.
            /// </summary>
            /// <param name="direction">The jump direction.</param>
            private void OnJumpBegin(Vector2 direction)
            {
                FrogComponent component = this._frogInstance.Component;

                if (component == null || component.gameObject == null)
                {
                    return;
                }

                // Unsets the turtle group.
                if (this._turtleGroup != null)
                {
                    this._turtleGroup = null;
                }

                component.transform.parent = null;
            }

            /// <summary>
            /// Called when the frog has ended the jump.
            /// </summary>
            private void OnJumpEnd(Movement.JumpStopReason reason)
            {
                FrogComponent frog = this._frogInstance.Component;

                if (frog == null || frog.gameObject == null)
                {
                    return;
                }

                // Sets the found object.
                GameObject foundObject = null;

                if ((foundObject = frog.StandingOnObjectOfTag("Log")) != null)
                {
                    frog.transform.SetParent(foundObject.transform);
                }
                else if ((foundObject = frog.StandingOnObjectOfTag("object.turtle")) != null)
                {
                    // Sets the turtle group of the frog.
                    this._turtleGroup = foundObject.GetComponent<Turtle.TurtleGroup>();
                    frog.transform.SetParent(foundObject.transform);
                }
                else if (frog.IsStandingOnObjectOfTag("water.area"))
                {
                    // Kills the frog if standing on water.
                    this.Kill(State.DeathReason.REASON_DROWN);
                }
            }

            /// <summary>
            /// Called when the trigger has entered.
            /// </summary>
            /// <param name="collider">The other collider.</param>
            private void OnTriggerEnter(Collider2D collider)
            {
                switch (collider.tag)
                {
                    case "home.wall":
                    case "obstacle.car":
                    case "border.frog.bound":
                        this.Kill(State.DeathReason.REASON_SQUASH);
                        break;
                    case "safe.area":
                        this._safe = true;
                        break;
                    case "lady.frog":
                        this.OnLadyFrogCollide(collider.gameObject);
                        break;
                }
            }
            /// <summary>
            /// Called when the player runs out of time.
            /// </summary>
            private void OnTimeOut()
            {
                this._safe = false;
                this.Kill(State.DeathReason.REASON_TIMEUP);
            }

            /// <summary>
            /// Called when the trigger has stayed.
            /// </summary>
            /// <param name="collider">The other collider.</param>
            public void OnTriggerStay(Collider2D collider)
            {
                if (collider.tag == "obstacle.car")
                {
                    this.Kill(State.DeathReason.REASON_SQUASH);
                }
            }

            /// <summary>
            /// Called when the lady frog collides with the current frog.
            /// </summary>
            /// <param name="gameObject">The collider that collides with the object.</param>
            private void OnLadyFrogCollide(GameObject gameObject)
            {
                FrogComponent component = gameObject.GetComponent<FrogComponent>();
                AFrogInstance instance = component?.Instance;
                if (instance is LadyFrogInstance)
                {
                    this.LadyFrogCollideEvent((LadyFrogInstance)instance);
                }

                this._ladyFrog = true;
                // Destroys the lady frog.
                GameObject.Destroy(gameObject);
            }

            /// <summary>
            /// Called when the trigger has exited.
            /// </summary>
            /// <param name="collider">The other collider.</param>
            private void OnTriggerExit(Collider2D collider)
            {
                if (collider.tag == "safe.area")
                {
                    this._safe = false;
                }
            }

            /// <summary>
            /// Called when the frog has entered the home space.
            /// </summary>
            /// <param name="homeSpace">The home space entered.</param>
            /// <returns>True if the frog has successfully entered a home space, false otherwise.</returns>
            public bool OnEnterHomeSpace(Misc.HomeSpace homeSpace)
            {
                if (!homeSpace.Empty)
                {
                    this.Kill(State.DeathReason.REASON_SQUASH);
                    return false;
                }

                this._foundHome = true;
                return true;
            }


            /// <summary>
            /// Used to kill the frog.
            /// </summary>
            public void Kill(DeathReason reason)
            {
                if (!this._frogState.Equals(FrogState.STATE_ALIVE) || this._safe)
                {
                    return;
                }

                // Sets the transform.
                Transform transform = this._frogInstance.Component?.transform;
                if (transform != null)
                {
                    transform.parent = null;
                }

                this._frogState = FrogState.STATE_DYING;
                this.KillEvent(reason);
            }

            /// <summary>
            /// Calls the death event.
            /// </summary>
            public void CallDeathEvent()
            {
                if (!this._frogState.Equals(FrogState.STATE_DYING))
                {
                    return;
                }

                this._frogState = FrogState.STATE_DEAD;
                this.DeathEvent();
            }

            #endregion
        }

        /// <summary>
        /// Class used to control the lady frog state.
        /// </summary>
        public class LadyFrogStateController : AFrogStateController
        {

            #region properties

            /// <summary>
            /// Reference to the parent transform.
            /// </summary>
            public GameObject Parent
                => this._parentTransform;

            #endregion

            #region fields

            /// <summary>
            /// Reference to the parent transform this frog is associated
            /// with.
            /// </summary>
            private GameObject _parentTransform;

            #endregion

            #region constructor

            /// <summary>
            /// The Lady Frog State Controller.
            /// </summary>
            /// <param name="frogInstance"></param>
            public LadyFrogStateController(LadyFrogInstance frogInstance)
                : base(frogInstance)
            {
                this._parentTransform = null;
            }

            #endregion

            #region methods

            public override void HookEvents()
            {
                this._frogInstance.JumpBeginEvent += this.OnJumpBegin;
                this._frogInstance.JumpStopEvent += this.OnJumpEnd;
            }

            public override void UnHookEvents()
            {
                this._frogInstance.JumpBeginEvent -= this.OnJumpBegin;
                this._frogInstance.JumpStopEvent -= this.OnJumpEnd;
            }


            /// <summary>
            /// Called when the jump has begun.
            /// </summary>
            /// <param name="direction">The direction of the jump.</param>
            private void OnJumpBegin(Vector2 direction)
            {
                Transform transform = this._frogInstance.Component.transform.parent;

                // Sets the parent game object reference.
                if (transform != null && this._parentTransform != null)
                {
                    this._parentTransform = transform.gameObject;
                }
            }

            /// <summary>
            /// Called when the jump has ended.
            /// </summary>
            /// <param name="reason">The reason why the jump has ended.</param>
            private void OnJumpEnd(Movement.JumpStopReason reason) { }

            #endregion
        }
    }

    /// <summary>
    /// Defines the frog types.
    /// </summary>
    [System.Serializable]
    public enum FrogType
    {
        FROG_PLAYER,
        FROG_LADY
    }

    /// <summary>
    /// The Abstract class definition of a Frog Instance.
    /// </summary>
    public abstract class AFrogInstance
    {

        #region events

        // Event to handle trigger collisions.
        public event Action<Collider2D> TriggerEnterEvent
            = delegate { };

        // Event to handle exit trigger collisions.
        public event Action<Collider2D> TriggerExitEvent
            = delegate { };

        /// <summary>
        /// Jump begin event property definition.
        /// </summary>
        public event Action<Vector2> JumpBeginEvent
        {
            add
            {
                if (this.JumpController != null)
                {
                    this.JumpController.JumpBeginEvent += value;
                }
            }
            remove
            {
                if (this.JumpController != null)
                {
                    this.JumpController.JumpBeginEvent -= value;
                }
            }
        }

        /// <summary>
        /// Jump stop event property definition.
        /// </summary>
        public event Action<Movement.JumpStopReason> JumpStopEvent
        {
            add
            {
                if (this.JumpController != null)
                {
                    this.JumpController.JumpStopEvent += value;
                }
            }
            remove
            {
                if (this.JumpController != null)
                {
                    this.JumpController.JumpStopEvent -= value;
                }
            }
        }


        #endregion

        #region properties


        /// <summary>
        /// Gets the frog type.
        /// </summary>
        public FrogType FrogType
        {
            get
            {
                return this._frogType;
            }
        }

        /// <summary>
        /// The component of this frog.
        /// </summary>
        public FrogComponent Component
        {
            get
            {
                return this._frogComponent;
            }
        }

        /// <summary>
        /// The state controller of this frog.
        /// </summary>
        public abstract State.AFrogStateController StateController
        {
            get;
        }

        /// <summary>
        /// The jump controller of this frog.
        /// </summary>
        public abstract Movement.AFrogJumpController JumpController
        {
            get;
        }

        /// <summary>
        /// The animation controller of this frog.
        /// </summary>
        public abstract Animation.FrogAnimationController AnimationController
        {
            get;
        }

        #endregion

        #region fields

        /// <summary>
        /// The current Frog Type.
        /// </summary>
        protected readonly FrogType _frogType;

        /// <summary>
        /// Reference to the frog component.
        /// </summary>
        protected readonly FrogComponent _frogComponent;

        // Determines whether frog is initialized or not.
        private bool _initialized = false;

        #endregion

        #region constructor

        /// <summary>
        /// The Frog Instance Definition.
        /// </summary>
        /// <param name="frogComponent">The Frog Component Reference.</param>
        /// <param name="frogType">The type of frog.</param>
        /// <param name="animController">The animation controller.</param>
        public AFrogInstance(FrogComponent frogComponent, FrogType frogType)
        {
            this._frogType = frogType;
            this._frogComponent = frogComponent;
        }

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events to the frog instance.
        /// </summary>
        public virtual void HookEvents()
        {
            this.JumpController?.HookEvents();
            this.AnimationController?.HookEvents();
            this.StateController?.HookEvents();

            this._initialized = true;
        }

        /// <summary>
        /// Unhooks the events from the frog instance.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this.JumpController?.UnHookEvents();
            this.StateController?.UnHookEvents();
            this.AnimationController?.UnHookEvents();
        }

        /// <summary>
        /// Called when the frog enters a new trigger collision.
        /// </summary>
        /// <param name="collider">The collider entered.</param>
        public void OnTriggerEnter(Collider2D collider)
        {
            this.TriggerEnterEvent(collider);
        }

        /// <summary>
        /// Called when the frog exits a trigger collision.
        /// </summary>
        /// <param name="collider">the collider entered.</param>
        public void OnTriggerExit(Collider2D collider)
        {
            this.TriggerExitEvent(collider);
        }

        /// <summary>
        /// Called when the frog enters a home space.
        /// </summary>
        /// <param name="homeSpace">The home space.</param>
        /// <returns>True if the frog entered the home space succesfully, false otherwise.
        ///          Defaults to false as the player frog should handle this.</returns>
        public virtual bool OnEnterHomeSpace(Misc.HomeSpace homeSpace)
        {
            return false;
        }

        /// <summary>
        /// Called when the death sequence has been completed.
        /// </summary>
        abstract public void OnDeathSequenceCompleted();

        /// <summary>
        /// Updates the frog.
        /// </summary>
        public virtual void Update()
        {
            if (!this._initialized)
            {
                return;
            }

            // Updates the jumping of the controller.
            State.FrogState currentState = this.StateController.FrogState;
            if (currentState == State.FrogState.STATE_ALIVE)
            {
                this.JumpController.Update();
            }

            this.StateController.Update();
        }

        #endregion

        #region static_methods

        /// <summary>
        /// Creates a new instance of the frog.
        /// </summary>
        /// <param name="component">The frog component.</param>
        /// <param name="settings">The frog settings scriptable object.</param>
        /// <returns>A new frog instance based on the inputs.</returns>
        public static AFrogInstance CreateInstance(FrogComponent component, Frog.FrogSettings settings)
        {
            switch (settings.CurrentFrogType)
            {
                case Frog.FrogType.FROG_LADY:
                    return new LadyFrogInstance(
                        component, settings.JumpData, settings.AnimatorController);
                case Frog.FrogType.FROG_PLAYER:
                    return new PlayerFrogInstance(
                        component, settings);
            }

            return null;
        }

        #endregion
    }


    /// <summary>
    /// The Player Frog Instance Defintion.
    /// </summary>
    public class PlayerFrogInstance : AFrogInstance
    {
        #region events

        /// <summary>
        /// Death event used by static classes.
        /// </summary>
        public static event Action<PlayerFrogInstance> GlobalDeathEvent
            = delegate { };

        /// <summary>
        /// The Trigger stay event.
        /// </summary>
        public event Action<Collider2D> TriggerStayEvent
            = delegate { };

        /// <summary>
        /// The Kill Event Shortcut.
        /// </summary>
        public event Action<State.DeathReason> KillEvent
        {
            add
            {
                this._frogStateController.KillEvent += value;
            }
            remove
            {
                this._frogStateController.KillEvent -= value;
            }
        }

        /// <summary>
        /// The Lady frog collide event shortcut.
        /// </summary>
        public event Action<LadyFrogInstance> LadyFrogCollideEvent
        {
            add
            {
                this._frogStateController.LadyFrogCollideEvent += value;
            }
            remove
            {
                this._frogStateController.LadyFrogCollideEvent -= value;
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the state controller of the frog.
        /// </summary>
        public override State.AFrogStateController StateController =>
            this._frogStateController;

        /// <summary>
        /// Gets the jump controller of the frog.
        /// </summary>
        public override Movement.AFrogJumpController JumpController =>
            this._jumpController;

        /// <summary>
        /// Gets the animation controller.
        /// </summary>
        public override Animation.FrogAnimationController AnimationController
            => this._animationController;

        #endregion

        #region fields

        /// <summary>
        /// The Frog State controller.
        /// </summary>
        protected State.PlayerFrogStateController _frogStateController;

        /// <summary>
        /// The Frog Jump Controller.
        /// </summary>
        protected Movement.PlayerFrogJumpController _jumpController;

        /// <summary>
        /// The frog animation controller.
        /// </summary>
        protected Animation.PlayerFrogAnimationController _animationController;

        /// <summary>
        /// The frog audio controller.
        /// </summary>
        private Audio.PlayerFrogAudioController _audioController;

        #endregion

        #region constructor

        public PlayerFrogInstance(FrogComponent frogComponent, FrogSettings settings)
            : base(frogComponent, FrogType.FROG_PLAYER)
        {
            this._frogStateController = new State.PlayerFrogStateController(this);
            this._jumpController = new Movement.PlayerFrogJumpController(this, settings.JumpData);
            this._animationController = new Animation.PlayerFrogAnimationController(this, settings.AnimatorController);
            this._audioController = new Audio.PlayerFrogAudioController(this, settings.AudioData);
        }

        #endregion

        #region methods

        public override void HookEvents()
        {
            base.HookEvents();
            this._audioController.HookEvents();
        }

        public override void UnHookEvents()
        {
            base.UnHookEvents();
            this._audioController.UnHookEvents();
        }

        /// <summary>
        /// Called when the frog collides with a trigger but
        /// stays.
        /// </summary>
        /// <param name="collider">The collider.</param>
        public void OnTriggerStay(Collider2D collider)
        {
            this.TriggerStayEvent(collider);
        }

        public override void OnDeathSequenceCompleted()
        {
            this._frogStateController.CallDeathEvent();
            GlobalDeathEvent(this);
        }

        public override bool OnEnterHomeSpace(Misc.HomeSpace homeSpace)
        {
            return this._frogStateController.OnEnterHomeSpace(homeSpace);
        }

        #endregion
    }

    /// <summary>
    /// The Lady Frog Instance Definition
    /// </summary>
    public class LadyFrogInstance : AFrogInstance
    {

        #region properties

        /// <summary>
        /// The State Controller property reference.
        /// </summary>
        public override State.AFrogStateController StateController
            => this._frogStateController;

        /// <summary>
        /// The Jump controller property reference.
        /// </summary>
        public override Movement.AFrogJumpController JumpController
            => this._jumpController;

        /// <summary>
        /// The Animation controller.
        /// </summary>
        public override Animation.FrogAnimationController AnimationController
            => this._animationController;

        #endregion

        #region fields

        /// <summary>
        /// The Frog State Controller.
        /// </summary>
        protected State.LadyFrogStateController _frogStateController;

        /// <summary>
        /// The lady frog jump controller.
        /// </summary>
        protected Movement.LadyFrogJumpController _jumpController;

        /// <summary>
        /// The animation controller of the lady frog.
        /// </summary>
        protected Animation.FrogAnimationController _animationController;

        #endregion

        #region constructor

        public LadyFrogInstance(FrogComponent component, Movement.JumpData settings, RuntimeAnimatorController animController)
            : base(component, FrogType.FROG_LADY)
        {
            this._frogStateController = new State.LadyFrogStateController(this);
            this._jumpController = new Movement.LadyFrogJumpController(this, settings);
            this._animationController = new Animation.FrogAnimationController(this, animController);
        }

        #endregion

        #region methods

        // Does nothing here as it's not ever called.
        public override void OnDeathSequenceCompleted() { }

        #endregion
    }
}

