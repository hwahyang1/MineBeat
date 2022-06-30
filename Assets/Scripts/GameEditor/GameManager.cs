using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using MineBeat.GameEditor.Notes;

/*
 * [Namespace] Minebeat.GameEditor
 * Desciption
 */
namespace MineBeat.GameEditor
{
	/*
	 * [Class] SongInfo
	 * 곡의 전체 데이터를 담는 Class 입니다.
	 */
	[System.Serializable]
	public class SongInfo
	{
		public string songName;
		public string songAuthor;
		public ushort songLevel;
		public List<Note> notes;

		public SongInfo(string songName, string songAuthor, ushort songLevel, List<Note> notes)
		{
			this.songName = songName;
			this.songAuthor = songAuthor;
			this.songLevel = songLevel;
			this.notes = notes;
		}
	}

	/*
	 * [Class] SongManager
	 * 게임의 전반적인 실행을 관리합니다.
	 */
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private Transform SongInfoArea;
		private NotesManager notesManager;

		private void Start()
		{
			notesManager = GameObject.Find("Notes").GetComponent<NotesManager>();
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
			return new SongInfo(
				SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text,
				SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text,
				ushort.Parse(SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text),
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
			SongInfoArea.GetChild(0).GetComponent<TMP_InputField>().text = info.songName;
			SongInfoArea.GetChild(1).GetComponent<TMP_InputField>().text = info.songAuthor;
			SongInfoArea.GetChild(3).GetComponent<TMP_InputField>().text = info.songLevel+"";
			notesManager.Set(info.notes);
		} 
	}
}
