using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/*
 * [Namespace] Minebeat.GameEditor
 * Desciption
 */
namespace MineBeat.GameEditor
{
	/*
	 * [Enum] NoteType
	 * 노트의 종류를 정의합니다.
	 */
	public enum NoteType
	{
		Normal,
		Vertical,
		SizeChange,
		BlankS,
		BlankE,
		ImpactLine
	}

	/*
	 * [Struct] Note
	 * 노트의 정보를 정의합니다.
	 */
	public struct Note
	{
		public float timeCode;
		public NoteType type;
		public Vector2Int position;
	}

	/*
	 * [Class] NoteListManager
	 * 노트의 목록을 관리합니다.
	 */
	public class NoteListManager : MonoBehaviour
	{
		private List<Note> notes = new List<Note>();

		/*
		 * [Method] Add(Note item): void
		 * 노트 목록에 새로운 항목을 추가합니다.
		 * 
		 * <Note item>
		 * 추가할 노트의 정보를 담습니다.
		 */
		public void Add(Note item)
		{
			notes.Add(item);
			SortList();
		}

		/*
		 * [Method] Remove(Note item): void
		 * 노트 목록에 특정 항목을 제거합니다.
		 * 
		 * <Note item>
		 * 제거할 노트의 정보를 담습니다.
		 */
		public void Remove(Note item)
		{
			for (int i = 0; i < notes.Count; i++)
			{
				if (notes[i].Equals(item))
				{
					notes.RemoveAt(i);
					break;
				}
			}
		}

		/*
		 * [Method] Remove(Note item): void
		 * 노트 목록을 초기화 합니다.
		 */
		public void RemoveAll()
		{
			notes.Clear();
		}

		/*
		 * [Mehtod] SortList(): void
		 * 노트 목록을 시간순으로 내림차순 정렬합니다.
		 */
		public void SortList()
		{
			notes = notes.OrderBy(n => n.timeCode).ThenBy(n => n.type).ThenBy(n => n.position).ToList();
		}

		/*
		 * [Method] GetList(): List<Note>
		 * 노트 목록을 반환합니다.
		 * 
		 * <RETURN: List<Note>>
		 * 노트 목록이 담겨있는 List 입니다.
		 */
		public List<Note> GetList()
		{
			return notes;
		}
	}
}
