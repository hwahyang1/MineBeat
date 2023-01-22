using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace MineBeat.GameEditor.Notes
{
	/// <summary>
	/// 배치된 노트의 논리적 오류를 찾아냅니다.
	/// </summary>
	public class NotesVerifier : MonoBehaviour
	{
		[SerializeField]
		private Text textArea;
		[SerializeField]
		private Color colorSuccess;
		[SerializeField]
		private Color colorError;

		public bool IsError { get; private set; } = false;

		private string returnMessage = "";

		private NotesManager notesManager;

		private void Start()
		{
			notesManager = GetComponent<NotesManager>();
		}

		/// <summary>
		/// 노트의 배치에 대한 논리적 오류를 찾고, 결과를 UI에 띄웁니다.
		/// </summary>
		public void Verify()
		{
			if (notesManager.Find(NoteType.ImpactLine).Count > 1)
			{
				IsError = true;
				returnMessage = "'Impact Line'은 최대 한개만 존재해야 합니다.";
				ChangeMessage();
				return;
			}

			if (notesManager.Find(NoteType.BlankS).Count != notesManager.Find(NoteType.BlankE).Count)
			{
				IsError = true;
				returnMessage = "'Blank Area S'에 대응하는 'Blank Area E'가 존재하지 않습니다. (또는 그 반대)";
				ChangeMessage();
				return;
			}

			if (notesManager.Find(NoteType.PreviewS).Count != 1 || notesManager.Find(NoteType.PreviewE).Count != 1)
			{
				IsError = true;
				returnMessage = "'Preview Area S'와 'Preview Area E'는 무조건 존재해야 하며, 한개만 존재해야 합니다.";
				ChangeMessage();
				return;
			}

			IsError = false;
			returnMessage = "문제가 발견되지 않았습니다.";
			ChangeMessage();
		}

		/// <summary>
		/// UI의 텍스트와 색상을 바꿉니다.
		/// </summary>
		private void ChangeMessage()
		{
			textArea.color = IsError ? colorError : colorSuccess;
			textArea.text = "※ " + (IsError ? "[경고] " : "[알림] " ) + returnMessage;
		}
	}
}
