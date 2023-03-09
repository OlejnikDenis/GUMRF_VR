using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Changes the value in Animator at the click of a button 
/// </summary>
public class AnimateHandPrefab : MonoBehaviour
{
    public InputActionProperty pinchAnimAction;
    public InputActionProperty gripAnimaAction;

    public Animator handAnimation;

    void Update()
    {
        float triggerValue = pinchAnimAction.action.ReadValue<float>();
        float gripValue = gripAnimaAction.action.ReadValue<float>();

        handAnimation.SetFloat("Trigger", triggerValue);
        handAnimation.SetFloat("Grip", gripValue);
    }
}