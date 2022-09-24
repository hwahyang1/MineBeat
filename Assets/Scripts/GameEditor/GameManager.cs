using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using MineBeat.GameEditor.Files;
using MineBeat.GameEditor.Notes;

namespace MineBeat.GameEditor
{
	/// <summary>
	/// 게임의 전반적인 실행을 관리합니다.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private Transform SongInfoArea;
		private NotesManager notesManager;
		private FileManager fileManager;

		[SerializeField]
		private TMP_InputField[] blockInputs = new TMP_InputField[3];

		private ulong songId;

		[SerializeField]
		public bool blockInput = false;

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

			if (fileManager.maintainCanvas)
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
			string songName = SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text;
			string songAuthor = SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text;
			string songLevel = SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text;

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
			SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = info.songName == "SongName" ? "" : info.songName;
			SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = info.songAuthor == "Author" ? "" : info.songAuthor;
			SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = info.songLevel == 0 ? "" : info.songLevel + "";
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
			SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = "";
			SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = "";
			SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = "";
		}
	}
}
