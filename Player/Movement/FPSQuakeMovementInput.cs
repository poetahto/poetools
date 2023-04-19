using System;
using UnityEngine;

namespace poetools.player.Player.Movement
{
    public class FPSQuakeMovementInput : MonoBehaviour, IInputProvider
    {
        public FPSQuakeMovementLogicContainer container;

        public bool Active { get; set; } = true;

        private void Update()
        {
            if (Active)
            {
                var direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                container.QuakeMovementLogic.TargetDirection = direction;
            }
        }

        private void OnDisable()
        {
            container.QuakeMovementLogic.TargetDirection = Vector3.zero;
        }
    }
}
