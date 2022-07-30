using System.Linq;
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

		[SerializeField]
		private Text currentColorText;

		[HideInInspector]
		public ObjectType currentObject = ObjectType.None;

		[HideInInspector]
		public NoteColor currentColor = NoteColor.White;

		private NotesManager notesManager;
		private SongManager songManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			notesManager = managers.Find(target => target.name == "NoteManagers").GetComponent<NotesManager>();
			songManager = managers.Find(target => target.name == "SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				currentObject = ObjectType.None;
			}

			for (int i = 0; i < objectButtons.Length; i++)
			{
				objectButtons[i].interactable = i != (int)currentObject;
			}

			currentColorText.text = currentColor.ToString();
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
		 * [Method] OnBeforeColorClicked(): void
		 * Object 탭의 이전 색상으로 변경 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBeforeColorClicked()
		{
			if (currentColor == 0) currentColor = NoteColor.Purple; // 더 앞으로 갈 수 없으니 맨 뒤로 보냄
			else currentColor--;
		}

		/*
		 * [Method] OnAfterColorClicked(): void
		 * Object 탭의 다음 색상으로 변경 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnAfterColorClicked()
		{
			if (currentColor == System.Enum.GetValues(typeof(NoteColor)).Cast<NoteColor>().Last()) currentColor = NoteColor.White; // 더 뒤로 갈 수 없으니 맨 앞으로 보냄
			else currentColor++;
		}

		/*
		 * [Method] OnBlankAreaStartButtonClicked()
		 * Timeline -> Blank Area 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBlankAreaStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.BlankS));
		}

		/*
		 * [Method] OnBlankAreaEndButtonClicked()
		 * Timeline -> Blank Area 탭의 End 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBlankAreaEndButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.BlankE));
		}

		/*
		 * [Method] OnBlankAreaDeleteButtonClicked()
		 * Timeline -> Blank Area 탭의 Delete 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnBlankAreaDeleteButtonClicked()
		{
			// TODO
		}

		/*
		 * [Method] OnImpactLineStartButtonClicked()
		 * Timeline -> Imapct Line 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnImpactLineStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.ImpactLine));
		}

		/*
		 * [Method] OnPreviewAreaStartButtonClicked()
		 * Timeline -> Preview Area 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnPreviewAreaStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.PreviewS));
		}

		/*
		 * [Method] OnPreviewAreaEndButtonClicked()
		 * Timeline -> Preview Area 탭의 End 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnPreviewAreaEndButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.PreviewE));
		}

		/*
		 * [Method] OnPreviewAreaDeleteButtonClicked()
		 * Timeline -> Preview Area 탭의 Delete 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnPreviewAreaDeleteButtonClicked()
		{
			// TODO
		}
	}
}
