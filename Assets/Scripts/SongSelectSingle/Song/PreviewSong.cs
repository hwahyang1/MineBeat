using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.SongSelectSingle.Song
{
	/// <summary>
	/// 곡의 미리듣기를 재생합니다. (DefineNote.NoteType에서, PreviewS/PreviewE 사이의 구간)
	/// </summary>
	public class PreviewSong : MonoBehaviour
	{
		private float maxVolume;

		float[] timecodes = new float[2];
		private AudioClip audioClip = null;
		private AudioSource backgroundSound;
		//private AudioSource effectSound;

		[HideInInspector]
		public bool forceFadeout = false;
		private bool reloadRequired = true;

		private void Start()
		{
			List<GameObject> audioSources = new List<GameObject>(GameObject.FindGameObjectsWithTag("AudioSource"));
			backgroundSound = audioSources.Find(target => target.name == "BackgroundSound").GetComponent<AudioSource>();
			//effectSound = audioSources.Find(target => target.name == "EffectSound").GetComponent<AudioSource>();

			maxVolume = backgroundSound.volume;
		}

		private void Update()
		{
			if (reloadRequired)
			{
				backgroundSound.Stop();
				backgroundSound.clip = audioClip;
				backgroundSound.time = timecodes[0];
				backgroundSound.volume = 0f;
				backgroundSound.Play();
				reloadRequired = false;
			}

			if (forceFadeout)
			{
				backgroundSound.volume = Mathf.Lerp(backgroundSound.volume, 0f, 3f * Time.deltaTime);
			}
			else
			{
				if (backgroundSound.time <= timecodes[0] + 0.75f)
				{
					backgroundSound.volume = Mathf.Lerp(backgroundSound.volume, maxVolume, 2.75f * Time.deltaTime);
				}
				else if (backgroundSound.time >= timecodes[1] - 1f)
				{
					backgroundSound.volume = Mathf.Lerp(backgroundSound.volume, 0f, 3f * Time.deltaTime);
				}
				else
				{
					backgroundSound.volume = maxVolume;
				}

				if (backgroundSound.time > timecodes[1])
				{
					backgroundSound.time = timecodes[0];
				}
			}
		}

		/// <summary>
		/// 곡을 재생합니다.
		/// </summary>
		/// <param name="audioClip">재생할 곡을 재생합니다.</param>
		/// <param name="timecodes">재생할 구간을 입력합니다.</param>
		public void Play(AudioClip audioClip, float[] timecodes)
		{
			this.audioClip = audioClip;
			this.timecodes = timecodes;

			reloadRequired = true;
		}

		private void OnDestroy()
		{
			backgroundSound.time = 0f;
			backgroundSound.Stop();
			backgroundSound.volume = maxVolume;
			backgroundSound.clip = null;
		}
	}
}
