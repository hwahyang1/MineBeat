using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Namespace] MineBeat.Config.UI.Elements
 * Description
 */
namespace MineBeat.Config.UI.Elements
{
	/*
	 * [Class] CategoryElement
	 * 선택형 항목 - 카테고리 각 항목을 관리합니다.
	 */
	public abstract class SelectCategoryElement : CategoryElement
	{
		[SerializeField]
		protected List<string> selections = new List<string>(); // 사용 가능한 선택지들
		protected int currentSelection = 0;

		protected GameObject elementName;
		protected GameObject elementLeftArrow;
		protected GameObject elementSelected;
		protected GameObject elementRightArrow;

		protected override void Awake()
		{
			base.Awake();

			elementName = transform.GetChild(0).gameObject;
			elementLeftArrow = transform.GetChild(1).gameObject;
			elementSelected = transform.GetChild(2).gameObject;
			elementRightArrow = transform.GetChild(3).gameObject;

			elementLeftArrow.GetComponent<Button>().onClick.AddListener(OnLeftArrowClicked);
			elementRightArrow.GetComponent<Button>().onClick.AddListener(OnRightArrowClicked);
		}

		/// <summary>
		/// 해당 Method는 currentSelection 변수를 기준으로 UI를 업데이트 시킵니다.
		/// 모든 작업이 완료된 후 base.Start()를 호출합니다.
		/// </summary>
		protected virtual void Start()
		{
			OnValueChanged();
		}

		/*
		 * [Method] OnValueChanged(): void
		 * 선택 값이 바뀌었을 때 이벤트를 처리합니다.
		 */
		protected virtual void OnValueChanged()
		{
			elementSelected.GetComponent<Text>().text = selections[currentSelection];
			elementRightArrow.GetComponent<Button>().interactable = currentSelection != selections.Count - 1;
			elementLeftArrow.GetComponent<Button>().interactable = currentSelection != 0;
			StartCoroutine(UpdateLeftArrow());
		}

		public IEnumerator UpdateLeftArrow()
		{
			yield return new WaitForEndOfFrame();
			RectTransform rectTransform = elementLeftArrow.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(-100 - elementSelected.GetComponent<RectTransform>().rect.width, rectTransform.anchoredPosition.y);
		}

		/*
		 * [Method] OnLeftArrowClicked(): void
		 * [Method] OnRightArrowClicked(): void
		 * 각 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public virtual void OnLeftArrowClicked()
		{
			if (currentSelection == 0) return;
			currentSelection -= 1;
			OnValueChanged();
		}
		public virtual void OnRightArrowClicked()
		{
			if (currentSelection == selections.Count - 1) return;
			currentSelection += 1;
			OnValueChanged();
		}
	}
}
