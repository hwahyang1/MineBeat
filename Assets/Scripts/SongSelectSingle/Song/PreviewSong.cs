using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.SongSelectSingle.Song
 * Description
 */
namespace MineBeat.SongSelectSingle.Song
{
	/*
	 * [Class] PreviewSong
	 * 곡의 미리듣기를 재생합니다. (DefineNote.NoteType에서, PreviewS/PreviewE 사이의 구간)
	 */
	public class PreviewSong : MonoBehaviour
	{
		float[] timecodes = new float[2];
		private AudioClip audioClip = null;
		private AudioSource audioSource;

		private bool reloadRequired = true;

		private void Start()
		{
			audioSource = gameObject.GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (reloadRequired)
			{
				audioSource.Stop();
				audioSource.clip = audioClip;
				audioSource.time = timecodes[0];
				audioSource.Play();
				reloadRequired = false;
			}

			if (audioSource.time > timecodes[1])
			{
				audioSource.time = timecodes[0];
			}
		}

		/*
		 * [Method] Play(AudioClip audioClip, float[] timecodes): void
		 * 곡을 재생합니다.
		 * 
		 * <AudioClip audioClip>
		 * 재생할 곡을 재생합니다.
		 * 
		 * <float[] timecodes>
		 * 재생할 구간을 입력합니다.
		 */
		public void Play(AudioClip audioClip, float[] timecodes)
		{
			this.audioClip = audioClip;
			this.timecodes = timecodes;

			reloadRequired = true;
		}
	}
}
