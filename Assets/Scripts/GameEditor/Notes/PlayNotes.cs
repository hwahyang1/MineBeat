using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.GameEditor.Song;
using MineBeat.GameEditor.TileBox;

namespace MineBeat.GameEditor.Notes
{
	/// <summary>
	/// TimeCode에 따른 노트의 위치를 계산하고, 노트를 배치 및 제거합니다.
	/// </summary>
	public class PlayNotes : MonoBehaviour
	{
		[Header("Tilemap")]
		[SerializeField]
		private Tilemap noteWarningTilemap;

		[SerializeField]
		private TileBase normalWarning;
		[SerializeField, Tooltip("노트의 위치를 기준으로 상, 하, 좌, 우에서 보일 TileBase를 입력하세요.")]
		private TileBase[] verticalWarning = new TileBase[4];

		[Header("Prefab")]
		[SerializeField]
		private GameObject boxNote;
		[SerializeField, Tooltip("DefineNote.NoteColor을 기준으로 해당되는 색상의 노트 Sprite를 입력하세요.")]
		private Sprite[] boxSprites = new Sprite[6];

		[Header("Parent")]
		[SerializeField]
		private Transform noteParent;
		[SerializeField]
		private Transform disabledNoteParent;

		[Header("Timing")]
		[SerializeField]
		private float aroundTime; // 몇초 전부터 Warining 띄울지
		[SerializeField]
		private float verticalTime; // 몇초간 유지할지
		[SerializeField]
		private float normalNoteSpeed;

		private float prevTime;
		private List<Note> prevNotes;

		private BoxSize boxSize;
		private NotesManager notesManager;
		private SongManager songManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			boxSize = GameObject.Find("Tilemaps").GetComponent<BoxSize>();
			notesManager = GetComponent<NotesManager>();
			songManager = managers.Find(target => target.name == "SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			float currentTime = songManager.GetCurrentTime();
			List<Note> notes = notesManager.GetList();

			if (prevTime != currentTime || prevNotes != notes)
			{
				prevTime = currentTime;
				prevNotes = notes;
				Draw(notes, currentTime);
			}
		}

