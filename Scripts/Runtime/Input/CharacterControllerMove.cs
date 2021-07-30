using UnityEngine;

namespace Input
{
    public class CharacterControllerMove : InputMoveBase
    {
        [Header("Character Controller Settings")]
        [SerializeField] private CharacterController controller;

        protected override void Update()
        {
            base.Update();
            
            controller.Move(CurrentVelocity * Time.deltaTime);
            
            Vector3 resultantVelocity = controller.velocity;
            resultantVelocity.y = CurrentVelocity.y;
            CurrentVelocity = resultantVelocity;
        }
    }
}