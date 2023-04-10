using System;
using System.Linq;
using JetBrains.Annotations;
using poetools.Core;
using poetools.player.Player.Crouching.Colliders;
using poetools.player.Player.Rotation;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Crouching
{
    /// <summary>
    /// A MonoBehaviour wrapper for an instance of <see cref="FPSCrouchingLogic"/>.
    /// Use this to easily define settings and targets in the inspector, and find it in
    /// GetComponent queries.
    /// </summary>
    [HideMonoScript]
    [DeclareTabGroup("tabs")]
    public class FPSCrouchingLogicContainer : MonoBehaviour, IInputProvider
    {
        [Group("tabs"), Tab("Settings")]
        [Required]
        [HideLabel]
        [InlineEditor]
        [SerializeField]
        [ScriptableAssetReference]
        private CrouchingSettingsAsset settingsAsset;

        [Group("tabs"), Tab("References")]
        [SerializeField]
        [Tooltip("The transform that will be moved to eye height when the player crouches.")]
        public Transform crouchTransform;

        [Group("tabs"), Tab("References")]
        [SerializeField]
        [Tooltip("A transform that follows the base of the object smoothly, since the normal transform is often teleported.")]
        private Transform steadyBase;

        [Group("tabs"), Tab("References")]
        [SerializeField]
        [Tooltip("The component used for checking if this object is currently grounded.")]
        private GroundCheck groundCheck;

        [Group("tabs"), Tab("References")]
        [SerializeField]
        [Tooltip("The component used for checking if this object is currently grounded.")]
        private CrouchingCollider crouchingCollider;

        [Group("tabs"), Tab("Input")]
        [PropertyTooltip("Should this container automatically supply inputs to the underlying logic.")]
        [SerializeField]
        private bool automaticallyProvideInput = true;

        [Group("tabs"), Tab("Input")]
        [ShowIf(nameof(automaticallyProvideInput))]
        [PropertyTooltip("The key that should be held to make this object crouch.")]
        [SerializeField]
        private KeyCode crouchKey = KeyCode.LeftShift;

        private FPSCrouchingLogic _crouchingLogic;

        /// <summary>
        /// Gets the crouching logic that is setup and controlled by this container.
        /// </summary>
        public FPSCrouchingLogic CrouchingLogic =>
            // Lazy initialization, also to simply order-of-initialization for users.
            _crouchingLogic ??= new FPSCrouchingLogic(settingsAsset.Generate(), transform)
            {
                SmoothedCrouchTransform = crouchTransform,
                SteadyBase = steadyBase,
                GroundCheck = groundCheck,
                CrouchingCollider = crouchingCollider,
            };

        public bool Active { get; set; } = true;

        private bool HasMissingReferences => crouchTransform == null || steadyBase == null || groundCheck == null || crouchingCollider == null || settingsAsset == null;

        [Button]
        [UsedImplicitly]
        [GUIColor(1, 51/255f, 51/255f)]
        [ShowIf(nameof(HasMissingReferences))]
        private void TryFixReferences()
        {
            #if UNITY_EDITOR
            if (crouchTransform == null)
            {
                crouchTransform = GetComponentsInChildren<Transform>()
                    .FirstOrDefault(target => target.name.Equals("yaw", StringComparison.InvariantCultureIgnoreCase) || target.name.Equals("crouch transform", StringComparison.InvariantCultureIgnoreCase));

                if (crouchTransform == null)
                {
                    var rotationLogic = GetComponent<FPSRotationLogicContainer>();

                    if (rotationLogic != null && rotationLogic.YawTransforms.Count > 0)
                        crouchTransform = rotationLogic.YawTransforms[0];

                    else
                    {
                        var go = new GameObject("Crouch Transform");
                        go.transform.SetParent(transform);
                        crouchTransform = go.transform;
                    }
                }
            }

            if (steadyBase == null)
            {
                steadyBase = GetComponentsInChildren<Transform>()
                    .FirstOrDefault(target => target.name.Equals("steady base", StringComparison.InvariantCultureIgnoreCase));

                if (steadyBase == null)
                {
                    var go = new GameObject("Steady Base");
                    go.transform.SetParent(transform);
                    steadyBase = go.transform;
                }
            }

            if (crouchingCollider == null)
            {
                crouchingCollider = GetComponentInChildren<CrouchingCollider>();

                if (crouchingCollider == null)
                    crouchingCollider = EditorFactoryUnityImpl.AddCrouchingCapsuleWithUndo(gameObject);
            }

            if (groundCheck == null)
            {
                groundCheck = GetComponentInChildren<GroundCheck>();

                if (groundCheck == null)
                    groundCheck = EditorFactoryUnityImpl.AddGroundCheckWithUndo(gameObject);
            }

            if (settingsAsset == null)
                settingsAsset = ScriptableObject.CreateInstance<CrouchingSettingsAsset>();
            #endif
        }

        private void Update()
        {
            if (automaticallyProvideInput && Active)
                CrouchingLogic.WantsToCrouch = Input.GetKey(crouchKey);

            CrouchingLogic.Tick();
        }

        private void FixedUpdate()
        {
            CrouchingLogic.PhysicsTick();
        }
    }
}
