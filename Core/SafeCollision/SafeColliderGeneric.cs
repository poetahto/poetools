﻿using UnityEngine;

namespace poetools.Core.SafeCollision
{
    public abstract class SafeColliderGeneric<T> : SafeCollider where T : Collider
    {
        protected override Collider GetCollider()
        {
            return GetComponent<T>();
        }

        protected override Trigger AddTrigger(GameObject target)
        {
            var originalCollider = GetComponent<T>();
            var targetCollider = target.AddComponent<T>();
            targetCollider.isTrigger = true;
            
            CopyTo(targetCollider, originalCollider);

            return target.AddComponent<Trigger>();
        }
        
        protected abstract void CopyTo(T target, T original);
    }
}