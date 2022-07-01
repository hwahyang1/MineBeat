using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MineBeat.GameEditor.Notes;

/*
 * [Namespace] Minebeat.GameEditor.UI
 * Desciption
 */
namespace MineBeat.GameEditor.UI
{
	/*
	 * [Class] NoteListContent
	 * UI 오른쪽 노트 목록 창의 항목 생성을 수행합니다.
	 */
	public class NoteListContent : MonoBehaviour
	{
		private Note _note;
		public Note note
		{
			get { return _note; }
			private set { _note = value; }
		}

		/*
		 * [Method] TimeCodeSelected(): void
		 * TimeCode가 클릭되었을 때 해당 위치로 Timeline을 이동시켜주는 역할을 합니다.`
		 */
		public void TimeCodeSelected()
		{
			GameObject.Find("TimeLine").GetComponent<TimelineManager>().ChangeTimeCode(note.timeCode);
		}

		/*
		 * [Method] Selected(): void
		 * Toggle Button이 아닌 텍스트가 클릭되었을 때 Toggle Button을 제어해주는 역할을 합니다.
		 */
		public void Selected()
		{
			gameObject.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = !gameObject.transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn;
		}

		/*
		 * [Method] Perform(Note note): void
		 * 항목의 초기 생성을 진행합니다.
		 * 
		 * <Note note>
		 * 항목의 값을 입력받습니다.
		 */
		public void Perform(Note note)
		{
			string noteTimeCodeText;
			string noteTypeText = NoteType.Normal.ToString();
			string notePositionText = "-";

			int front = Mathf.FloorToInt(note.timeCode);
			float back = Mathf.FloorToInt((note.timeCode - front) * 1000);

			int min = front / 60;
			int sec = front % 60;

			noteTimeCodeText = string.Format("{0:00}:{1:00}:{2:000}", min, sec, back);

			switch (note.type)
			{
				case NoteType.Normal:
					notePositionText = string.Format("{0}, {1}", note.position.x, note.position.y);
					break;
				case NoteType.Vertical:
					notePositionText = string.Format("{0}", note.position.x);
					break;
				case NoteType.SizeChange:
					noteTypeText = "Size Change";
					notePositionText = string.Format("{0} → {1}", note.position.x, note.position.y);
					break;
				case NoteType.BlankS:
					noteTypeText = "Blank Area S";
					break;
				case NoteType.BlankE:
					noteTypeText = "Blank Area E";
					break;
				case NoteType.ImpactLine:
					noteTypeText = "Impact Line S";
					break;
			}

			gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = noteTimeCodeText;
			gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = noteTypeText;
			gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = notePositionText;

			this.note = note;

			//Destroy(gameObject);
		}
	}
}
