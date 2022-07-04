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
	 * [Enum] NoteDirection
	 * 박스를 기준으로 노트에 어느 방향에 존재하는지 표현합니다.
	 */
	[System.Serializable]
	public enum NoteDirection
	{
		Up,
		Down,
		Left,
		Right,
		None
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
		public NoteDirection direction;

		public Note(float timeCode, NoteType type)
		{
			this.timeCode = timeCode;
			this.type = type;

			position = new NotePosition(0, 0);
			direction = NoteDirection.None;
		}
		public Note(float timeCode, NoteType type, NotePosition position)
		{
			this.timeCode = timeCode;
			this.type = type;
			this.position = position;

			direction = NoteDirection.None;
		}
		public Note(float timeCode, NoteType type, NotePosition position, NoteDirection direction)
		{
			this.timeCode = timeCode;
			this.type = type;
			this.position = position;
			this.direction = direction;
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
		private NotesVerifier notesVerifier;

		private List<Note> notes = new List<Note>();

		private void Start()
		{
			notesVerifier = gameObject.GetComponent<NotesVerifier>();
		}

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
			notesVerifier.Verify();
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
			notesVerifier.Verify();
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
			notesVerifier.Verify();
		}

		/*
		 * [Method] Remove(Note item): void
		 * 노트 목록을 초기화 합니다.
		 */
		public void RemoveAll()
		{
			notes.Clear();

			noteListManager.UpdateList(notes);
			notesVerifier.Verify();
		}

		/*
		 * [Method] Find(float timecode, bool allowUpToSec = false)
		 * 입력된 값에 부합하는 노트를 찾습니다.
		 * 
		 * <float timecode>
		 * 검색을 원하는 값을 입력합니다.
		 * 
		 * <bool allowUpToSec = false>
		 * 초단위로 일치하는 값까지 반환할 지 여부를 결정합니다.
		 * false일 경우, ms단위까지 정확히 일치해야 반환합니다.
		 * 
		 * <RETURN: List<Note>>
		 * 검색 기준에 부합하는 결과값을 반환합니다.
		 * 결과값이 없을 경우, 빈 List가 반환됩니다.
		 */
		public List<Note> Find(float timecode, bool allowUpToSec = false)
		{
			List<Note> result = new List<Note>();

			if (allowUpToSec)
			{
				result = notes.FindAll(target => Mathf.FloorToInt(target.timeCode) == Mathf.FloorToInt(timecode));
			}
			else
			{
				result = notes.FindAll(target => target.timeCode == timecode);
			}

			return result;
		}

		/*
		 * [Method] Find(NoteType noteType)
		 * 입력된 값에 부합하는 노트를 찾습니다.
		 * 
		 * <NoteType noteType>
		 * 검색을 원하는 값을 입력합니다.
		 * 
		 * <RETURN: List<Note>>
		 * 검색 기준에 부합하는 결과값을 반환합니다.
		 * 결과값이 없을 경우, 빈 List가 반환됩니다.
		 */
		public List<Note> Find(NoteType noteType)
		{
			List<Note> result = new List<Note>();

			result = notes.FindAll(target => target.type == noteType);

			return result;
		}

		/*
		 * [Method] Find(NoteType noteType, float timecode, bool allowUpToSec = false)
		 * 입력된 값에 부합하는 노트를 찾습니다.
		 * 
		 * <NoteType noteType>
		 * 검색을 원하는 값을 입력합니다.
		 * 
		 * <float timecode>
		 * 검색을 원하는 값을 입력합니다.
		 * 
		 * <bool allowUpToSec = false>
		 * 초단위로 일치하는 값까지 반환할 지 여부를 결정합니다.
		 * false일 경우, ms단위까지 정확히 일치해야 반환합니다.
		 * 
		 * <RETURN: List<Note>>
		 * 검색 기준에 모두 부합하는(AND) 결과값을 반환합니다.
		 * 결과값이 없을 경우, 빈 List가 반환됩니다.
		 */
		public List<Note> Find(NoteType noteType, float timecode, bool allowUpToSec = false)
		{
			List<Note> result = new List<Note>();

			if (allowUpToSec)
			{
				result = notes.FindAll(target => Mathf.FloorToInt(target.timeCode) == Mathf.FloorToInt(timecode));
			}
			else
			{
				result = notes.FindAll(target => target.timeCode == timecode);
			}

			result = result.FindAll(target => target.type == noteType);

			return result;
		}

		/*
		 * [Mehtod] SortList(): void
		 * 노트 목록을 시간순으로 내림차순 정렬합니다.
		 */
		public void SortList()
		{
			notes = notes.OrderBy(n => n.timeCode).ThenBy(n => n.type)/*.ThenBy(n => n.position.x).ThenBy(n => n.position.y)*/.ToList();
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
