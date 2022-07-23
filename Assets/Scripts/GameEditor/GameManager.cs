using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

using MineBeat.GameEditor.Files;
using MineBeat.GameEditor.Notes;

/*
 * [Namespace] Minebeat.GameEditor
 * Desciption
 */
namespace MineBeat.GameEditor
{
	/*
	 * [Class] GameManager
	 * 게임의 전반적인 실행을 관리합니다.
	 */
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private Transform SongInfoArea;
		private NotesManager notesManager;
		private FileManager fileManager;

		private ulong songId;

		public bool blockInput = false;

		private void Start()
		{
			notesManager = GameObject.Find("NoteManagers").GetComponent<NotesManager>();
			fileManager = GameObject.Find("FileManager").GetComponent<FileManager>();
		}

		private void Update()
		{
			if (!fileManager.maintainCanvas && EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.parent != SongInfoArea)
			{
				blockInput = false;
				EventSystem.current.SetSelectedGameObject(null);
			}
			else
			{
				blockInput = true;
			}
		}

		/*
		 * [Method] GetSongInfo(): SongInfo
		 * 곡 정보를 반환합니다.
		 * 
		 * <REUTRN: SongInfo>
		 * 곡 정보를 반환합니다.
		 */
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

		/*
		 * [Method] Set(SongInfo info): void
		 * 곡 정보를 덮어씌웁니다.
		 * 
		 * <SongInfo info>
		 * 덮어씌울 곡의 정보를 담습니다.
		 */
		public void SetSongInfo(SongInfo info)
		{
			songId = info.id;
			SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = info.songName == "SongName" ? "" : info.songName;
			SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = info.songAuthor == "Author" ? "" : info.songAuthor;
			SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = info.songLevel == 0 ? "" : info.songLevel + "";
			notesManager.Set(info.notes);
		}

		/*
		 * [Method] SetSongId(ulong id): void
		 * 곡의 고유 ID를 지정합니다.
		 */
		public void SetSongId(ulong id)
		{
			songId = id;
		}

		/*
		 * [Method] ClearSongInfoInput(): void
		 * 곡 정보 입력란을 비웁니다.
		 */
		public void ClearSongInfoInput()
		{
			SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = "";
			SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = "";
			SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = "";
		}
	}
}
