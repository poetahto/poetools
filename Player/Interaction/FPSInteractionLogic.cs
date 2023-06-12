using System;
using poetools.Core.Tools;
using UnityEngine;

namespace poetools.player.Player.Interaction
{
    public struct InteractionData
    {
        public GameObject Interactable;
        public GameObject Sender;
    }

    public class FPSInteractionLogic
    {
        private readonly InteractionSettings _settings;
        private IInteractable[] _facingInteractables = Array.Empty<IInteractable>();
        private IInteractable[] _heldInteractables = Array.Empty<IInteractable>();
        private Ray _viewRay;

        public FPSInteractionLogic(InteractionSettings settings)
        {
            _settings = settings;
        }

        public Ray ViewRay
        {
            get => _viewRay;
            set
            {
                if (!_viewRay.Equals(value))
                {
                    _viewRay = value;
                    Tick();
                }
            }
        }

        public bool HasFacingObject { get; private set; }
        public GameObject TargetObject { get; private set; }

        public event Action<InteractionData> Interacted;
        public event Action<GameObject> FaceObjectStarted;
        public event Action FaceObjectEnded;

        public void Tick()
        {
            if (RaycastTools.Raycast3D(ViewRay, out var hit, _settings.range, ~LayerMask.GetMask("Ignore Raycast"),
                    QueryTriggerInteraction.Collide, 0.0f))
            {
                _facingInteractables = hit.transform.GetComponents<IInteractable>();

                if (!HasFacingObject && _facingInteractables.Length > 0)
                {
                    FaceObjectStarted?.Invoke(hit.transform.gameObject);
                }
                else if (HasFacingObject && _facingInteractables.Length <= 0)
                {
                    FaceObjectEnded?.Invoke();
                }
                else if (HasFacingObject && TargetObject != hit.transform.gameObject)
                {
                    FaceObjectEnded?.Invoke();
                    FaceObjectStarted?.Invoke(hit.transform.gameObject);
                }

                TargetObject = hit.transform.gameObject;
                HasFacingObject = _facingInteractables.Length > 0;
            }
            else if (HasFacingObject)
            {
                FaceObjectEnded?.Invoke();
                HasFacingObject = false;
                _facingInteractables = Array.Empty<IInteractable>();
            }
            else
            {
                _facingInteractables = Array.Empty<IInteractable>();
            }
        }

        public void Interact(GameObject sender)
        {
            if (HasFacingObject)
            {
                Interacted?.Invoke(new InteractionData{Sender = sender, Interactable = TargetObject});
                _heldInteractables = _facingInteractables;

                foreach (var interactable in _heldInteractables)
                    interactable.HandleInteractStart(sender);
            }
        }

        public void StopInteracting(GameObject sender)
        {
            foreach (var interactable in _heldInteractables)
                interactable.HandleInteractStop(sender);

            _heldInteractables = Array.Empty<IInteractable>();
        }
    }
}
