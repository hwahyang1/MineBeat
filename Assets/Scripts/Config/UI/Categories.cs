using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.Config.Selection;
using MineBeat.Config.UI.Elements;

/*
 * [Namespace] MineBeat.Config.UI
 * Description
 */
namespace MineBeat.Config.UI
{
	/*
	 * [Class] Categories
	 * 카테고리/설명란의 UI 표출을 관리합니다.
	 */
	public class Categories : MonoBehaviour
	{
		[SerializeField, Tooltip("CategorySelectionManager.Category 순서대로 카테고리 Element GameObject를 입력합니다.")]
		private List<GameObject> categoryObjects = new List<GameObject>();
		[SerializeField, Tooltip("CategorySelectionManager.Category 순서대로 카테고리 Content GameObject를 입력합니다.")]
		private List<GameObject> categoryElements = new List<GameObject>();

		[SerializeField, Tooltip("Top, Middle, Bottom 순서대로 선택되지 않았을 때의 배경 Sprite를 입력합니다.")]
		private List<Sprite> unselectedImages = new List<Sprite>();
		[SerializeField, Tooltip("Top, Middle, Bottom 순서대로 선택되었을 때의 배경 Sprite를 입력합니다.")]
		private List<Sprite> selectedImages = new List<Sprite>();

		[SerializeField]
		private Color unselectedTextColor = new Color(1f, 1f, 1f, 1f);
		[SerializeField]
		private Color selectedTextColor = new Color(1f, 1f, 1f, 1f);

		private Category currentCategory = Category.Graphic;

		private CategorySelectionManager categorySelectionManager;

		private void Start()
		{
			categorySelectionManager = gameObject.GetComponent<CategorySelectionManager>();
		}

		private void Update()
		{
			if (currentCategory != categorySelectionManager.selectedCategory)
			{
				currentCategory = categorySelectionManager.selectedCategory;

				for (int i = 0; i < categoryObjects.Count; i++)
				{
					int currentOrder = (i == categoryObjects.Count - 1) ? 2 : (i == 0) ? 0 : 1;
					categoryObjects[i].GetComponent<Image>().sprite = (int)currentCategory == i ? selectedImages[currentOrder] : unselectedImages[currentOrder];
					categoryObjects[i].transform.GetChild(0).GetComponent<Text>().color = (int)currentCategory == i ? selectedTextColor : unselectedTextColor;

					categoryElements[i].SetActive((int)currentCategory == i);
				}
			}

			Vector3 mPosition = Input.mousePosition; //마우스의 스크린 좌표를 입력받는다.
			Vector3 target = Camera.main.ScreenToWorldPoint(mPosition); //입력 받은 마우스의 좌표값을 월드좌표로 변환
			Instantiate(obj, target, Quaternion.identity); //변환한 위치에 원하는 오브젝트를 생성
		}
	}
}
