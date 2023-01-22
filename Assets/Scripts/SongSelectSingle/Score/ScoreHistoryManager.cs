using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;
using UnityEngine.Serialization;

namespace MineBeat.SongSelectSingle.Score
{
	/// <summary>
	/// 이전 플레이 기록을 저장 및 관리합니다.
	/// </summary>
	public class ScoreHistoryManager : Singleton<ScoreHistoryManager>
	{
		// 플레이 기록은 rootPath에 ID.dat 파일로 저장합니다.
		[SerializeField, ReadOnly]
		private string rootPath;
		private string RootPath
		{
			get { return rootPath; }
			set { if (!protectModify) rootPath = value; }
		}
		private bool protectModify = false;

		private BinaryFormatter formatter = new BinaryFormatter();
		private List<System.Tuple<ulong, FileStream, PlayHistory>> history = new List<System.Tuple<ulong, FileStream, PlayHistory>>();

		protected override void Awake()
		{
			base.Awake();

			RootPath = Application.persistentDataPath + @"\History\";
			protectModify = true;

			if (!Directory.Exists(RootPath)) Directory.CreateDirectory(RootPath);
			string[] files = Directory.GetFiles(RootPath, "*.dat");
			foreach(string file in files)
			{
				FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
				PlayHistory data = formatter.Deserialize(stream) as PlayHistory;

				if (data.songId + ".dat" != Path.GetFileName(file)) return; // 파일명과 데이터상의 곡 ID가 불일치하는 경우 등록하지 않음

				history.Add(new System.Tuple<ulong, FileStream, PlayHistory>(data.songId, stream, data));
			}
		}

		private void Update()
		{
			if (SceneManager.GetActiveScene().name != "SongSelectSingleScene" &&
				SceneManager.GetActiveScene().name != "InGameSingleScene" &&
				SceneManager.GetActiveScene().name != "ResultSingleScene")
				Destroy(gameObject);
		}

		/// <summary>
		/// 특정한 곡의 플레이 기록을 반환합니다.
		/// </summary>
		/// <param name="id">찾을 곡의 ID를 입력합니다.</param>
		/// <returns>해당하는 곡의 플레이 기록을 반환합니다. 플레이 기록이 없을 경우, 기본값을 반환합니다.</returns>
		public PlayHistory GetHistory(ulong id)
		{
			if (history.Exists(target => target.Item1 == id))
			{
				return history.Find(target => target.Item1 == id).Item3;
			}
			return new PlayHistory(id, 0, 0, 0, PlayRank.X);
		}

		/// <summary>
		/// 특정한 곡의 플레이 기록을 추가합니다.
		/// 기존에 플레이 기록이 있다면, 덮어씌웁니다.
		/// </summary>
		/// <param name="playHistory">플레이 기록을 입력합니다.</param>
		public void AddHistory(PlayHistory playHistory)
		{
			if (history.Exists(target => target.Item1 == playHistory.songId))
			{
				Tuple<ulong, FileStream, PlayHistory> target = history.Find(target => target.Item1 == playHistory.songId);
				target.Item2.Close();
				history.Remove(target);
			}

			FileStream stream = new FileStream(RootPath + playHistory.songId + ".dat", FileMode.Create, FileAccess.Write);
			formatter.Serialize(stream, playHistory);

			history.Add(new System.Tuple<ulong, FileStream, PlayHistory>(playHistory.songId, stream, playHistory));
		}

		private void OnDestroy()
		{
			foreach (Tuple<ulong, FileStream, PlayHistory> data in history)
			{
				data.Item2.Close();
			}
		}
	}
}
