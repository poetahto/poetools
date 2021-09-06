using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundedCollisionListener : GroundCheck
{
    public override bool IsGrounded => _touchingColliders.Count > 0;
    public override Vector3 ContactNormal => _contactNormal;
    public override Collider ConnectedCollider => IsGrounded ? _touchingColliders.First.Value : null;

    private LinkedList<Collider> _touchingColliders = new LinkedList<Collider>();
    private Vector3 _contactNormal;

    private void OnCollisionEnter(Collision other)
    {
        Vector3 contactNormal = other.GetContact(0).normal;

        if (Vector3.Angle(-gravityDirection, contactNormal) <= slopeLimitDegrees)
            AddNewContact(other);
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (_touchingColliders.Contains(other.collider))
            RemoveContact(other);
    }

    private void AddNewContact(Collision other)
    {
        _touchingColliders.AddLast(other.collider);
        _contactNormal = other.GetContact(0).normal;
    }

    private void RemoveContact(Collision other)
    {
        _touchingColliders.Remove(other.collider);
        _contactNormal = gravityDirection;
    }
}