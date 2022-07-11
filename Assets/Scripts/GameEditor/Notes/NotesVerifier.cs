using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Namespace] Minebeat.GameEditor.Notes
 * Desciption
 */
namespace MineBeat.GameEditor.Notes
{
	/*
	 * [Class] NotesVerifier
	 * 배치된 노트의 논리적 오류를 찾아냅니다.
	 */
	public class NotesVerifier : MonoBehaviour
	{
		[SerializeField]
		private Text textArea;
		[SerializeField]
		private Color colorSuccess;
		[SerializeField]
		private Color colorError;

		private bool _isError = false;
		public bool isError
		{
			get { return _isError; }
			private set { _isError = value; }
		}

		private string returnMessage = "";

		private NotesManager notesManager;

		private void Start()
		{
			notesManager = gameObject.GetComponent<NotesManager>();
		}

		/*
		 * [Method] Verify()
		 * 노트의 배치에 대한 논리적 오류를 찾고, 결과를 UI에 띄웁니다.
		 */
		public void Verify()
		{
			if (notesManager.Find(NoteType.ImpactLine).Count > 1)
			{
				isError = true;
				returnMessage = "There cannot be more than one 'Impact Line'.";
				ChangeMessage();
				return;
			}

			if (notesManager.Find(NoteType.BlankS).Count != notesManager.Find(NoteType.BlankE).Count)
			{
				isError = true;
				returnMessage = "There is not enough 'Blank Area E' corresponding to 'Blank Area S'. (or maybe vice versa)";
				ChangeMessage();
				return;
			}

			if (notesManager.Find(NoteType.PreviewS).Count != 1 || notesManager.Find(NoteType.PreviewE).Count != 1)
			{
				isError = true;
				returnMessage = "There must be one 'Preview Area S' and one 'Preview Area E'.";
				ChangeMessage();
				return;
			}

			isError = false;
			returnMessage = "No error detected.";
			ChangeMessage();
		}


		/*
		 * [Method] Verify()
		 * UI의 텍스트와 색상을 바꿉니다.
		 */
		private void ChangeMessage()
		{
			textArea.color = isError ? colorError : colorSuccess;
			textArea.text = "※ " + (isError ? "[ERROR] " : "[SUCCESS] " ) + returnMessage;
		}
	}
}
