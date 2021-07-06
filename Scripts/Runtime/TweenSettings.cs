#if DOTWEEN_ENABLED
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[Serializable]
public abstract class TweenSettings<T1, T2, TPlugOptions> where TPlugOptions : struct, IPlugOptions
{
    // ---- Editor Variables ----

    public int id;
    public GameObject link;
    public T2 startValue;
    public T2 endValue;
    public float duration = 1f;
    public float delay;
    public Ease ease = Ease.Linear;
    public LoopSettings loop;
    public bool speedBased = true;
    public bool timescaleIndependent;
    public bool relativeEnd;
    public UpdateType updateType;
        
    // ---- Public Methods ----
        
    public void ApplyTo(TweenerCore<T1, T2, TPlugOptions> tween)
    {
        tween.id = id;
        tween.SetLink(link);
        tween.ChangeStartValue(startValue);
        tween.ChangeEndValue(endValue);
        tween.SetDelay(delay);
        tween.SetEase(ease);
        tween.SetLoops(loop.amount, loop.type);
        tween.SetSpeedBased(speedBased);
        tween.SetRelative(relativeEnd);
        tween.SetUpdate(updateType, timescaleIndependent);
    }
}

[Serializable]
public class LoopSettings
{
    public int amount;
    public LoopType type = LoopType.Yoyo;
}

[Serializable]
public class FloatTweenSettings : TweenSettings<float, float, FloatOptions> { }

#endif