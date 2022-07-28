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
		private AudioSource backgroundSound;
		//private AudioSource effectSound;

		private bool isStarted = false;

		public bool isPlaying
		{
			get { return !(isStarted && backgroundSound.time == 0f && !backgroundSound.isPlaying); }
		}

		public float timecode
		{
			get { return backgroundSound.time; }
		}

		private ulong id;

		private void Awake()
		{
			id = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>().id;

			List<GameObject> audioSources = new List<GameObject>(GameObject.FindGameObjectsWithTag("AudioSource"));
			backgroundSound = audioSources.Find(target => target.name == "BackgroundSound").GetComponent<AudioSource>();
			//effectSound = audioSources.Find(target => target.name == "EffectSound").GetComponent<AudioSource>();

			backgroundSound.clip = PackageManager.Instance.GetMedias(id).Item2;
		}

		private void Start()
		{
			StartCoroutine("DelayedStart");
		}

		public IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(1.5f);
			backgroundSound.Play();
			isStarted = true;
		}

		private void OnDestroy()
		{
			backgroundSound.time = 0f;
			backgroundSound.Stop();
			backgroundSound.clip = null;
		}
	}
}
