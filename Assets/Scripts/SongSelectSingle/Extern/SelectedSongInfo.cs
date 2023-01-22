using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;
using UnityEngine.Serialization;

namespace MineBeat.SongSelectSingle.Extern
{
	/// <summary>
	/// 선택된 곡의 정보를 담습니다.
	/// </summary>
	public class SelectedSongInfo : Singleton<SelectedSongInfo>
	{
		[SerializeField, ReadOnly]
		private ulong id;
		public ulong ID
		{
			get { return id; }
			set { if (SceneManager.GetActiveScene().name == "SongSelectSingleScene") id = value; }
		}

		[SerializeField, ReadOnly]
		private uint score;
		public uint Score
		{
			get { return score; }
			set { if (SceneManager.GetActiveScene().name == "InGameSingleScene") score = value; }
		}

		[SerializeField, ReadOnly]
		private uint combo;
		public uint Combo
		{
			get { return combo; }
			set { if (SceneManager.GetActiveScene().name == "InGameSingleScene") combo = value; }
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
