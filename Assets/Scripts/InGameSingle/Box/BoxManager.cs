using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace MineBeat.InGameSingle.Box
{
	/// <summary>
	/// 박스의 크기와 색상을 제어합니다.
	/// </summary>
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

		public int Size { get; private set; } = 7;

		public NoteColor Color { get; private set; } = NoteColor.White;

		private void Start()
		{
			Draw(Size);
		}

		/// <summary>
		/// 박스를 다시 그리고 플레이어와 카메라를 중앙으로 위치시킵니다.
		/// </summary>
		/// <param name="boxSize">박스의 크기를 입력합니다.</param>
		/// <param name="noteColor">박스의 색상을 입력합니다.</param>
		public void Draw(int boxSize, NoteColor noteColor = NoteColor.White)
		{
			boxTilemap.ClearAllTiles();
			gridTilemap.ClearAllTiles();

			int drawSize = boxSize + 2; // 박스 크기는 경계선을 제외하고 입력하게 되어있음
			for (int i = 0; i < drawSize; i++)
			{
				for (int j = 0; j < drawSize; j++)
				{
					if (i == 0 || i == drawSize - 1 || // 가로 첫줄/끝줄
						j == 0 || j == drawSize - 1) // 세로 첫줄/끝줄
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
			Camera.main.transform.position = new Vector3(boxCenter, boxCenter, -10f); // 그냥 냅두면 Lerp때문에 플레이에 방해가 될 수도 있음

			Size = boxSize;
			Color = noteColor;
		}

		/// <summary>
		/// 박스의 활성 여부를 결정합니다.
		/// </summary>
		/// <param name="isVisible">활성 여부를 입력합니다.</param>
		/// <param name="movePlayer">플레이어와 카메라를 중앙으로 위치시킬지 여부를 입력합니다.</param>
		public void ChangeVisibility(bool isVisible, bool movePlayer)
		{
			boxTilemap.transform.parent.gameObject.SetActive(isVisible);

			if (movePlayer)
			{
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				player.SetActive(true);
				float boxCenter = (Size + 2) / 2f;
				player.GetComponent<Transform>().position = new Vector3(boxCenter, boxCenter, 0f);
				Camera.main.transform.position = new Vector3(boxCenter, boxCenter, -10f);
			}
		}
	}
}
