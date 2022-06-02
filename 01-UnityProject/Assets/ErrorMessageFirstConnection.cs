using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessageFirstConnection : MonoBehaviour
{
    /// <summary>
    /// A ref to the animator of ErrorText
    /// </summary>
    public Animator errorTextAnimator;

    /// <summary>
    /// Use for restart animation, even if animation is already playing
    /// </summary>
    public void RestartAnim()
    {
        errorTextAnimator.Play("FadeOut", -1, 0f);
    }

    /// <summary>
    /// Triggered by animation in order to desactivate GameObject
    /// </summary>
    public void EndOfAnimation()
    {
        gameObject.SetActive(false);
    }
}
