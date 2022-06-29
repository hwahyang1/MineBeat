using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
	}

	/*
	 * [Class] GameManager
	 * 게임의 전반적인 실행을 관리합니다.
	 */
	public class GameManager : MonoBehaviour
	{

	}
}
