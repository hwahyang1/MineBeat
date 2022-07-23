using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.UI;
using MineBeat.Preload.Scene;

/*
 * [Namespace] MineBeat.Config.KeyInput
 * Description
 */
namespace MineBeat.Config.KeyInput
{
	/*
	 * [Class] HotKeyInputManager
	 * 단축키 입력 이벤트를 처리합니다.
	 */
	public class HotKeyInputManager : MonoBehaviour
	{
		private bool active = true;

		private void Update()
		{
			if (!active) return;

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				OnEscaapeKeyPressed();
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				OnUpArrowKeyPressed();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				OnDownArrowKeyPressed();
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				OnLeftArrowKeyPressed();
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				OnRightArrowKeyPressed();
			}
		}

		/*
		 * [Method] OnEscaapeKeyPressed(): void
		 * [Method] OnUpArrowKeyPressed(): void
		 * [Method] OnDownArrowKeyPressed(): void
		 * [Method] OnLeftArrowKeyPressed(): void
		 * [Method] OnRightArrowKeyPressed(): void
		 * 각 키의 입력 이벤트를 처리합니다.
		 */
		public void OnEscaapeKeyPressed()
		{
			AlertManager.Instance.Show("확인", "모드 선택 화면으로 돌아갈까요?", AlertManager.AlertButtonType.Double, new string[] { "예", "아니요" }, () => { SceneChange.Instance.ChangeScene("ModeSelectScene"); }, () => { StartCoroutine(ToggleTwiceActiveDelay()); });
		}
		public void OnUpArrowKeyPressed()
		{

		}
		public void OnDownArrowKeyPressed()
		{

		}
		public void OnLeftArrowKeyPressed()
		{

		}
		public void OnRightArrowKeyPressed()
		{

		}

		/*
		 * [Coroutine] ToggleTwiceActiveDelay(): IEnumerator
		 * 잠깐의 시간을 두고 active 변수의 값을 총 2번 반전시킵니다.
		 * (active 값 반전 -> 일정 시간 대기 -> active 값 반전)
		 */
		public IEnumerator ToggleTwiceActiveDelay()
		{
			active = !active;
			yield return new WaitForSeconds(0.1f);
			active = !active;
		}
	}
}