		/// <summary>
		/// 화면상에 표기되는 노트를 다시 그립니다.
		/// </summary>
		/// <param name="notes">노트 정보를 입력합니다.</param>
		/// <param name="currentTime">현재 Timecode를 입력합니다.</param>
		public void Draw(List<Note> notes, float currentTime)
		{
			for (int i = 0; i < noteParent.childCount; i++)
			{
				//Destroy(noteParent.GetChild(i).gameObject);
				GameObject obj = noteParent.GetChild(0).gameObject;
				obj.transform.SetParent(disabledNoteParent);
				obj.SetActive(false);
			}
			noteWarningTilemap.ClearAllTiles();

			List<Note> prevSizeChange = notes.FindAll(target => target.timeCode < currentTime && target.type == NoteType.SizeChange);
			if (prevSizeChange.Count == 0)
			{
				boxSize.SetBoxSize(7);
			}
			else
			{
				boxSize.SetBoxSize(prevSizeChange[prevSizeChange.Count - 1].position.y);
			}

			List<Note> prevBlank = notes.FindAll(target => target.timeCode < currentTime && (target.type == NoteType.BlankS || target.type == NoteType.BlankE));
			if (prevBlank.Count == 0)
			{
				boxSize.gameObject.SetActive(true);
			}
			else
			{
				boxSize.gameObject.SetActive(prevBlank[prevBlank.Count - 1].type == NoteType.BlankE);
			}

			List<Note> earlyNotes = notes.FindAll(target => (currentTime - 1f <= target.timeCode && target.timeCode <= currentTime));
			foreach (Note current in earlyNotes)
			{
				switch (current.type)
				{
					case NoteType.Normal:
						int directionX = 0;
						int directionY = 0;

						switch (current.direction)
						{
							case NoteDirection.Up:
								directionY--;
								break;
							case NoteDirection.Down:
								directionY++;
								break;
							case NoteDirection.Left:
								directionX++;
								break;
							case NoteDirection.Right:
								directionX--;
								break;
						}

						float posX = current.position.x + (directionX * normalNoteSpeed * (currentTime - current.timeCode)) + 0.5f;
						float posY = current.position.y + (directionY * normalNoteSpeed * (currentTime - current.timeCode)) + 0.5f;

						if (posX >= boxSize.currentSize + 1.5 || posX <= 0.5 || posY >= boxSize.currentSize + 1.5 || posY <= 0.5) continue;

						if (disabledNoteParent.childCount > 0)
						{
							GameObject obj = disabledNoteParent.GetChild(0).gameObject;
							obj.GetComponent<Transform>().position = new Vector3(posX, posY, 0f);
							obj.GetComponent<SpriteRenderer>().sprite = boxSprites[(int)current.color];
							obj.transform.SetParent(noteParent);
							obj.SetActive(true);
						}
						else
						{
							Instantiate(boxNote, new Vector3(posX, posY, 0f), Quaternion.identity, noteParent)
								.GetComponent<SpriteRenderer>().sprite = boxSprites[(int)current.color];
						}

						break;
					case NoteType.Vertical:
						if (currentTime - current.timeCode > verticalTime) continue;

						if (current.direction == NoteDirection.Up || current.direction == NoteDirection.Down)
						{
							for (int i = 0; i < boxSize.currentSize; i++)
							{
								if (disabledNoteParent.childCount > 0)
								{
									GameObject obj = disabledNoteParent.GetChild(0).gameObject;
									obj.GetComponent<Transform>().position = new Vector3(current.position.x + 0.5f, i + 1.5f, 0);
									obj.GetComponent<SpriteRenderer>().sprite = boxSprites[(int)current.color];
									obj.transform.SetParent(noteParent);
									obj.SetActive(true);
								}
								else
								{
									Instantiate(boxNote, new Vector3(current.position.x + 0.5f, i + 1.5f, 0), Quaternion.identity, noteParent)
										.GetComponent<SpriteRenderer>().sprite = boxSprites[(int)current.color];
								}
							}
						}
						else
						{
							for (int i = 0; i < boxSize.currentSize; i++)
							{
								if (disabledNoteParent.childCount > 0)
								{
									GameObject obj = disabledNoteParent.GetChild(0).gameObject;
									obj.GetComponent<Transform>().position = new Vector3(i + 1.5f, current.position.y + 0.5f, 0);
									obj.GetComponent<SpriteRenderer>().sprite = boxSprites[(int)current.color];
									obj.transform.SetParent(noteParent);
									obj.SetActive(true);
								}
								else
								{
									Instantiate(boxNote, new Vector3(i + 1.5f, current.position.y + 0.5f, 0), Quaternion.identity, noteParent)
										.GetComponent<SpriteRenderer>().sprite = boxSprites[(int)current.color];
								}
							}
						}

						break;
					case NoteType.SizeChange:
						if (current.timeCode != currentTime) break; // 현재 타임 이전꺼는 이미 작업된 상태로 넘어옴
						boxSize.SetBoxSize(current.position.y);
						break;
					case NoteType.BlankS:
						if (current.timeCode != currentTime) break;
						boxSize.gameObject.SetActive(false);
						break;
					case NoteType.BlankE:
						if (current.timeCode != currentTime) break;
						boxSize.gameObject.SetActive(true);
						break;
					case NoteType.ImpactLine:
						break;
				}
			}

			List<Note> lateNotes = notes.FindAll(target => (currentTime <= target.timeCode && target.timeCode <= currentTime + aroundTime));
			foreach (Note current in lateNotes)
			{
				if (!(current.type == NoteType.Normal || current.type == NoteType.Vertical)) continue;

				TileBase noteTile = current.type == NoteType.Normal ? normalWarning : verticalWarning[(int)current.direction];
				int addX = 0;
				int addY = 0;

				switch (current.direction)
				{
					case NoteDirection.Up:
						addY++;
						break;
					case NoteDirection.Down:
						addY--;
						break;
					case NoteDirection.Left:
						addX--;
						break;
					case NoteDirection.Right:
						addX++;
						break;
				}

				noteWarningTilemap.SetTile(new Vector3Int(current.position.x + addX, current.position.y + addY, 0), noteTile);
			}
		}
	}
}
