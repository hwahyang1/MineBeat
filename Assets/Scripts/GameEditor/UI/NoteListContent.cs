using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Song;

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

		private SongManager songManager;

		private void Start()
		{
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			float currentTime = songManager.GetCurrentTime();
			Color setColor = new Color(0.9019608f, 0.9019608f, 0.9294118f);

			if (note.type == NoteType.Normal || note.type == NoteType.Vertical)
			{
				if (note.timeCode - 0.35f <= currentTime && currentTime < note.timeCode)
				{
					setColor = new Color(0.8823529f, 0.9490196f, 0f);
				}
			}
			
			if (note.timeCode <= currentTime && currentTime <= note.timeCode + 0.2f)
			{
				setColor = new Color(0f, 0.9686275f, 0.227451f);
			}

			gameObject.transform.GetChild(1).GetComponent<Text>().color = setColor;
		}

		/*
		 * [Method] TimeCodeSelected(): void
		 * TimeCode가 클릭되었을 때 해당 위치로 Timeline을 이동시켜주는 역할을 합니다.
		 */
		public void TimeCodeSelected()
		{
			GameObject.Find("UIManagers").GetComponent<TimelineManager>().ChangeTimeCode(note.timeCode);
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
			string noteTypeText = note.type.ToString();
			string notePositionText = "-";

			int front = Mathf.FloorToInt(note.timeCode);
			float back = Mathf.FloorToInt((note.timeCode - front) * 1000);

			int min = front / 60;
			int sec = front % 60;

			noteTimeCodeText = string.Format("{0:00}:{1:00}:{2:000}", min, sec, back);

			switch (note.type)
			{
				case NoteType.Normal:
					noteTypeText += string.Format(" ({0})", note.color.ToString()[0]);
					notePositionText = string.Format("{0}, {1}", note.position.x, note.position.y);
					break;
				case NoteType.Vertical:
					noteTypeText += string.Format(" ({0})", note.color.ToString()[0]);
					if (note.direction == NoteDirection.Up || note.direction == NoteDirection.Down)
					{
						notePositionText = string.Format("{0}", note.position.x);
					}
					else
					{
						notePositionText = string.Format("{0}", note.position.y);
					}
					break;
				case NoteType.BoxColorChange:
					noteTypeText = "Color Change";
					notePositionText = note.color.ToString();
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
				case NoteType.PreviewS:
					noteTypeText = "Preview Area S";
					break;
				case NoteType.PreviewE:
					noteTypeText = "Preview Area E";
					break;
			}

			gameObject.transform.GetChild(1).GetComponent<Text>().text = noteTimeCodeText;
			gameObject.transform.GetChild(2).GetComponent<Text>().text = noteTypeText;
			gameObject.transform.GetChild(3).GetComponent<Text>().text = notePositionText;

			this.note = note;

			//Destroy(gameObject);
		}
	}
}
