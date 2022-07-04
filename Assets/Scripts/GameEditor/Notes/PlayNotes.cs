using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.GameEditor.Song;
using MineBeat.GameEditor.TileBox;

/*
 * [Namespace] Minebeat.GameEditor.Notes
 * Desciption
 */
namespace MineBeat.GameEditor.Notes
{
	/*
	 * [Class] PlayNotes
	 * TimeCode에 따른 노트의 위치를 계산하고, 노트를 배치 및 제거합니다.
	 */
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
		private GameObject whiteBox;

		[Header("Parent")]
		[SerializeField]
		private Transform noteParent;

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
			boxSize = GameObject.Find("Tilemaps").GetComponent<BoxSize>();
			notesManager = gameObject.GetComponent<NotesManager>();
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
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

		/*
		 * [Method] Draw(): void
		 * 화면상에 표기되는 노트를 다시 그립니다.
		 */
		public void Draw(List<Note> notes, float currentTime)
		{
			for (int i = 0; i < noteParent.childCount; i++)
			{
				Destroy(noteParent.GetChild(i).gameObject);
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

						Instantiate(whiteBox, new Vector3(posX, posY, 0f), Quaternion.identity, noteParent);

						break;
					case NoteType.Vertical:
						if (currentTime - current.timeCode > verticalTime) continue;

						if (current.direction == NoteDirection.Up || current.direction == NoteDirection.Down)
						{
							for (int i = 0; i < boxSize.currentSize; i++)
							{
								Instantiate(whiteBox, new Vector3(current.position.x + 0.5f, i + 1.5f, 0), Quaternion.identity, noteParent);
							}
						}
						else
						{
							for (int i = 0; i < boxSize.currentSize; i++)
							{
								Instantiate(whiteBox, new Vector3(i + 1.5f, current.position.y + 0.5f, 0), Quaternion.identity, noteParent);
							}
						}

						break;
					case NoteType.SizeChange:
						if (current.timeCode != currentTime) break; // 이전꺼는 이미 작업됨
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
