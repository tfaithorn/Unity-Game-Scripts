using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Character character;
    Animator animator;

    public void Awake()
    {
        character = GetComponent<Character>();
        animator = character.animator;
    }

    public enum States {
        DEFAULT = 0,
        PLAY = 1,
        CLEAR = 2
    }

    public enum ActionType { 
        IDLE = 1,
        RUN = 2,
        ABILITY = 3,
        FALLING = 4,
        WALK = 5
    }

    public enum Phase { 
        PHASE_1 = 1,
        PHASE_2 = 2
    }

    public enum Layer { 
        FULL = 0,
        HANDS = 1,
        LEGS = 2,
        HEAD = 3
    }

    public void AnimationActionRequest(List<AnimationVariable> variables, float duration = -1, Layer animationLayer = Layer.FULL)
    {
        //enable animation layers
        switch (animationLayer) {
            case Layer.FULL:
                animator.SetLayerWeight((int)Layer.FULL, 1);
                animator.SetLayerWeight((int)Layer.HANDS, 0);
                animator.SetLayerWeight((int)Layer.LEGS, 0);
                break;
            case Layer.HANDS:
                animator.SetLayerWeight((int)Layer.HANDS, 1);
                break;
            case Layer.LEGS:
                animator.SetLayerWeight((int)Layer.LEGS, 1);
                break;
        }

        foreach (AnimationVariable variable in variables)
        {

            if (variable.hasBool)
            {
                animator.SetBool(variable.name, variable.boolValue);
            }

            if (variable.hasFloat)
            {
                animator.SetFloat(variable.name, variable.floatValue);
            }

            if (variable.hasInt)
            {
                animator.SetInteger(variable.name, variable.intValue);
            }
        }

        if (duration != -1)
        { 
        
        }

    }

    //needs fixing. I got it working perfectly once but can't remember how :(
    IEnumerator UpdateAnimationSpeed(float duration)
    {
        //wait for the animation to be on the player
        yield return new WaitForFixedUpdate();
        float currActionSpeed = animator.GetFloat("Action Speed");
        float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;

        AnimatorClipInfo[] clipinfo = animator.GetCurrentAnimatorClipInfo(0);

        float clipInfoTotal = 0f;
        foreach (AnimatorClipInfo clip in clipinfo)
        {
            clipInfoTotal += clip.clip.length;
        }

        AnimatorClipInfo[] nextClipInfos = animator.GetNextAnimatorClipInfo(0);
        float nextClipLength = 0f;
        foreach (AnimatorClipInfo nextClip in nextClipInfos)
        {
            nextClipLength += nextClip.clip.length;
        }

        float desiredActionSpeed = 0f;

        if (nextClipLength > 0f)
        {
            desiredActionSpeed = ((nextClipLength)) / (duration);
        }
        else
        {
            desiredActionSpeed = ((clipInfoTotal)) / (duration);
        }


        animator.SetFloat("Action Speed", desiredActionSpeed);
    }

    public void ClearAnimationState()
    {
        animator.SetInteger("Action",0);
    }

    public void SetWeaponClass(ItemWeapon.WeaponClass weaponClass)
    {
        animator.SetInteger("Weapon Class", (int)weaponClass);
    }
}
