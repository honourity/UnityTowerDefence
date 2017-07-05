using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioManager : MonoBehaviour {

	public static AudioManager Instance
	{
		get { return _instance = _instance ?? FindObjectOfType<AudioManager>() ?? new AudioManager { }; }
	}
	private static AudioManager _instance;

	public enum AudioClipNames
	{
		None,
		AmbientWindAtmosphere,
		MusicCamelHorizon,
		SFXCropTaken,
		SFXDeath1,
		SFXDeath2,
		SFXGallop,
		SFXGunshot,
		SFXHeartbeat,
		SFXHit,
		SFXNewCrop,
	}

	public AudioClip[] AudioClips;
	public AudioClipNames NextMusicClip;
	public AudioClipNames NextAmbientClip;

	[Tooltip("The maximum number of sound effects to play at the same time")]
	public int MaxSoundEffects = 32;
	[Range(0, 1)]
	public float SoundEffectVolume = 0.5f;
	private AudioSource _musicAudioSource;
	private AudioSource _ambientAudioSource;

	private List<AudioSource> _soundEffectsPool;

	public void PlayClip(AudioClipNames clipEnum)
	{
		var freeAudioSource = _soundEffectsPool.FirstOrDefault(a => a.isPlaying == false);
		if (freeAudioSource == null && _soundEffectsPool.Count < MaxSoundEffects)
		{
			freeAudioSource = gameObject.AddComponent<AudioSource>();

			freeAudioSource.volume = SoundEffectVolume;
			_soundEffectsPool.Add(freeAudioSource);
		}

		if (freeAudioSource != null) freeAudioSource.PlayOneShot(AudioClips.FirstOrDefault(clip => clip.name == clipEnum.ToString()));
	}

	private void Awake()
	{
		_soundEffectsPool = new List<AudioSource>();

		_musicAudioSource = gameObject.AddComponent<AudioSource>();
		_musicAudioSource.priority = 10;
		_musicAudioSource.playOnAwake = false;

		_ambientAudioSource = gameObject.AddComponent<AudioSource>();
		_ambientAudioSource.priority = 20;
		_ambientAudioSource.loop = true;
		_ambientAudioSource.playOnAwake = false;
	}

	private void Update()
	{
		if (NextMusicClip != AudioClipNames.None)
		{
			if (!_musicAudioSource.isPlaying)
			{
				var clip = AudioClips.FirstOrDefault(c => c.name == NextMusicClip.ToString());
				_musicAudioSource.clip = clip;
				_musicAudioSource.PlayOneShot(clip);
				NextMusicClip = AudioClipNames.None;
			}
		}

		if (NextAmbientClip != AudioClipNames.None)
		{
			//todo - replace this with a crossfade to new clip
			if (!_ambientAudioSource.isPlaying)
			{
				var clip = AudioClips.FirstOrDefault(c => c.name == NextAmbientClip.ToString());
				_ambientAudioSource.clip = clip;
				_ambientAudioSource.time = GameManager.Random.Next(0, Mathf.FloorToInt(clip.length));
				_ambientAudioSource.Play();
				NextAmbientClip = AudioClipNames.None;
			}
		}
	}
}
