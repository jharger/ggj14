using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager> {
	public int numAudioSources = 32;
	public AudioSource audioSourcePrefab;

	private Queue<AudioSource> sourceQueue;

	void Awake () {
		GameObject.DontDestroyOnLoad(gameObject);

		sourceQueue = new Queue<AudioSource>(numAudioSources);
		for(int i = 0; i < numAudioSources; i ++) {
			AudioSource source = (AudioSource)Instantiate(audioSourcePrefab);
			source.transform.parent = transform;
			sourceQueue.Enqueue(source);
		}
	}

	public void PlayAudioClip(AudioClip clip)
	{
		AudioSource source = sourceQueue.Dequeue();
		source.PlayOneShot(clip);
		sourceQueue.Enqueue(source);
	}
}
