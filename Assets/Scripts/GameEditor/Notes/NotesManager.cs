using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] Minebeat.GameEditor.Notes
 * Desciption
 */
namespace MineBeat.GameEditor.Notes
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
	 * [Class] Note
	 * 노트의 정보를 정의합니다.
	 */
	public class Note
	{
		public float timeCode;
		public NoteType type;
		public Vector2Int position;
	}

	/*
	 * [Class] NotesManager
	 * 노트의 추가와 제거를 관리합니다.
	 */
	public class NotesManager : MonoBehaviour
	{
		private List<Note> notes = new List<Note>();

		/*
		 * [Method] Set(List<Note> item): void
		 * 노트 목록을 덮어씌웁니다.
		 * 
		 * <List<Note> item>
		 * 덮어씌울 노트의 정보를 담습니다.
		 */
		public void Set(List<Note> item)
		{
			notes = item;
		}

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
