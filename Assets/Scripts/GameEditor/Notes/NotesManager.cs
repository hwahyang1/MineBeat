using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.GameEditor.UI;

namespace MineBeat.GameEditor.Notes
{
	/// <summary>
	/// 노트의 추가와 제거를 관리합니다.
	/// </summary>
	public class NotesManager : MonoBehaviour
	{
		[SerializeField]
		private NoteListManager noteListManager;
		private NotesVerifier notesVerifier;

		private List<Note> notes = new List<Note>();

		private void Start()
		{
			notesVerifier = GetComponent<NotesVerifier>();
		}

		/// <summary>
		/// 노트 목록을 덮어씌웁니다.
		/// </summary>
		/// <param name="item">덮어씌울 노트의 정보를 담습니다.</param>
		public void Set(List<Note> item)
		{
			notes = item;

			SortList();
			notesVerifier.Verify();
		}

		/// <summary>
		/// 노트 목록에 새로운 항목을 추가합니다.
		/// </summary>
		/// <param name="item">추가할 노트의 정보를 담습니다.</param>
		public void Add(Note item)
		{
			if (notes.Contains(item)) return;
			notes.Add(item);

			SortList();
			notesVerifier.Verify();
		}

		/// <summary>
		/// 노트 목록에 특정 항목을 제거합니다.
		/// </summary>
		/// <param name="item">제거할 노트의 정보를 담습니다.</param>
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

		/// <summary>
		/// 노트 목록을 초기화 합니다.
		/// </summary>
		public void RemoveAll()
		{
			notes.Clear();

			noteListManager.UpdateList(notes);
			notesVerifier.Verify();
		}

		/// <summary>
		/// 입력된 값에 부합하는 노트를 찾습니다.
		/// </summary>
		/// <param name="timecode">검색을 원하는 값을 입력합니다.</param>
		/// <param name="allowUpToSec">초단위로 일치하는 값까지 반환할 지 여부를 결정합니다. false일 경우, ms단위까지 정확히 일치해야 반환합니다.</param>
		/// <returns>검색 기준에 부합하는 결과값을 반환합니다. 결과값이 없을 경우, 빈 List가 반환됩니다.</returns>
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

		/// <summary>
		/// 입력된 값에 부합하는 노트를 찾습니다.
		/// </summary>
		/// <param name="noteType">검색을 원하는 값을 입력합니다.</param>
		/// <returns>검색 기준에 부합하는 결과값을 반환합니다. 결과값이 없을 경우, 빈 List가 반환됩니다.</returns>
		public List<Note> Find(NoteType noteType)
		{
			List<Note> result = new List<Note>();

			result = notes.FindAll(target => target.type == noteType);

			return result;
		}

		/// <summary>
		/// 입력된 값에 부합하는 노트를 찾습니다.
		/// </summary>
		/// <param name="noteType">검색을 원하는 값을 입력합니다.</param>
		/// <param name="timecode">검색을 원하는 값을 입력합니다.</param>
		/// <param name="allowUpToSec">초단위로 일치하는 값까지 반환할 지 여부를 결정합니다. false일 경우, ms단위까지 정확히 일치해야 반환합니다.</param>
		/// <returns>검색 기준에 모두 부합하는(AND) 결과값을 반환합니다. 결과값이 없을 경우, 빈 List가 반환됩니다.</returns>
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

		/// <summary>
		/// 노트 목록을 시간순으로 내림차순 정렬합니다.
		/// </summary>
		public void SortList()
		{
			notes = notes.OrderBy(n => n.timeCode).ThenBy(n => n.type)/*.ThenBy(n => n.position.x).ThenBy(n => n.position.y)*/.ToList();
			noteListManager.UpdateList(notes);
		}

		/// <summary>
		/// 노트 목록을 반환합니다.
		/// </summary>
		/// <returns>노트 목록이 담겨있는 List 입니다.</returns>
		public List<Note> GetList() => notes;
	}
}
