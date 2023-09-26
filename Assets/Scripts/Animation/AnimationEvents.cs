using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void OnFootstep(AnimationEvent animationEventData)
    {
        if (animationEventData.animatorClipInfo.weight > 0.5f)
        {
            if (!string.IsNullOrEmpty(animationEventData.stringParameter))
            {
                if (Enum.TryParse(typeof(AnimationEventName), animationEventData.stringParameter, false,
                        out object animationEventName))
                {
                    switch ((AnimationEventName)animationEventName)
                    {
                        case AnimationEventName.OnFootStep:
                            
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            // animationEventData.stringParameter;
            // if (FootstepAudioClips.Length > 0)
            // {
            //     var index = Random.Range(0, FootstepAudioClips.Length);
            //     AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            // }
        }
    }

    public void OnLand(AnimationEvent animationEventData)
    {
         //if (animationEventData.animatorClipInfo.weight > 0.5f)
        // {
        //     AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center),
        //         FootstepAudioVolume);
        // }
    }
}
