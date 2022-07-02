using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Song;
using MineBeat.GameEditor.Notes;

/*
 * [Namespace] Minebeat.GameEditor.UI
 * Desciption
 */
namespace MineBeat.GameEditor.UI
{
	/*
	 * [Enum] ObjectType
	 * Object탭에서 사용 가능한 항목을 정의합니다.
	 */
	public enum ObjectType
	{
		Normal,
		Vertical,
		None
	}

	/*
	 * [Class] ToolBarInteract
	 * UI 좌측 Toolbar의 Interaction을 관리합니다.
	 */
	public class ToolBarInteract : MonoBehaviour
	{
		[Header("Object Tab")]
		[SerializeField, Tooltip("버튼 GameObject를 ObjectType에 맞게 배치합니다.")]
		private Button[] objectButtons = new Button[3];

		[HideInInspector]
		public ObjectType currentObject = ObjectType.None;

		private NotesManager notesManager;
		private SongManager songManager;

		private void Start()
		{
			notesManager = GameObject.Find("Notes").GetComponent<NotesManager>();
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			if (songManager.playStatus != PlayStatus.Stopped)
			{
				currentObject = ObjectType.None;
			}

			for (int i = 0; i < objectButtons.Length; i++)
			{
				objectButtons[i].interactable = i != (int)currentObject;
			}
		}

		/*
		 * [Method] OnNormalNoteButtonClicked()
		 * Object 탭의 Normal Note 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnNormalNoteButtonClicked()
		{
			currentObject = ObjectType.Normal;
		}

		/*
		 * [Method] OnVerticalNoteButtonClicked()
		 * Object 탭의 Vertical Note 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnVerticalNoteButtonClicked()
		{
			currentObject = ObjectType.Vertical;
		}

		/*
		 * [Method] OnNoneButtonClicked()
		 * Object 탭의 None 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnNoneButtonClicked()
		{
			currentObject = ObjectType.None;
		}

		/*
		 * [Method] OnBlankAreaStartButtonClicked()
		 * Timeline -> Blank Area 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBlankAreaStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.BlankS, new NotePosition()));
		}

		/*
		 * [Method] OnBlankAreaEndButtonClicked()
		 * Timeline -> Blank Area 탭의 End 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBlankAreaEndButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.BlankE, new NotePosition()));
		}

		/*
		 * [Method] OnBlankAreaDeleteButtonClicked()
		 * Timeline -> Blank Area 탭의 Delete 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBlankAreaDeleteButtonClicked()
		{

		}

		/*
		 * [Method] OnImpactLineStartButtonClicked()
		 * Timeline -> Imapct Line 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnImpactLineStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.ImpactLine, new NotePosition()));
		}
	}
}
