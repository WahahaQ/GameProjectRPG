using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public static SoundController soundController;
	public List<SoundEntity> sounds = new List<SoundEntity>();

	private void Awake()
	{
		if (soundController == null)
		{
			soundController = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach (SoundEntity sound in sounds)
		{
			sound.audioSource = gameObject.AddComponent<AudioSource>();
			sound.audioSource.clip = sound.audioClip;
		}
	}

	private void Start()
	{
		Play(GameConstants.MAIN_THEME_2);
	}

	public SoundEntity GetSoundByName(string soundName)
	{
		return sounds.Where(x => x.soundName == soundName).FirstOrDefault();
	}

	public void Play(string soundName, bool isSoundEffect = true)
	{
		SoundEntity soundEntity = GetSoundByName(soundName);

		if (soundEntity == null)
		{
			return;
		}

		if (!isSoundEffect)
		{
			foreach (SoundEntity sound in sounds)
			{
				if (!sound.isSoundEffect)
				{
					sound.audioSource.Stop();
				}
			}
		}

		AudioSource audioSource = soundEntity.audioSource;
		audioSource.volume = soundEntity.volume;
		audioSource.Play();
	}
}
