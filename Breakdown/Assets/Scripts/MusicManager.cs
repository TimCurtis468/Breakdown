using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Singleton
    private static MusicManager _instance;
    public static MusicManager Instance => _instance;


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

    public int currentTrack;
    public List<AudioClip> tracks = new List<AudioClip>();
    private AudioSource audioSource;

    bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTrack = 0;
        if (tracks.Count > 0)
        {
            audioSource.clip = tracks[currentTrack];
            audioSource.Play();
            isPlaying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((!audioSource.isPlaying) && (isPlaying == true))
        {
            currentTrack++;
            if (currentTrack > tracks.Count)
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

    public void StopMusic()
    {
        audioSource.Stop();
        isPlaying = false;
    }

    public void StartMusic()
    {
        isPlaying = true;
    }
}
