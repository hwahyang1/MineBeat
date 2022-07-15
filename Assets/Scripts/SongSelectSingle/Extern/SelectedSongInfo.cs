using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * [Namespace] MineBeat.SongSelectSingle.Extern
 * Description
 */
namespace MineBeat.SongSelectSingle.Extern
{
	/*
	 * [Class] SelectedSongInfo
	 * 선택된 곡의 정보를 담습니다.
	 */
	public class SelectedSongInfo : Singleton<SelectedSongInfo>
	{
		private SongInfo _songInfo;
		public SongInfo songInfo
		{
			get { return _songInfo; }
			set { if (SceneManager.GetActiveScene().name == "SongSelectSingleScene") _songInfo = value; }
		}

		private AudioClip _audioClip;
		public AudioClip audioClip
		{
			get { return _audioClip; }
			set { if (SceneManager.GetActiveScene().name == "SongSelectSingleScene") _audioClip = value; }
		}

		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		private void Update()
		{
			if (SceneManager.GetActiveScene().name != "SongSelectSingleScene" &&
				SceneManager.GetActiveScene().name != "InGameSingleScene" &&
				SceneManager.GetActiveScene().name != "ResultSingleScene")
				Destroy(gameObject);
		}
	}
}
