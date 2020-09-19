using UnityEngine;

[System.Serializable]
public class SoundEntity
{
	public string soundName;

	[Range(0f, 1f)]
	public float volume = 1f;

	public AudioClip audioClip;

	[System.NonSerialized]
	public AudioSource audioSource;

	public bool isSoundEffect = true;
}
