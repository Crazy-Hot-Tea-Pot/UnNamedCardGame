using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlayerSoundFromAnimation : MonoBehaviour
{
    public List<SoundFX> ListOfFootSteps = new List<SoundFX>();

    public PlayerController PlayerController;

    public void PlayFootStepSound()
    {

        int randomIndex = Random.Range(0,ListOfFootSteps.Count);

        SoundManager.PlayFXSound(ListOfFootSteps[randomIndex]);

        //If help is less than 35% play another audio
        if(PlayerController.Health <= PlayerController.MaxHealth * 0.35f)
        {
            SoundManager.PlayFXSound(SoundFX.EnemyDefeated);
        }
    }
    public void TakeDamage()
    {

    }
}
