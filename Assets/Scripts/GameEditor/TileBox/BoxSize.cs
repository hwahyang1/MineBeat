using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * [Namespace] Minebeat.GameEditor.TileBox
 * Desciption
 */
namespace MineBeat.GameEditor.TileBox
{
	/*
	 * [Class] BoxSize
	 * 박스의 크기를 조정합니다.
	 */
	public class BoxSize : MonoBehaviour
	{
		[Header("박스 최대/최소 크기"), SerializeField]
		public int maxSize = 9;
		[SerializeField]
		public int minSize = 3;
		private int currentSize = 7;

		[Header("박스 크기 조정 시 사용되는 TileBase"), SerializeField]
		private TileBase boxTile;
		private Tilemap boxMap;

		[SerializeField]
		private TileBase gridTile;
		private Tilemap gridMap;

		[Header("UI"), SerializeField]
		private TextMeshProUGUI text;

		private Camera mainCamara;

		private void Start()
		{
			mainCamara = Camera.main;

			boxMap = transform.Find("Box").GetComponent<Tilemap>();
			gridMap = transform.Find("Box Grid").GetComponent<Tilemap>();

			DrawBox();
		}

		/*
		 * [Method] DrawBox(): void
		 * 상자를 다시 그립니다.
		 */
		public void DrawBox()
		{
			boxMap.ClearAllTiles();
			gridMap.ClearAllTiles();

			int drawSize = currentSize + 2;// boxSize 사이즈 기준이 안에 빈 공간이라 2 더해야함

			for (int i = 0; i < drawSize; i++)
			{
				for (int j = 0; j < drawSize; j++)
				{
					if (i == 0 || i == drawSize - 1) // 첫줄&막줄: 전부 boxTile로 그림
					{
						boxMap.SetTile(new Vector3Int(i, j, 0), boxTile);
					}
					else // 나머지: 양끝만 boxTile로 그리고 나머지는 gridTile로 그림
					{
						if (j == 0 || j == drawSize - 1)
						{
							boxMap.SetTile(new Vector3Int(i, j, 0), boxTile);
						}
						else
						{
							gridMap.SetTile(new Vector3Int(i, j, 0), gridTile);
						}
					}
				}
			}

			float boxCenter = drawSize / 2f;
			mainCamara.transform.position = new Vector3(boxCenter, boxCenter, -10f);

			text.text = currentSize.ToString();
		}

		/*
		 * [Method] ChangeBoxSizeUp(): void
		 * 상자의 크기를 키웁니다.
		 */
		public void ChangeBoxSizeUp()
		{
			if (currentSize == maxSize) return;
			currentSize++;
			DrawBox();
		}

		/*
		 * [Method] ChangeBoxSizeDown(): void
		 * 상자의 크기를 줄입니다.
		 */
		public void ChangeBoxSizeDown()
		{
			if (currentSize == minSize) return;
			currentSize--;
			DrawBox();
		}
	}
}