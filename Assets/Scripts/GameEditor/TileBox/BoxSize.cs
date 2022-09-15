using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.GameEditor.Song;
using MineBeat.GameEditor.Notes;

namespace MineBeat.GameEditor.TileBox
{
	/// <summary>
	/// 박스의 크기와 색상을 조정합니다.
	/// </summary>
	public class BoxSize : MonoBehaviour
	{
		[Header("박스 최대/최소 크기"), SerializeField]
		public int maxSize = 9;
		[SerializeField]
		public int minSize = 3;
		private int _currentSize = 7;
		public int currentSize
		{
			get { return _currentSize; }
			private set { _currentSize = value; }
		}

		private NoteColor _currentColor = NoteColor.White; // 얘는 나중에 작업 예정
		public NoteColor currentColor
		{
			get { return _currentColor; }
			private set { _currentColor = value; }
		}

		[Header("박스 크기 조정 시 사용되는 TileBase")]
		[SerializeField, Tooltip("DefineNote.NoteColor의 순서대로 TileBase를 입력합니다.")]
		private TileBase[] boxTiles = new TileBase[6];
		private Tilemap boxMap;

		[SerializeField]
		private TileBase gridTile;
		private Tilemap gridMap;

		[Header("UI"), SerializeField]
		private TMP_InputField textInput;

		private Camera mainCamara;
		private SongManager songManager;
		private NotesManager notesManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			boxMap = transform.Find("Box").GetComponent<Tilemap>();
			gridMap = transform.Find("Box Grid").GetComponent<Tilemap>();

			mainCamara = Camera.main;
			songManager = managers.Find(target => target.name == "SongManager").GetComponent<SongManager>();
			notesManager = managers.Find(target => target.name == "NoteManagers").GetComponent<NotesManager>();

			DrawBox();
		}

		/// <summary>
		/// 상자를 다시 그립니다.
		/// </summary>
		public void DrawBox()
		{
			boxMap.ClearAllTiles();
			gridMap.ClearAllTiles();

			int drawSize = currentSize + 2;
			for (int i = 0; i < drawSize; i++)
			{
				for (int j = 0; j < drawSize; j++)
				{
					if (i == 0 || i == drawSize - 1 ||
						j == 0 || j == drawSize - 1)
					{
						boxMap.SetTile(new Vector3Int(i, j, 0), boxTiles[(int)currentColor]);
					}
					else
					{
						gridMap.SetTile(new Vector3Int(i, j, 0), gridTile);
					}
				}
			}

			float boxCenter = drawSize / 2f;
			mainCamara.transform.position = new Vector3(boxCenter, boxCenter, -10f);

			textInput.text = currentSize.ToString();
		}

		/// <summary>
		/// 상자의 크기를 키웁니다.
		/// </summary>
		public void ChangeBoxSizeUp()
		{
			if (currentSize == maxSize) return;
			AddSizeChangeNote(songManager.GetCurrentTime(), currentSize++, currentSize);
		}

		/// <summary>
		/// 상자의 크기를 줄입니다.
		/// </summary>
		public void ChangeBoxSizeDown()
		{
			if (currentSize == minSize) return;
			AddSizeChangeNote(songManager.GetCurrentTime(), currentSize--, currentSize);
		}

		/// <summary>
		/// Input Field에 입력된 값으로 박스의 크기를 수정합니다.
		/// </summary>
		public void ChangeBoxSize()
		{
			int afterSize = textInput.text == "" ? 7 : int.Parse(textInput.text);
			if (currentSize == afterSize) return;
			AddSizeChangeNote(songManager.GetCurrentTime(), currentSize, afterSize);
		}

		/// <summary>
		/// 상자의 크기를 지정합니다.
		/// </summary>
		/// <param name="size">원하는 박스의 크기를 지정합니다.</param>
		public void SetBoxSize(int size)
		{
			if (size < minSize) size = minSize;
			if (maxSize < size) size = maxSize;
			currentSize = size;
			DrawBox();
		}

		/// <summary>
		/// NotesManager에 SizeChange 노트를 추가합니다.
		/// </summary>
		/// <param name="timeCode">SizeChange 노트를 추가할 위치를 입력합니다.</param>
		/// <param name="beforeSize">변경 전 크기를 입력합니다.</param>
		/// <param name="afterSize">변경 후 크기를 입력합니다.</param>
		private void AddSizeChangeNote(float timeCode, int beforeSize, int afterSize)
		{
			List<Note> duplicate = notesManager.Find(NoteType.SizeChange, timeCode);
			if (duplicate.Count == 0)
			{
				notesManager.Add(new Note(timeCode, NoteType.SizeChange, new NotePosition(beforeSize, afterSize)));
			}
			else
			{
				if (duplicate[0].position.y == afterSize) return; // 바꾼 이유가 없을 때

				notesManager.Remove(duplicate[0]);

				if (duplicate[0].position.x == afterSize) // 바꾸기 이전 크기로 돌아가려 할 경우
				{
					afterSize = duplicate[0].position.x;
				}
				else
				{
					notesManager.Add(new Note(timeCode, NoteType.SizeChange, new NotePosition(duplicate[0].position.x, afterSize)));
				}
			}

			//뒤에 노트 있는지 읽어야함
			List<Note> afterNotes = notesManager.GetList().FindAll(target => target.timeCode > timeCode && target.type == NoteType.SizeChange);

			//for (int i = 0; i < afterNotes.Count; i++)
			foreach (Note note in afterNotes)
			{
				/*
				 * 이번 노트 Y하고 다음 노트 X 비교 (같으면 return) -> 다르면 다음 노트 X를 이번 노트 Y로 대입 -> 다음 노트 X하고 Y하고 서로 비교 (다르면 return) -> 서로 같으면 없애고 그 다음 노트를 대상으로 다시 검증
				 */
				if (afterSize == afterNotes[0].position.x) return;
				afterNotes[0].position.x = (ushort)afterSize;
				if (afterNotes[0].position.x != afterNotes[0].position.y) return;
				notesManager.Remove(afterNotes[0]);
			}

			DrawBox();
		}
	}
}
