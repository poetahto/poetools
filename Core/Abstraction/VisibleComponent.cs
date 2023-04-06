using System;
using UnityEngine;

namespace poetools.Core.Abstraction
{
    public interface IVisibleComponent
    {
        void SetVisible(bool isVisible);
        bool IsVisible();
        
        event Action<bool, bool> OnVisibilityChanged;
    }
    
    public abstract class VisibleComponent : MonoBehaviour, IVisibleComponent
    {
        public abstract void SetVisible(bool isVisible);
        public abstract bool IsVisible();

        public abstract event Action<bool, bool> OnVisibilityChanged;
    }

    public static class VisibleComponentExtensions
    {
        public static void Toggle(this IVisibleComponent component)
        {
            bool currentState = component.IsVisible();
            component.SetVisible(!currentState);
        }

        public static void Show(this IVisibleComponent component)
        {
            component.SetVisible(true);
        }

        public static void Hide(this IVisibleComponent component)
        {
            component.SetVisible(false);
        }
    }
}