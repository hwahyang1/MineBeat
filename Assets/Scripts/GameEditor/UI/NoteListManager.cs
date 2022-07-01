using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Notes;

/*
 * [Namespace] Minebeat.GameEditor.UI
 * Desciption
 */
namespace MineBeat.GameEditor.UI
{
	/*
	 * [Class] NoteListManager
	 * UI 우측의 노트 목록 창 항목을 관리합니다.
	 */
	public class NoteListManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject contentGroupPrefab;
		[SerializeField]
		private Button deleteNoteButton;

		private ToggleGroup toggleGroup;
		private NotesManager notesManager;

		private void Start()
		{
			toggleGroup = gameObject.GetComponent<ToggleGroup>();
			notesManager = GameObject.Find("Notes").GetComponent<NotesManager>();
		}

		private void Update()
		{
			deleteNoteButton.interactable = toggleGroup.AnyTogglesOn();

			if (toggleGroup.AnyTogglesOn() && Input.GetKeyDown(KeyCode.Delete))
			{
				DeleteSelectedNote();
			}
		}

		/*
		 * [Method] UpdateList(): void
		 * 노트 목록을 갱신합니다.
		 */
		public void UpdateList(List<Note> notes)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Destroy(gameObject.transform.GetChild(i).gameObject);
			}
			foreach (Note note in notes)
			{
				GameObject content = Instantiate(contentGroupPrefab, transform);
				content.transform.GetChild(0).GetComponent<Toggle>().group = toggleGroup;
				content.GetComponent<NoteListContent>().Perform(note);
			}
		}

		/*
		 * [Method] DeleteSeletedNote(): void
		 * 선택된 노트를 제거합니다.
		 */
		public void DeleteSelectedNote()
		{
			GameObject selected = gameObject;

			foreach (Toggle item in toggleGroup.ActiveToggles())
			{
				selected = item.gameObject.transform.parent.gameObject;
				break;
			}

			notesManager.Remove(selected.GetComponent<NoteListContent>().note);
		}
	}
}
