using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using NaughtyAttributes;

using MineBeat.GameEditor.Files;
using MineBeat.GameEditor.Notes;
using UnityEngine.Serialization;

namespace MineBeat.GameEditor
{
	/// <summary>
	/// 게임의 전반적인 실행을 관리합니다.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private Transform songInfoArea;
		private NotesManager notesManager;
		private FileManager fileManager;

		[SerializeField]
		private TMP_InputField[] blockInputs = new TMP_InputField[3];

		[SerializeField]
		public bool blockInput = false;

		[SerializeField, ReadOnly]
		private ulong songId;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			notesManager = managers.Find(target => target.name == "NoteManagers").GetComponent<NotesManager>();
			fileManager = managers.Find(target => target.name == "FileManager").GetComponent<FileManager>();
		}

		private void Update()
		{
			foreach (TMP_InputField inputField in blockInputs)
			{
				if (inputField.isFocused)
				{
					blockInput = true;
					return;
				}
			}

			if (fileManager.MaintainCanvas)
			{
				blockInput = true;
				EventSystem.current.SetSelectedGameObject(null);
			}
			else
			{
				blockInput = false;
			}
		}

		/// <summary>
		/// 곡 정보를 반환합니다.
		/// </summary>
		/// <returns>곡 정보를 반환합니다.</returns>
		public SongInfo GetSongInfo()
		{
			string songName = songInfoArea.GetChild(0).GetComponent<TMP_InputField>().text;
			string songAuthor = songInfoArea.GetChild(1).GetComponent<TMP_InputField>().text;
			string songLevel = songInfoArea.GetChild(3).GetComponent<TMP_InputField>().text;

			return new SongInfo(
				songId,
				songName == "" ? "SongName" : songName,
				songAuthor == "" ? "Author" : songAuthor,
				songLevel == "" ? (ushort)0 : ushort.Parse(songLevel),
				notesManager.GetList()
			);
		}

		/// <summary>
		/// 곡 정보를 덮어씌웁니다.
		/// </summary>
		/// <param name="info">덮어씌울 곡의 정보를 담습니다.</param>
		public void SetSongInfo(SongInfo info)
		{
			songId = info.id;
			songInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = info.songName == "SongName" ? "" : info.songName;
			songInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = info.songAuthor == "Author" ? "" : info.songAuthor;
			songInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = info.songLevel == 0 ? "" : info.songLevel + "";
			notesManager.Set(info.notes);
		}

		/// <summary>
		/// 곡의 고유 ID를 지정합니다.
		/// </summary>
		/// <param name="id">지정할 ID를 입력합니다.</param>
		public void SetSongId(ulong id)
		{
			songId = id;
		}

		/// <summary>
		/// 곡 정보 입력란을 비웁니다.
		/// </summary>
		public void ClearSongInfoInput()
		{
			songInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = "";
			songInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = "";
			songInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = "";
		}
	}
}
