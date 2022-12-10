using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;

namespace MineBeat.SongSelectSingle.Extern
{
	/// <summary>
	/// 선택된 곡의 정보를 담습니다.
	/// </summary>
	public class SelectedSongInfo : Singleton<SelectedSongInfo>
	{
		[SerializeField, ReadOnly]
		private ulong _id;
		public ulong id
		{
			get { return _id; }
			set { if (SceneManager.GetActiveScene().name == "SongSelectSingleScene") _id = value; }
		}

		[SerializeField, ReadOnly]
		private uint _score;
		public uint score
		{
			get { return _score; }
			set { if (SceneManager.GetActiveScene().name == "InGameSingleScene") _score = value; }
		}

		[SerializeField, ReadOnly]
		private uint _combo;
		public uint combo
		{
			get { return _combo; }
			set { if (SceneManager.GetActiveScene().name == "InGameSingleScene") _combo = value; }
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
