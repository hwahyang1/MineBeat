using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * [Namespace] Minebeat.GameEditor
 * Desciption
 */
namespace MineBeat.GameEditor
{
	/*
	 * [Class] BoxSizeManager
	 * 박스 크기의 조절을 관리합니다.
	 */
	public class BoxSizeManager : MonoBehaviour
	{
		[Header("박스 최대/최소 크기"), SerializeField]
		public int maxSize = 9;
		[SerializeField]
		public int minSize = 3;
		private int boxSize = 7;

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

			DrawBox(true);
		}

		/*
		 * [Method] DrawBox(bool changeCameraPosition): void
		 * 상자를 다시 그립니다.
		 * 
		 * <bool changeCameraPosition>
		 * 상자를 다시 그린 이후 카메라를 중앙에 위치할지 여부를 결정합니다.
		 */
		public void DrawBox(bool changeCameraPosition)
		{
			boxMap.ClearAllTiles();
			gridMap.ClearAllTiles();

			int drawSize = boxSize + 2;// boxSize 사이즈 기준이 안에 빈 공간이라 2 더해야함

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

			if (changeCameraPosition)
			{
				float boxCenter = drawSize / 2f;
				mainCamara.transform.position = new Vector3(boxCenter, boxCenter, -10f);
			}

			text.text = boxSize.ToString();
		}

		/*
		 * [Method] ChangeBoxSizeUp(): void
		 * 상자의 크기를 키웁니다.
		 * 
		 * <bool changeCameraPosition>
		 * 상자의 크기를 줄인 이후 카메라를 중앙에 위치할지 여부를 결정합니다.
		 */
		public void ChangeBoxSizeUp(bool changeCameraPosition)
		{
			if (boxSize == maxSize) return;
			boxSize++;
			DrawBox(changeCameraPosition);
		}

		/*
		 * [Method] ChangeBoxSizeDown(): void
		 * 상자의 크기를 줄입니다.
		 * 
		 * <bool changeCameraPosition>
		 * 상자의 크기를 줄인 이후 카메라를 중앙에 위치할지 여부를 결정합니다.
		 */
		public void ChangeBoxSizeDown(bool changeCameraPosition)
		{
			if (boxSize == minSize) return;
			boxSize--;
			DrawBox(changeCameraPosition);
		}
	}
}
