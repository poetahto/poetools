using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using poetools.Core;
using poetools.Core.Tools;
using poetools.player.Player.Crouching;
using poetools.player.Player.Interaction;
using poetools.player.Player.Movement;
using TriInspector;
using UnityEngine;

namespace poetools.player.Player.Rotation
{
    /// <summary>
    /// A MonoBehaviour wrapper for an instance of <see cref="FPSRotationLogic"/>.
    /// Use this to easily define settings and targets in the inspector, and find it in
    /// GetComponent queries.
    /// </summary>
    [HideMonoScript]
    [DeclareTabGroup("tabs")]
    public class FPSRotationLogicContainer : MonoBehaviour, IInputProvider
    {
        [Group("tabs"), Tab("Settings")]
        [Required]
        [HideLabel]
        [InlineEditor]
        [SerializeField]
        [ScriptableAssetReference]
        private RotationSettingsAsset settingsAsset;

        [ValidateInput(nameof(WarnIfNoTargets))]

        [Group("tabs"), Tab("References")]
        [Required]
        [ListDrawerSettings(Draggable = false)]
        [PropertyTooltip("The transforms that will be updated with the current pitch.")]
        [SerializeField]
        private List<Transform> pitchTransforms = new List<Transform>();

        [Group("tabs"), Tab("References")]
        [Required]
        [ListDrawerSettings(Draggable = false)]
        [PropertyTooltip("The transforms that will be updated with the current yaw.")]
        [SerializeField]
        private List<Transform> yawTransforms = new List<Transform>();

        [Group("tabs"), Tab("Input")]
        [SerializeField]
        [PropertyTooltip("Should this container automatically supply inputs to the underlying logic.")]
        private bool automaticallyProvideInput = true;

        private FPSRotationLogic _rotationLogic;

        /// <summary>
        /// Gets the rotation logic that is setup and controlled by this container.
        /// </summary>
        public FPSRotationLogic RotationLogic
        {
            get
            {
                // Lazy initialization, also to simply order-of-initialization for users.
                if (_rotationLogic == null)
                {
                    _rotationLogic = new FPSRotationLogic(settingsAsset.Generate());
                    _rotationLogic.PitchTargets.AddRange(pitchTransforms);
                    _rotationLogic.YawTargets.AddRange(yawTransforms);
                }

                return _rotationLogic;
            }
        }

        public bool Active { get; set; } = true;

        // Checks to see if the user has remembered to assign transforms in the inspector.
        private bool HasNoTargets => pitchTransforms.Count <= 0 && yawTransforms.Count <= 0;
        private bool HasMissingReferences => pitchTransforms.Count <= 0 || yawTransforms.Count <= 0 || settingsAsset == null || HasNull(yawTransforms) || HasNull(pitchTransforms);

        public List<Transform> YawTransforms => yawTransforms;
        public List<Transform> PitchTransforms => pitchTransforms;

        private static bool HasNull(IEnumerable<Transform> values)
        {
            foreach (var t in values)
            {
                if (t == null)
                    return true;
            }

            return false;
        }

        private void Update()
        {
            if (automaticallyProvideInput && Active)
            {
                var delta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                RotationLogic.ApplyDelta(delta);
            }
        }

        [Button]
        [UsedImplicitly]
        [GUIColor(1, 51/255f, 51/255f)]
        [ShowIf(nameof(HasMissingReferences))]
        private void FixMissingReferences()
        {
            for (int i = yawTransforms.Count - 1; i >= 0; i--)
            {
                if (yawTransforms[i] == null)
                    yawTransforms.RemoveAt(i);
            }

            for (int i = pitchTransforms.Count - 1; i >= 0; i--)
            {
                if (pitchTransforms[i] == null)
                    pitchTransforms.RemoveAt(i);
            }

            var cameraTarget = GetComponentInChildren<Camera>();

            bool TryFindAndAdd(string targetName, List<Transform> output)
            {
                var target = GetComponentsInChildren<Transform>()
                    .FirstOrDefault(target => target.name.Equals(targetName, StringComparison.InvariantCultureIgnoreCase));

                if (target != null)
                {
                    output.Add(target);
                    return true;
                }
                else if (cameraTarget != null)
                {
                    output.Add(cameraTarget.transform);
                    return true;
                }

                return false;
            }

            if (yawTransforms.Count <= 0 && !TryFindAndAdd("yaw", yawTransforms))
            {
                var yawObj = new GameObject("Yaw").transform;
                yawTransforms.Add(yawObj);
                yawObj.SetParent(transform);

                if (TryGetComponent(out FPSQuakeMovementLogicContainer movementLogic))
                    movementLogic.yawTransform = yawObj;

                if (TryGetComponent(out FPSCrouchingLogicContainer crouchingLogic))
                    crouchingLogic.crouchTransform = yawObj;
            }

            if (pitchTransforms.Count <= 0 && !TryFindAndAdd("pitch", pitchTransforms))
            {
                var pitchObj = new GameObject("Pitch").transform;
                pitchTransforms.Add(pitchObj);
                pitchObj.SetParent(yawTransforms[yawTransforms.Count - 1]);

                if (TryGetComponent(out FPSInteractionLogicContainer interactionLogic))
                    interactionLogic.viewDirection = pitchObj;
            }

            if (settingsAsset == null)
                settingsAsset = ScriptableObject.CreateInstance<RotationSettingsAsset>();
        }

        // Warns the user if they haven't added any targets to this system.
        private TriValidationResult WarnIfNoTargets()
        {
            if (HasNoTargets)
            {
                return TriValidationResult.Warning("There are no targets assigned! Nothing will be rotated.");
            }

            return TriValidationResult.Valid;
        }
    }
}
