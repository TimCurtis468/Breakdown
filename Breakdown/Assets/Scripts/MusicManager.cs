using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public int currentTrack;
    public List<AudioClip> tracks = new List<AudioClip>();
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTrack = 0;
        if( tracks.Count > 0)
        {
            audioSource.clip = tracks[currentTrack];
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            currentTrack++;
            if(currentTrack > tracks.Count)
            {
                currentTrack = 0;
            }

            if (tracks.Count > 0)
            {
                audioSource.clip = tracks[currentTrack];
                audioSource.Play();
            }
        }
    }
}
