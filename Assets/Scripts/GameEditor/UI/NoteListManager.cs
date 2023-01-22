using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Notes;

namespace MineBeat.GameEditor.UI
{
	/// <summary>
	/// UI 우측의 노트 목록 창 항목을 관리합니다.
	/// </summary>
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
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			toggleGroup = gameObject.GetComponent<ToggleGroup>();
			notesManager = managers.Find(target => target.name == "NoteManagers").GetComponent<NotesManager>();
		}

		private void Update()
		{
			deleteNoteButton.interactable = toggleGroup.AnyTogglesOn();

			if (toggleGroup.AnyTogglesOn() && Input.GetKeyDown(KeyCode.Delete))
			{
				DeleteSelectedNote();
			}
		}

		/// <summary>
		/// 노트 목록을 갱신합니다.
		/// </summary>
		/// <param name="notes">갱신할 정보를 입력합니다.</param>
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

		/// <summary>
		/// 선택된 노트를 제거합니다.
		/// </summary>
		public void DeleteSelectedNote()
		{
			GameObject selected = gameObject;

			foreach (Toggle item in toggleGroup.ActiveToggles())
			{
				selected = item.gameObject.transform.parent.gameObject;
				break;
			}

			notesManager.Remove(selected.GetComponent<NoteListContent>().Note);
		}
	}
}
