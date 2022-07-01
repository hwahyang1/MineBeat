using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.GameEditor.UI;

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
		/*
		 * [Enum] NoteDirection
		 * 박스를 기준으로 노트에 어느 방향에 존재하는지 표현합니다.
		 */
		enum NoteDirection
		{
			Up,
			Down,
			Left,
			Right,
			None
		}

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

		private void Update()
		{
			previewTilemap.ClearAllTiles();

			if (toolBarInteract.currentObject == ObjectType.None) return;

			TileBase selectedSprite = toolBarInteract.currentObject == ObjectType.Normal ? normalNote : verticalNote[(int)noteDirection];

			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int posX = Mathf.FloorToInt(worldPosition.x);
			int posY = Mathf.FloorToInt(worldPosition.y);
			
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

			previewTilemap.SetTile(new Vector3Int(posX, posY, 0), selectedSprite);
		}
	}
}
