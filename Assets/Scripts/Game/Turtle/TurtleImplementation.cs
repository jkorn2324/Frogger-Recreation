using UnityEngine;

namespace Frogger.Game.Turtle
{

    namespace Animation
    {
        /// <summary>
        /// The turtle animator controller.
        /// </summary>
        public class TurtleAnimationController
        {

            #region fields

            // The instance of the turtle.
            protected ATurtleInstance _instance;

            // The animator instance.
            protected Animator _animator;

            #endregion

            #region constructor

            /// <summary>
            /// The animator controller of the turtle.
            /// </summary>
            /// <param name="animatorController">The animator controller.</param>
            public TurtleAnimationController(ATurtleInstance instance, RuntimeAnimatorController animatorController)
            {
                this._instance = instance;

                this._animator = instance.Component.gameObject.AddComponent<Animator>();
                this._animator.runtimeAnimatorController = animatorController;
                this._animator.updateMode = AnimatorUpdateMode.Normal;
                this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }

            #endregion

            #region methods

            /// <summary>
            /// Hooks the events to the turtle.
            /// </summary>
            public virtual void HookEvents() { }

            /// <summary>
            /// Unhooks the events from the turtle.
            /// </summary>
            public virtual void UnHookEvents() { }

            #endregion
        }

        /// <summary>
        /// The disappearing turtle animation controller.
        /// </summary>
        public class DisappearingTurtleAnimationController : TurtleAnimationController
        {

            #region constructor

            /// <summary>
            /// The disappearing turtle animation controller constructor.
            /// </summary>
            /// <param name="instance">The instance of the turtle.</param>
            /// <param name="controller">The runtime animator controller.</param>
            public DisappearingTurtleAnimationController(DisappearingTurtleInstance instance, RuntimeAnimatorController controller)
                : base(instance, controller) { }

            #endregion

            #region methods

            /// <summary>
            /// Hooks the events from the Turtle Instance. 
            /// </summary>
            public override void HookEvents()
            {
                base.HookEvents();

                DisappearingTurtleInstance instance = (DisappearingTurtleInstance)this._instance;
                instance.UnderwaterEvent += this.OnTurtleUnderwater;
                instance.OverwaterEvent += this.OnTurtleOverwater;
            }

            /// <summary>
            /// Unhooks the events from the Turtle Instance.
            /// </summary>
            public override void UnHookEvents()
            {
                base.UnHookEvents();

                DisappearingTurtleInstance instance = (DisappearingTurtleInstance)this._instance;
                instance.UnderwaterEvent -= this.OnTurtleUnderwater;
                instance.OverwaterEvent -= this.OnTurtleOverwater;
            }

            /// <summary>
            /// Called when the turtle goes underwater.
            /// </summary>
            private void OnTurtleUnderwater()
            {
                this._animator.enabled = false;
            }

            /// <summary>
            /// Called when the turtle goes over water.
            /// </summary>
            private void OnTurtleOverwater()
            {
                this._animator.enabled = true;
            }

            #endregion
        }
    }

    /// <summary>
    /// Contains the classes that handle the frog state.
    /// </summary>
    namespace State
    {

        /// <summary>
        /// Determines whether the turtle is above water.
        /// </summary>
        [System.Serializable]
        public enum TurtleState
        {
            STATE_ABOVE_WATER,
            STATE_BELOW_WATER
        }

        /// <summary>
        /// The abstract turtle state controller class.
        /// </summary>
        public abstract class ATurtleStateController
        {
            #region properties

            /// <summary>
            /// The turtle state.
            /// </summary>
            public TurtleState State
            {
                get
                {
                    return this._state;
                }
            }

            #endregion

            #region fields

            // The instance of the turtle reference.
            protected ATurtleInstance _instance;

            // Field of the turtle state.
            protected TurtleState _state;

            #endregion

            #region constructor

            /// <summary>
            /// The turtle state controller. 
            /// </summary>
            /// <param name="instance">The instance of the turtle.</param>
            public ATurtleStateController(ATurtleInstance instance)
            {
                this._instance = instance;
                this._state = TurtleState.STATE_ABOVE_WATER;
            }

            #endregion

            #region methods

            /// <summary>
            /// Called to hook the events of the turtle state.
            /// </summary>
            public virtual void HookEvents() { }

            /// <summary>
            /// Called to unhook the events of the turtle.
            /// </summary>
            public virtual void UnHookEvents() { }

            #endregion
        }

