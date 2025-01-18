using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlayerSoundFromAnimation : MonoBehaviour
{

    public void PlayFootStepSound()
    {
        SoundManager.PlayFXSound(SoundFX.Footstep);
    }
    //public void PlayRunningSound()
    //{
    //    SoundManager.PlayFXSound(SoundFX.Running);
    //}
}
