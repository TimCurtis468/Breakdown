using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{

    #region Singleton
    private static SoundFxManager _instance;
    public static SoundFxManager Instance => _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public AudioSource audioSource;

    public List<AudioClip> soundFx = new List<AudioClip>();
    public List<AudioClip> LoFx = new List<AudioClip>();
    public AudioClip Heart;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Play a single clip through the sound effects source.
    public void PlayHeart()
    {
        audioSource.PlayOneShot(Heart);
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void PlaySoundFx(bool isLoFx)
    {
        int randomIndex = Random.Range(0, soundFx.Count);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        audioSource.pitch = randomPitch;
        if (isLoFx == true)
        {
            audioSource.PlayOneShot(LoFx[randomIndex]);
        }
        else
        {
            audioSource.PlayOneShot(soundFx[randomIndex]);
        }
    }
}
