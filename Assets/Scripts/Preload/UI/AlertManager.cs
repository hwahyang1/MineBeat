using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MineBeat.Preload.UI
{
	/// <summary>
	/// 알림창의 생성과 버튼 이벤트를 관리합니다.
	/// </summary>
	public class AlertManager : Singleton<AlertManager>
	{
		/// <summary>
		/// 알림창 버튼의 갯수를 정의합니다.
		/// </summary>
		public enum AlertButtonType
		{
			None,
			Single,
			Double,
			Triple
		}

		[SerializeField]
		private GameObject canvas;
		[SerializeField]
		private GameObject parent;

		[Header("Text")]
		[SerializeField]
		private Text titleArea;
		[SerializeField]
		private Text descriptionArea;

		[Header("Button Group")]
		[SerializeField, Tooltip("AlertButtonType와 동일한 순서로 입력합니다.")]
		private GameObject[] buttonGroups = new GameObject[4];

		private bool _isActive = false;
		public bool isActive
		{
			get { return _isActive; }
			private set { _isActive = value; }
		}

		private AlertButtonType alertButtonType;
		private System.Action[] buttonActions;

		private void Start()
		{
			/*for (int i = 0; i < buttonGroups.Length; i++)
			{
				buttonGroups[i].SetActive(false);
			}
			parent.SetActive(false);*/
		}

		private void Update()
		{
			if (isActive)
			{
				canvas.SetActive(true);
				EventSystem.current.SetSelectedGameObject(null);

				if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
				{
					OnButtonClickedLeft();
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					switch (alertButtonType) // 버튼 순서: Yes - No - Cancel
					{
						case AlertButtonType.None:
							break;
						case AlertButtonType.Single:
							OnButtonClickedLeft();
							break;
						case AlertButtonType.Double:
							OnButtonClickedCenter();
							break;
						case AlertButtonType.Triple:
							OnButtonClickedRight();
							break;
					}
				}
			}
		}

		/// <summary>
		/// 알림창을 표시합니다.
		/// </summary>
		/// <param name="title">알림창의 제목을 입력합니다.</param>
		/// <param name="description">알림창의 설명을 입력합니다. 최대 글자수를 넘을 경우 자동으로 잘립니다.</param>
		/// <param name="buttonType">사용할 버튼의 타입을 입력합니다.</param>
		/// <param name="buttonTexts">버튼에 대한 텍스트를 입력합니다. 무조건 ButtonType과 수가 일치해야 하며, ButtonType.None일 경우, 1개의 빈 string을 입력합니다.</param>
		/// <param name="buttonActions">
		/// 버튼에 대한 이벤트를 입력합니다.
		/// 이벤트 입력 순서는 해당 ButtonGroup의 왼쪽부터입니다. (ex- Double을 선택 한 경우, Y, N 순서로 이벤트 입력) 무조건 ButtonType과 수가 일치해야 하며, ButtonType.None일 경우, null을 입력합니다.
		/// 아무 이벤트 없이 창을 닫게만 하고 싶을 경우, 해당되는 위치에 null을 입력합니다.
		/// </param>
		/// <returns>
		/// 오류 여부를 반환합니다.
		/// 0: 정상적으로 알림창이 표시 된 경우입니다.
		/// 1: 현재 다른 알림창이 열려 있는 경우입니다.
		/// 2: 입력값 오류입니다. ButtonType과 buttonActions, buttonTexts의 수가 정확히 일치하는지 확인하세요.
		/// </returns>
		public int Show(string title, string description, AlertButtonType buttonType, string[] buttonTexts, params System.Action[] buttonActions)
		{
			if (parent.activeInHierarchy) return 1;
			if ((buttonType == AlertButtonType.None && buttonActions.Length != 1) || (int)buttonType != buttonActions.Length) return 2;
			if ((buttonType == AlertButtonType.None && buttonTexts.Length != 1) || (int)buttonType != buttonTexts.Length) return 2;

			titleArea.text = title;
			descriptionArea.text = description;

			GameObject currentButtonGroup = buttonGroups[(int)buttonType];

			for (int i = 0; i < (int)buttonType; i++)
			{
				currentButtonGroup.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = buttonTexts[i];
			}

			currentButtonGroup.SetActive(true);
			parent.SetActive(true);
			isActive = true;

			alertButtonType = buttonType;
			this.buttonActions = buttonActions;

			return 0;
		}

		/// <summary>
		/// 알림창을 닫습니다.
		/// </summary>
		public void Close()
		{
			if (!parent.activeInHierarchy) return;

			buttonGroups[(int)alertButtonType].SetActive(false);
			parent.SetActive(false);
			canvas.SetActive(false);
			isActive = false;
		}

		/* Button Events */
		// 함수는 버튼 개수대로 Left -> Center -> Right를 사용합니다.
		public void OnButtonClickedLeft()
		{
			if ((int)alertButtonType < 1) return;
			buttonActions[0].Invoke();
			Close();
		}
		public void OnButtonClickedCenter()
		{
			if ((int)alertButtonType < 2) return;
			buttonActions[1].Invoke();
			Close();
		}
		public void OnButtonClickedRight()
		{
			if ((int)alertButtonType < 3) return;
			buttonActions[2].Invoke();
			Close();
		}
	}
}
