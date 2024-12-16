using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FadeEvent_SO", menuName = "Event/FadeEvent")]
public class FadeEvent_SO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;

    /// <summary>
    /// 变黑
    /// </summary>
    /// <param name="dutation"></param>
    public void FadeIn(float dutation)
    {
        RaiseEvent(Color.black, dutation, true);
    }
    public void FadeOut(float dutation)
    {
        RaiseEvent(Color.clear, dutation, false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    /// <param name="fadeIn">true 变黑 false 变透明</param>
    public void RaiseEvent(Color color,float duration,bool fadeIn)
    {
        OnEventRaised?.Invoke(color,duration,fadeIn);
    }
}
