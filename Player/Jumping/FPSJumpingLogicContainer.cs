using JetBrains.Annotations;
using poetools.Core;
using poetools.Core.Abstraction;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Jumping
{
    /// <summary>
    /// A MonoBehaviour wrapper for an instance of <see cref="FPSJumpingLogic"/>.
    /// Use this to easily define settings and targets in the inspector, and find it in
    /// GetComponent queries.
    /// </summary>
    [HideMonoScript]
    [DeclareTabGroup("tabs")]
    public class FPSJumpingLogicContainer : MonoBehaviour, IInputProvider
    {
        [Group("tabs"), Tab("Settings")]
        [Required]
        [HideLabel]
        [InlineEditor]
        [SerializeField]
        [ScriptableAssetReference]
        private JumpingSettingsAsset settingsAsset;

        [Group("tabs"), Tab("References")]
        [ValidateInput(nameof(ValidatePhysics))]
        [PropertyTooltip("The physics object to which the jumping forces will be applied.")]
        [SerializeField]
        private PhysicsComponent physicsComponent;

        [Group("tabs"), Tab("References")]
        [ValidateInput(nameof(ValidateGroundCheck))]
        [PropertyTooltip("The ground checker that passes information on if this object is grounded.")]
        [SerializeField]
        private GroundCheck groundCheck;

        [Group("tabs"), Tab("References")]
        [ValidateInput(nameof(ValidateGravity))]
        [PropertyTooltip("The gravity applier that should be used for moving this object.")]
        [SerializeField]
        private Gravity gravity;

        [Group("tabs"), Tab("Input")]
        [PropertyTooltip("Should this container automatically supply inputs to the underlying logic.")]
        [SerializeField]
        private bool automaticallyProvideInput = true;

        [Group("tabs"), Tab("Input")]
        [ShowIf(nameof(automaticallyProvideInput))]
        [PropertyTooltip("Should scrolling the mouse wheel cause this object to jump.")]
        [SerializeField]
        private bool scrollToJump;

        [Group("tabs"), Tab("Input")]
        [ShowIf(nameof(automaticallyProvideInput))]
        [PropertyTooltip("Should holding the jump key cause this object to jump.")]
        [SerializeField]
        private bool holdToJump;

        [Group("tabs"), Tab("Input")]
        [ShowIf(nameof(automaticallyProvideInput))]
        [PropertyTooltip("The key that should be held to cause this object to jump.")]
        [SerializeField]
        private KeyCode jumpKey = KeyCode.Space;

        private FPSJumpingLogic _jumpingLogic;

        /// <summary>
        /// Gets the jumping logic that is setup and controller by this container.
        /// </summary>
        public FPSJumpingLogic JumpingLogic
        {
            get
            {
                // Lazy initialization, also to simply order-of-initialization for users.
                _jumpingLogic ??= new FPSJumpingLogic(settingsAsset.Generate())
                {
                    Physics = physicsComponent,
                    Gravity = gravity,
                    GroundCheck = groundCheck,
                };

                return _jumpingLogic;
            }
        }

        public bool Active { get; set; } = true;

        private bool HasMissingReferences => physicsComponent == null || groundCheck == null || gravity == null || settingsAsset == null;

        private void Update()
        {
            if (automaticallyProvideInput && Active)
            {
                bool wantsToJump = Input.GetKeyDown(jumpKey)
                                   || holdToJump && Input.GetKey(jumpKey)
                                   || scrollToJump && Input.mouseScrollDelta.y != 0;

                if (wantsToJump)
                    JumpingLogic.Jump();

                JumpingLogic.HoldingJump = Input.GetKey(jumpKey);
            }

            JumpingLogic.Tick();
        }

        private void FixedUpdate()
        {
            JumpingLogic.PhysicsTick();
        }

        [Button]
        [UsedImplicitly]
        [GUIColor(1, 51/255f, 51/255f)]
        [ShowIf(nameof(HasMissingReferences))]
        private void TryFixReferences()
        {
            #if UNITY_EDITOR

            // Setup undo.
            UnityEditor.Undo.IncrementCurrentGroup();
            UnityEditor.Undo.SetCurrentGroupName("FPS Jumping Container Fix References");

            if (physicsComponent == null)
                physicsComponent = EditorFactoryUnityImpl.AddPhysicsRigidbodyWithUndo(gameObject);

            if (groundCheck == null)
                groundCheck = EditorFactoryUnityImpl.AddGroundCheckWithUndo(gameObject);

            if (gravity == null)
                gravity = EditorFactoryUnityImpl.AddGravityWithUndo(gameObject);

            if (settingsAsset == null)
                settingsAsset = ScriptableObject.CreateInstance<JumpingSettingsAsset>();

            // Finalize undo.
            UnityEditor.Undo.CollapseUndoOperations(UnityEditor.Undo.GetCurrentGroup());

            #endif
        }

        private TriValidationResult ValidatePhysics()
        {
            if (physicsComponent == null)
                return TriValidationResult.Error("Without physics, this object will not respond to any jump requests.");

            return TriValidationResult.Valid;
        }

        private TriValidationResult ValidateGroundCheck()
        {
            if (groundCheck == null)
                return new TriValidationResult(false, "Without a ground check, this object will always assume it is grounded.", TriMessageType.Info);

            return TriValidationResult.Valid;
        }

        private TriValidationResult ValidateGravity()
        {
            if (gravity == null)
                return TriValidationResult.Warning("Without gravity, the requested jumping settings may be incorrectly applied.");

            return TriValidationResult.Valid;
        }
    }
}
