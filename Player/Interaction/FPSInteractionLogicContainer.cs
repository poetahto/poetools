using System;
using System.Linq;
using JetBrains.Annotations;
using poetools.Core;
using poetools.player.Player.Rotation;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace poetools.player.Player.Interaction
{
    /// <summary>
    /// A MonoBehaviour wrapper for an instance of <see cref="FPSInteractionLogic"/>.
    /// Use this to easily define settings and targets in the inspector, and find it in
    /// GetComponent queries.
    /// </summary>
    [HideMonoScript]
    [DeclareTabGroup("tabs")]
    public class FPSInteractionLogicContainer : MonoBehaviour, IInputProvider
    {
        [Group("tabs"), Tab("Settings")]
        [Required]
        [HideLabel]
        [InlineEditor]
        [SerializeField]
        [ScriptableAssetReference]
        private InteractionSettingsAsset settingsAsset;

        [Group("tabs"), Tab("Settings")]
        [PropertyTooltip("Should extra debugging information be drawn to the screen.")]
        [SerializeField]
        private bool showDebug;

        [Group("tabs"), Tab("References")]
        [PropertyTooltip("The direction this object is facing - anything close enough in the forward direction will be interacted with.")]
        [SerializeField]
        public Transform viewDirection;

        [Group("tabs"), Tab("Input")]
        [PropertyTooltip("Should this container automatically supply inputs to the underlying logic.")]
        [SerializeField]
        private bool automaticallyProvideInput = true;

        [Group("tabs"), Tab("Input")]
        [ShowIf(nameof(automaticallyProvideInput))]
        [PropertyTooltip("The keys that should be pressed to make this object interact with another.")]
        [SerializeField]
        private KeyCode[] interactionKeys = {KeyCode.E, KeyCode.Mouse0};

        [Group("tabs"), Tab("Events")]
        [PropertyTooltip("Called when this object interacts with another.")]
        [SerializeField]
        public UnityEvent<InteractionData> interacted;

        [Group("tabs"), Tab("Events")]
        [PropertyTooltip("Called when this object starts facing an interactable object.")]
        [SerializeField]
        public UnityEvent<GameObject> faceObjectStarted;

        [Group("tabs"), Tab("Events")]
        [PropertyTooltip("Called when this object stops facing an interactable object.")]
        [SerializeField]
        public UnityEvent faceObjectEnded;

        [SerializeField] private GameObject grabber;

        private FPSInteractionLogic _interactionLogic;

        /// <summary>
        /// Gets the interaction logic that is setup and controlled by this container.
        /// </summary>
        public FPSInteractionLogic InteractionLogic
        {
            get
            {
                // Lazy initialization, also to simply order-of-initialization for users.
                if (_interactionLogic == null)
                {
                    _interactionLogic = new FPSInteractionLogic(settingsAsset.Generate());
                    _interactionLogic.Interacted += interacted.Invoke;
                    _interactionLogic.FaceObjectStarted += faceObjectStarted.Invoke;
                    _interactionLogic.FaceObjectEnded += faceObjectEnded.Invoke;
                }

                return _interactionLogic;
            }
        }

        public bool Active { get; set; } = true;

        private bool HasMissingReferences => viewDirection == null || settingsAsset == null;

        public bool PollWantsToInteract()
        {
            // foreach (var keyCode in interactionKeys)
            // {
            //     if (Input.GetKeyDown(keyCode))
            //         return true;
            // }
            //
            // return false;
            //

            bool released = false;

            foreach (var keyCode in interactionKeys)
            {
                if (Input.GetKeyDown(keyCode))
                    released = true;

                else if (Input.GetKey(keyCode))
                    return false;
            }

            return released;
        }

        public bool PollWantsToStopInteracting()
        {
            bool released = false;

            foreach (var keyCode in interactionKeys)
            {
                if (Input.GetKey(keyCode))
                    return false;

                if (Input.GetKeyUp(keyCode))
                    released = true;
            }

            return released;
        }

        [Button]
        [UsedImplicitly]
        [GUIColor(1, 51/255f, 51/255f)]
        [ShowIf(nameof(HasMissingReferences))]
        private void TryFixReferences()
        {
            if (viewDirection == null)
            {
                viewDirection = GetComponentsInChildren<Transform>()
                    .FirstOrDefault(target => target.name.Equals("pitch", StringComparison.InvariantCultureIgnoreCase));

                if (viewDirection == null)
                {
                    var rotationLogic = GetComponentInChildren<FPSRotationLogicContainer>();

                    if (rotationLogic != null && rotationLogic.PitchTransforms.Count > 0)
                        viewDirection = rotationLogic.PitchTransforms[0];

                    else
                    {
                        var cam = GetComponentInChildren<Camera>();
                        viewDirection = cam != null ? cam.transform : transform;
                    }
                }
            }

            if (settingsAsset == null)
                settingsAsset = ScriptableObject.CreateInstance<InteractionSettingsAsset>();
        }

        private void Update()
        {
            if (automaticallyProvideInput && Active)
            {
                if (PollWantsToInteract())
                    InteractionLogic.Interact(grabber);

                else if (PollWantsToStopInteracting())
                    InteractionLogic.StopInteracting(grabber);

                InteractionLogic.ViewRay = new Ray(viewDirection.position, viewDirection.forward);
            }

            InteractionLogic.Tick();
        }

        private void OnGUI()
        {
            if (!showDebug)
                return;

            GUILayout.Label($"Has Facing Object: {InteractionLogic.HasFacingObject}");

            if (InteractionLogic.HasFacingObject)
                GUILayout.Label($"Object: {InteractionLogic.TargetObject.name}");
        }
    }
}
