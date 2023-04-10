using System;
using System.Linq;
using JetBrains.Annotations;
using poetools.Core;
using poetools.Core.Abstraction;
using poetools.player.Player.Rotation;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace poetools.player.Player.Movement
{
    /// <summary>
    /// A MonoBehaviour wrapper for an instance of <see cref="FPSQuakeMovementLogic"/>.
    /// Use this to easily define settings and targets in the inspector, and find it in
    /// GetComponent queries.
    /// </summary>
    [HideMonoScript]
    [DeclareTabGroup("tabs")]
    public class FPSQuakeMovementLogicContainer : MonoBehaviour, IInputProvider
    {
        [Group("tabs"), Tab("Settings")]
        [Required]
        [HideLabel]
        [InlineEditor]
        [SerializeField]
        [ScriptableAssetReference]
        private FPSQuakeMovementSettingsAsset settingsAsset;

        [Group("tabs"), Tab("References")]
        [ValidateInput(nameof(ValidateGroundCheck))]
        [PropertyTooltip("The component used for check if this object is on the ground or not.")]
        [SerializeField]
        private GroundCheck groundCheck;

        [FormerlySerializedAs("physics")]
        [ValidateInput(nameof(ValidatePhysics))]
        [Group("tabs"), Tab("References")]
        [PropertyTooltip("The physics object that the movement forces will be applied to.")]
        [SerializeField]
        private PhysicsComponent physicsComponent;

        [Group("tabs"), Tab("References")]
        [ValidateInput(nameof(ValidateYawTransform))]
        [PropertyTooltip("The transform that will be used to determine the forward and right directions of movement.")]
        [SerializeField]
        public Transform yawTransform;

        [Group("tabs"), Tab("Input")]
        [PropertyTooltip("Should this container automatically supply inputs to the underlying logic.")]
        [SerializeField]
        private bool automaticallyProvideInput = true;

        private FPSQuakeMovementLogic _quakeMovementLogic;

        public FPSQuakeMovementLogic QuakeMovementLogic =>
            _quakeMovementLogic ??= new FPSQuakeMovementLogic(settingsAsset.Generate())
            {
                GroundCheck = groundCheck,
                PhysicsComponent = physicsComponent,
                YawTransform = yawTransform,
            };

        public bool Active { get; set; } = true;

        private void Update()
        {
            if (automaticallyProvideInput && Active)
            {
                var direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                QuakeMovementLogic.TargetDirection = direction;
            }
        }

        private void FixedUpdate()
        {
            QuakeMovementLogic.PhysicsTick();
        }

        private bool HasMissingReferences => physicsComponent == null || yawTransform == null || groundCheck == null || settingsAsset == null;

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

            if (groundCheck == null)
                groundCheck = EditorFactoryUnityImpl.AddGroundCheckWithUndo(gameObject);

            if (physicsComponent == null)
                physicsComponent = EditorFactoryUnityImpl.AddPhysicsRigidbodyWithUndo(gameObject);

            if (yawTransform == null)
            {
                var rotationSystem = GetComponent<FPSRotationLogicContainer>();

                if (rotationSystem != null && rotationSystem.YawTransforms.Count > 0)
                {
                    yawTransform = rotationSystem.YawTransforms[0];
                }
                else
                {
                    yawTransform = GetComponentsInChildren<Transform>()
                        .FirstOrDefault(target => target.name.Equals("yaw", StringComparison.InvariantCultureIgnoreCase));

                    if (yawTransform == null)
                    {
                        var cam = GetComponentInChildren<Camera>();
                        yawTransform = cam != null ? cam.transform : transform;
                    }
                }
            }

            if (settingsAsset == null)
                settingsAsset = ScriptableObject.CreateInstance<FPSQuakeMovementSettingsAsset>();

            // Finalize undo.
            UnityEditor.Undo.CollapseUndoOperations(UnityEditor.Undo.GetCurrentGroup());

#endif
        }

        private TriValidationResult ValidatePhysics()
        {
            if (physicsComponent == null)
                return TriValidationResult.Error("Without physics, this object will not respond to any movement inputs.");

            return TriValidationResult.Valid;
        }

        private TriValidationResult ValidateGroundCheck()
        {
            if (groundCheck == null)
                return new TriValidationResult(false, "Without a ground check, this object will always assume it is grounded.", TriMessageType.Info);

            return TriValidationResult.Valid;
        }

        private TriValidationResult ValidateYawTransform()
        {
            if (yawTransform == null)
                return TriValidationResult.Warning("Without a yaw transform, this object will only be able to move forward along positive Z.");

            return TriValidationResult.Valid;
        }
    }
}
