using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.GameEditor.UI;

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
	[System.Serializable]
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
	 * [Class] NotePosition
	 * 노트의 위치를 정의합니다.
	 */
	[System.Serializable]
	public class NotePosition
	{
		public ushort x;
		public ushort y;
		public NotePosition()
		{
			x = 0;
			y = 0;
		}
		public NotePosition(ushort x, ushort y)
		{
			this.x = x;
			this.y = y;
		}
		public NotePosition(int x, int y)
		{
			this.x = (ushort)x;
			this.y = (ushort)y;
		}
	}

	/*
	 * [Class] Note
	 * 노트의 정보를 정의합니다.
	 */
	[System.Serializable]
	public class Note
	{
		public float timeCode;
		public NoteType type;
		public NotePosition position;

		public Note(float timeCode, NoteType type, NotePosition position)
		{
			this.timeCode = timeCode;
			this.type = type;
			this.position = position;
		}
	}

	/*
	 * [Class] NotesManager
	 * 노트의 추가와 제거를 관리합니다.
	 */
	public class NotesManager : MonoBehaviour
	{
		[SerializeField]
		private NoteListManager noteListManager;

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
			SortList();
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
			if (notes.Contains(item)) return;
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
			if (!notes.Contains(item)) return;

			for (int i = 0; i < notes.Count; i++)
			{
				if (notes[i].Equals(item))
				{
					notes.RemoveAt(i);
					break;
				}
			}
			noteListManager.UpdateList(notes);
		}

		/*
		 * [Method] Remove(Note item): void
		 * 노트 목록을 초기화 합니다.
		 */
		public void RemoveAll()
		{
			notes.Clear();
			noteListManager.UpdateList(notes);
		}

		/*
		 * [Mehtod] SortList(): void
		 * 노트 목록을 시간순으로 내림차순 정렬합니다.
		 */
		public void SortList()
		{
			notes = notes.OrderBy(n => n.timeCode).ThenBy(n => n.type).ThenBy(n => n.position.x).ThenBy(n => n.position.y).ToList();
			noteListManager.UpdateList(notes);
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
