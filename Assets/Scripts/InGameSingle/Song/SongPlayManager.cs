using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;

/*
 * [Namespace] MineBeat.InGameSingle.Song
 * Description
 */
namespace MineBeat.InGameSingle.Song
{
	/*
	 * [Class] SongPlayManager
	 * 곡의 재생을 관리합니다.
	 */
	public class SongPlayManager : MonoBehaviour
	{
		private AudioSource audioSource;

		private bool isStarted = false;

		public bool isPlaying
		{
			get { return isStarted && audioSource.time == 0f && !audioSource.isPlaying; }
		}

		public float timecode
		{
			get { return audioSource.time; }
		}

		private ulong id;

		private void Awake()
		{
			id = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>().id;

			audioSource = gameObject.GetComponent<AudioSource>();

			audioSource.clip = PackageManager.Instance.GetMedias(id).Item2;
		}

		private void Start()
		{
			StartCoroutine("DelayedStart");
		}

		public IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(1.5f);
			audioSource.Play();
			isStarted = true;
		}
	}
}
