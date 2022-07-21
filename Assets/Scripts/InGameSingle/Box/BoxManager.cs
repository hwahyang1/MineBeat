using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * [Namespace] MineBeat.InGameSingle.Box
 * Description
 */
namespace MineBeat.InGameSingle.Box
{
	/*
	 * [Class] BoxManager
	 * 박스의 크기와 색상을 제어합니다.
	 */
	public class BoxManager : MonoBehaviour
	{
		[SerializeField]
		private Tilemap boxTilemap;
		[SerializeField]
		private Tilemap gridTilemap;

		[SerializeField]
		private TileBase gridTile;
		[SerializeField, Header("DefineNote.NoteColor를 기준으로 입력합니다.")]
		private TileBase[] boxes;

		private int _size = 7;
		public int size
		{
			get { return _size; }
			private set { _size = value; }
		}

		private NoteColor _color = NoteColor.White;
		public NoteColor color
		{
			get { return _color; }
			private set { _color = value; }
		}

		private void Start()
		{
			Draw(size);
		}

		/*
		 * [Method] Draw(int boxSize, NoteColor noteColor = NoteColor.White): void
		 * 박스를 그리고 플레이어와 카메라를 중앙으로 위치시킵니다.
		 * 
		 * <int boxSize>
		 * 박스의 크기를 입력합니다.
		 * 
		 * <NoteColor noteColor = NoteColor.White>
		 * 박스의 색상을 입력합니다.
		 */
		public void Draw(int boxSize, NoteColor noteColor = NoteColor.White)
		{
			boxTilemap.ClearAllTiles();
			gridTilemap.ClearAllTiles();

			int drawSize = boxSize + 2;
			for (int i = 0; i < drawSize; i++)
			{
				for (int j = 0; j < drawSize; j++)
				{
					if (i == 0 || i == drawSize - 1 ||
						j == 0 || j == drawSize - 1)
					{
						boxTilemap.SetTile(new Vector3Int(i, j, 0), boxes[(int)noteColor]);
					}
					else
					{
						gridTilemap.SetTile(new Vector3Int(i, j, 0), gridTile);
					}
				}
			}

			float boxCenter = drawSize / 2f;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(boxCenter, boxCenter, 0f);
			Camera.main.transform.position = new Vector3(boxCenter, boxCenter, -10f);

			size = boxSize;
			color = noteColor;
		}

		/*
		 * [Method] ChangeVisibility(bool isVisible): void
		 * 박스의 활성 여부를 결정합니다.
		 * 
		 * <bool isVisible>
		 * 활성 여부를 입력합니다.
		 */
		public void ChangeVisibility(bool isVisible)
		{
			boxTilemap.transform.parent.gameObject.SetActive(isVisible);
		}
	}
}
