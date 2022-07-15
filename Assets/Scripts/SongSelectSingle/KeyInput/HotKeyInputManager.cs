using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.Song;

using MineBeat.Preload.UI;
using MineBeat.Preload.Scene;

/*
 * [Namespace] MineBeat.SongSelectSingle.KeyInput
 * Description
 */
namespace MineBeat.SongSelectSingle.KeyInput
{
	/*
	 * [Class] HotKeyInputManager
	 * 단축키 입력을 받습니다.
	 */
	public class HotKeyInputManager : MonoBehaviour
	{
		[SerializeField]
		private float arrowCool = 0.25f;
		private float currentCool = 0f;

		private bool active = true;

		private SongManager songManager;

		private void Start()
		{
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			if (currentCool < arrowCool) currentCool += Time.deltaTime;

			if (active && !AlertManager.Instance.isActive)
			{
				if (Input.GetKey(KeyCode.UpArrow) && currentCool >= arrowCool)
				{
					songManager.SelectedUp();
					currentCool = 0f;
				}
				else if (Input.GetKey(KeyCode.DownArrow) && currentCool >= arrowCool)
				{
					songManager.SelectedDown();
					currentCool = 0f;
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					AlertManager.Instance.Show("확인", "모드 선택 화면으로 돌아갈까요?", AlertManager.AlertButtonType.Double, new string[] { "예", "아니요" }, () => { ChangeScene("ModeSelectScene"); }, () => { StartCoroutine(ToggleTwiceActiveDelay()); });
				}
				else if (Input.GetKeyDown(KeyCode.F10))
				{
					ChangeScene("ConfigScene");
				}
				else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
				{
					//
				}
			}
		}

		/*
		 * [Method] ChangeScene(string sceneName): void
		 * Scene을 변경합니다.
		 * 
		 * <string sceneName>
		 * 변경할 Scene 이름을 지정합니다.
		 */
		public void ChangeScene(string sceneName)
		{
			active = false;
			SceneChange.Instance.ChangeScene(sceneName);
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