        /// <summary>
        /// Handles a turtle that disappears.
        /// </summary>
        public class DisappearingTurtleStateController : ATurtleStateController
        {

            #region events

            /// <summary>
            /// Called when the turtle is above water.
            /// </summary>
            public event System.Action OverwaterEvent
                = delegate { };

            #endregion

            #region properties

            /// <summary>
            /// Determines whether the turtle is underwater.
            /// </summary>
            public bool Underwater
                => this._state.Equals(TurtleState.STATE_BELOW_WATER);

            #endregion

            #region fields

            // Reference to the sprite renderer.
            private SpriteRenderer _renderer;

            // The basic timer reference.
            private Utils.BasicTimer _timer;

            #endregion

            #region constructor

            /// <summary>
            /// The disappearing turtle state constructor.
            /// </summary>
            /// <param name="instance">The instance of the turtle.</param>
            /// <param name="underwaterTimeSeconds">The maximum amount of time the turtle is underwater.</param>
            public DisappearingTurtleStateController(DisappearingTurtleInstance instance, float underwaterTimeSeconds)
                : base(instance)
            {
                this._renderer = instance.Component.GetComponent<SpriteRenderer>();
                this._timer = new Utils.BasicTimer(System.TimeSpan.FromSeconds(underwaterTimeSeconds));
            }

            #endregion

            #region methods

            public override void HookEvents()
            {
                base.HookEvents();

                DisappearingTurtleInstance instance = (DisappearingTurtleInstance)this._instance;
                instance.UnderwaterEvent += this.OnTurtleUnderwater;
            }

            public override void UnHookEvents()
            {
                base.UnHookEvents();

                DisappearingTurtleInstance instance = (DisappearingTurtleInstance)this._instance;
                instance.UnderwaterEvent -= this.OnTurtleUnderwater;
            }

            /// <summary>
            /// Used to update the state of the turtle every frame.
            /// </summary>
            public void Update()
            {
                if (this.Underwater && this._timer.Completed)
                {
                    this.SetTurtleAboveWater();
                }
            }

            /// <summary>
            /// Called when the turtle goes underwater.
            /// </summary>
            private void OnTurtleUnderwater()
            {

                if (this.Underwater)
                {
                    return;
                }

                this._timer.Reset();

                this._renderer.enabled = false;

                this._state = TurtleState.STATE_BELOW_WATER;
            }

            /// <summary>
            /// Sets the turtle above water.
            /// </summary>
            private void SetTurtleAboveWater()
            {
                this._renderer.enabled = true;

                this._state = TurtleState.STATE_ABOVE_WATER;

                this.OverwaterEvent();
            }

            #endregion
        }

        /// <summary>
        /// The normal turtle state controller definition.
        /// </summary>
        public class NormalTurtleStateController : ATurtleStateController
        {
            public NormalTurtleStateController(NormalTurtleInstance instance)
                : base(instance) { }
        }
    }

    /// <summary>
    /// The Types of Turtles.
    /// </summary>
    [System.Serializable]
    public enum TurtleType
    {
        TURTLE_NORMAL,
        TURTLE_DISAPPEARING
    }

    /// <summary>
    /// The abstract turtle instance.
    /// </summary>
    public abstract class ATurtleInstance
    {

        #region properties

        /// <summary>
        /// The turtle component property.
        /// </summary>
        public TurtleComponent Component
            => this._component;


        /// <summary>
        /// Gets the animator controller.
        /// </summary>
        public abstract Animation.TurtleAnimationController AnimationController
        {
            get;
        }

        /// <summary>
        /// Gets the state controller.
        /// </summary>
        public abstract State.ATurtleStateController StateController
        {
            get;
        }

        /// <summary>
        /// The turtle type reference shortcut.
        /// </summary>
        public TurtleType Type
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        /// The turtle state reference shortcut.
        /// </summary>

        public State.TurtleState State
        {
            get
            {
                return this.StateController.State;
            }
        }

        #endregion

        #region fields

        // The field for the component.
        protected TurtleComponent _component;

        // The turtle type field.
        private readonly TurtleType _type;

        #endregion

        #region constructor

        /// <summary>
        /// The Turtle instance constructor.
        /// </summary>
        /// <param name="component">The turtle component.</param>
        public ATurtleInstance(TurtleComponent component, TurtleType type)
        {
            this._component = component;
            this._type = type;
        }

        /// <summary>
        /// Called when the turtle was disappeared.
        /// </summary>
        abstract public void OnTurtleDisappeared();

