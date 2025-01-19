using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlayerSoundFromAnimation : MonoBehaviour
{
    public List<SoundFX> ListOfFootSteps = new List<SoundFX>();

    public void PlayFootStepSound()
    {

        int randomIndex = Random.Range(0,ListOfFootSteps.Count);

        SoundManager.PlayFXSound(ListOfFootSteps[randomIndex]);
    }
    //public void PlayRunningSound()
    //{
    //    SoundManager.PlayFXSound(SoundFX.Running);
    //}
}
