using UnityEngine;

public class SprintAbility : MonoBehaviour
{
    [SerializeField] private Vector3 sprintMultiplier = new Vector3(1, 1.3f, 1);
    [SerializeField] private MovementBase movementSystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            movementSystem.MoveDirectionMultiplier += sprintMultiplier - Vector3.one;
        
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            movementSystem.MoveDirectionMultiplier -= sprintMultiplier - Vector3.one;
    }
}