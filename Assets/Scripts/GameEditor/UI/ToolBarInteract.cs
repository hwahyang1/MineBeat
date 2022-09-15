using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Song;
using MineBeat.GameEditor.Notes;

namespace MineBeat.GameEditor.UI
{
	/// <summary>
	/// Object탭에서 사용 가능한 항목을 정의합니다.
	/// </summary>
	public enum ObjectType
	{
		Normal,
		Vertical,
		None
	}

	/// <summary>
	/// UI 좌측 Toolbar의 Interaction을 관리합니다.
	/// </summary>
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

		/// <summary>
		/// Object 탭의 Normal Note 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnNormalNoteButtonClicked()
		{
			currentObject = ObjectType.Normal;
		}

		/// <summary>
		/// Object 탭의 Vertical Note 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnVerticalNoteButtonClicked()
		{
			currentObject = ObjectType.Vertical;
		}

		/// <summary>
		/// Object 탭의 None 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnNoneButtonClicked()
		{
			currentObject = ObjectType.None;
		}

		/// <summary>
		/// Object 탭의 이전 색상으로 변경 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnBeforeColorClicked()
		{
			if (currentColor == 0) currentColor = NoteColor.Purple; // 더 앞으로 갈 수 없으니 맨 뒤로 보냄
			else currentColor--;
		}

		/// <summary>
		/// Object 탭의 다음 색상으로 변경 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnAfterColorClicked()
		{
			if (currentColor == System.Enum.GetValues(typeof(NoteColor)).Cast<NoteColor>().Last()) currentColor = NoteColor.White; // 더 뒤로 갈 수 없으니 맨 앞으로 보냄
			else currentColor++;
		}

		/// <summary>
		/// Timeline -> Blank Area 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnBlankAreaStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.BlankS));
		}

		/// <summary>
		/// Timeline -> Blank Area 탭의 End 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnBlankAreaEndButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.BlankE));
		}

		/// <summary>
		/// Timeline -> Blank Area 탭의 Delete 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnBlankAreaDeleteButtonClicked()
		{
			// TODO
		}

		/// <summary>
		/// Timeline -> Imapct Line 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnImpactLineStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.ImpactLine));
		}

		/// <summary>
		/// Timeline -> Preview Area 탭의 Start 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnPreviewAreaStartButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.PreviewS));
		}

		/// <summary>
		/// Timeline -> Preview Area 탭의 End 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnPreviewAreaEndButtonClicked()
		{
			notesManager.Add(new Note(songManager.GetCurrentTime(), NoteType.PreviewE));
		}

		/// <summary>
		/// Timeline -> Preview Area 탭의 Delete 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnPreviewAreaDeleteButtonClicked()
		{
			// TODO
		}
	}
}
