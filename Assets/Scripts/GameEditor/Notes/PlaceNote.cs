using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.GameEditor.UI;
using MineBeat.GameEditor.Song;

/*
 * [Namespace] Minebeat.GameEditor.Notes
 * Desciption
 */
namespace MineBeat.GameEditor.Notes
{
	/*
	 * [Class] PlaceNote
	 * 노트 배치에 대한 미리보기를 제공하고 노트 배치를 관리합니다.
	 */
	public class PlaceNote : MonoBehaviour
	{
		[SerializeField]
		private ToolBarInteract toolBarInteract;

		[Header("Tilemap")]
		[SerializeField]
		private Tilemap boxTilemap;
		[SerializeField]
		private Tilemap boxGridTilemap;
		[SerializeField]
		private Tilemap previewTilemap;
		[SerializeField]
		private Sprite whiteBox;
		[SerializeField]
		private Sprite boxGrid;
		[SerializeField]
		private TileBase normalNote;
		[SerializeField, Tooltip("노트의 위치를 기준으로 상, 하, 좌, 우에서 보일 TileBase를 입력하세요.")]
		private TileBase[] verticalNote = new TileBase[4];

		private NoteDirection noteDirection = NoteDirection.None;

		private NotesManager notesManager;
		private SongManager songManager;

		private void Start()
		{
			notesManager = gameObject.GetComponent<NotesManager>();
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			previewTilemap.ClearAllTiles();

			if (toolBarInteract.currentObject == ObjectType.None) return;

			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int posX = Mathf.FloorToInt(worldPosition.x);
			int posY = Mathf.FloorToInt(worldPosition.y);
			
			// 노트가 박스 기준으로 어디에 위치 할 것인지
			if (boxGridTilemap.GetSprite(new Vector3Int(posX, posY, 0)) != boxGrid && boxTilemap.GetSprite(new Vector3Int(posX, posY, 0)) != whiteBox)
			{
				if (boxTilemap.GetSprite(new Vector3Int(posX, posY - 1, 0)) == whiteBox)
				{
					noteDirection = NoteDirection.Up;
				}
				else if (boxTilemap.GetSprite(new Vector3Int(posX, posY + 1, 0)) == whiteBox)
				{
					noteDirection = NoteDirection.Down;
				}
				else if (boxTilemap.GetSprite(new Vector3Int(posX + 1, posY, 0)) == whiteBox)
				{
					noteDirection = NoteDirection.Left;
				}
				else if (boxTilemap.GetSprite(new Vector3Int(posX - 1, posY, 0)) == whiteBox)
				{
					noteDirection = NoteDirection.Right;
				}
				else
				{
					noteDirection = NoteDirection.None;
					return;
				}
			}
			else
			{
				noteDirection = NoteDirection.None;
				return;
			}

			TileBase selectedSprite = toolBarInteract.currentObject == ObjectType.Normal ? normalNote : verticalNote[(int)noteDirection];

			previewTilemap.SetTile(new Vector3Int(posX, posY, 0), selectedSprite);

			if (Input.GetMouseButtonDown(0))
			{
				NotePosition position = new NotePosition(posX, posY);

				switch (noteDirection)
				{
					case NoteDirection.Up:
						position.y -= 1;
						break;
					case NoteDirection.Down:
						position.y += 1;
						break;
					case NoteDirection.Left:
						position.x += 1;
						break;
					case NoteDirection.Right:
						position.x -= 1;
						break;
				}

				notesManager.Add(new Note(songManager.GetCurrentTime(), (NoteType)toolBarInteract.currentObject, NoteColor.WHITE, position, noteDirection));
				notesManager.SortList();
			}
		}
	}
}
