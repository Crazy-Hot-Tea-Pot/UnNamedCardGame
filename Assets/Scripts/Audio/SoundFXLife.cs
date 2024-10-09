using System.Collections;
using UnityEngine;

/// <summary>
/// Plays the audio for the life given.
/// </summary>
public class SoundFXLife : MonoBehaviour
{
    private float _soundLength;

    /// <summary>
    /// How long the audio should play.
    /// </summary>
    public float SoundLength
    {
        get
        {
            return _soundLength;
        }
        set
        {
            _soundLength = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteMe(SoundLength));
    }
    IEnumerator DeleteMe(float length)
    {
        yield return new WaitForSeconds(length);
        Destroy(this.gameObject);
    }

}