        #endregion

        #region methods

        /// <summary>
        /// Hooks the events to the turtle instance.
        /// </summary>
        public virtual void HookEvents()
        {
            this.AnimationController?.HookEvents();
            this.StateController?.HookEvents();
        }

        /// <summary>
        /// Unhooks the events from the turtle instance.
        /// </summary>
        public virtual void UnHookEvents()
        {
            this.AnimationController?.UnHookEvents();
            this.StateController?.UnHookEvents();
        }

        /// <summary>
        /// Used to update the turtle every frame.
        /// </summary>
        public virtual void Update() { }

        #endregion

        #region static_methods

        /// <summary>
        /// Creates the instance of the turtle.
        /// </summary>
        /// <param name="component">The turtle component reference.</param>
        /// <param name="settings">The turtle settings.</param>
        /// <returns>Null or a new Turtle instance.</returns>
        public static ATurtleInstance CreateInstance(TurtleComponent component, TurtleSettings settings)
        {
            switch (settings.TurtleType)
            {
                case TurtleType.TURTLE_DISAPPEARING:
                    return new DisappearingTurtleInstance(component, settings);
                case TurtleType.TURTLE_NORMAL:
                    return new NormalTurtleInstance(component, settings.GetAnimatorController());
            }

            return null;
        }

        #endregion
    }

    /// <summary>
    /// Definition for the disappearing turtle instance.
    /// </summary>
    public class DisappearingTurtleInstance : ATurtleInstance
    {

        #region events

        /// <summary>
        /// Called when the turtle goes underwater. 
        /// </summary>
        public event System.Action UnderwaterEvent
            = delegate { };

        /// <summary>
        /// The over water event shortcut.
        /// </summary>
        public event System.Action OverwaterEvent
        {
            add
            {
                this._stateController.OverwaterEvent += value;
            }
            remove
            {
                this._stateController.OverwaterEvent -= value;
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the animator controller from the turtle instance.
        /// </summary>
        public override Animation.TurtleAnimationController AnimationController
            => this._animationController;

        /// <summary>
        /// The state controller from the turtle instance.
        /// </summary>
        public override State.ATurtleStateController StateController
            => this._stateController;

        #endregion

        #region fields

        // Reference to the animation controller.
        private Animation.DisappearingTurtleAnimationController _animationController;
        // Reference to the state controller.
        private State.DisappearingTurtleStateController _stateController;

        #endregion

        #region constructor

        /// <summary>
        /// The Disappearing turtle instance constructor.
        /// </summary>
        /// <param name="component">The component we are referencing.</param>
        /// <param name="settings">The turtle settings.</param>
        public DisappearingTurtleInstance(TurtleComponent component, TurtleSettings settings)
            : base(component, TurtleType.TURTLE_DISAPPEARING)
        {
            this._animationController = new Animation.DisappearingTurtleAnimationController(this, settings.GetAnimatorController());
            this._stateController = new State.DisappearingTurtleStateController(this, settings.MaxSecondsUnderwater);
        }

        #endregion

        #region methods

        public override void Update()
        {
            base.Update();

            this._stateController.Update();
        }

        /// <summary>
        /// Called when the turtle has disappeared.
        /// </summary>
        public override void OnTurtleDisappeared()
        {
            this.UnderwaterEvent();
        }

        #endregion
    }

    /// <summary>
    /// Definition of a normal turtle instance.
    /// </summary>
    public class NormalTurtleInstance : ATurtleInstance
    {

        #region properties

        /// <summary>
        /// Gets the animator controller from the turtle instance.
        /// </summary>
        public override Animation.TurtleAnimationController AnimationController
            => this._animationController;

        /// <summary>
        /// The state controller from the turtle instance.
        /// </summary>
        public override State.ATurtleStateController StateController
            => this._stateController;

        #endregion

        #region fields

        // The turtle state controller.
        private State.NormalTurtleStateController _stateController;

        // The animation controller for the turtle.
        private Animation.TurtleAnimationController _animationController;

        #endregion

        #region constructor

        public NormalTurtleInstance(TurtleComponent component, RuntimeAnimatorController controller)
            : base(component, TurtleType.TURTLE_NORMAL)
        {
            this._stateController = new State.NormalTurtleStateController(this);
            this._animationController = new Animation.TurtleAnimationController(this, controller);
        }

        #endregion

        #region methods

        public override void OnTurtleDisappeared() { }

        #endregion
    }
}
