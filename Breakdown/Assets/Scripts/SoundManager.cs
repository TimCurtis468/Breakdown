using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	// Audio players components.
	public AudioSource EffectsSource;

	public List<AudioClip> soundFx = new List<AudioClip>();
	public List<AudioClip> LoFx = new List<AudioClip>();
	public List<AudioClip> Heart = new List<AudioClip>();

	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Singleton instance.
	public static SoundManager Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (Instance == null)
		{
			Instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(transform.root.gameObject);
	}

	// Play a single clip through the sound effects source.
	public void PlayHeart()
	{
		EffectsSource.PlayOneShot(Heart[0]);
	}


	// Play a random clip from an array, and randomize the pitch slightly.
	public void PlaySoundFx(bool isLoFx)
	{
		int randomIndex = Random.Range(0, soundFx.Count);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		EffectsSource.pitch = randomPitch;
		if (isLoFx == true)
		{
			EffectsSource.PlayOneShot(LoFx[randomIndex]);
		}
		else
		{
			EffectsSource.PlayOneShot(soundFx[randomIndex]);
		}
	}
}
