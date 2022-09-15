using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.Song;

using MineBeat.Preload.UI;
using MineBeat.Preload.Scene;

namespace MineBeat.SongSelectSingle.KeyInput
{
	/// <summary>
	/// 단축키 입력을 받습니다.
	/// </summary>
	public class HotKeyInputManager : MonoBehaviour
	{
		[SerializeField]
		private float arrowCool = 0.25f;
		private float currentCool = 0f;

		private bool active = true;

		private SongManager songManager;
		private PreviewSong previewSong;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			songManager = managers.Find(target => target.name == "SongManager").GetComponent<SongManager>();
			previewSong = managers.Find(target => target.name == "SongManager").GetComponent<PreviewSong>();
		}

		private void Update()
		{
			if (currentCool < arrowCool) currentCool += Time.deltaTime;

			if (active && !AlertManager.Instance.isActive)
			{
				if (Input.GetKey(KeyCode.UpArrow) && currentCool >= arrowCool)
				{
					OnUpArrowKeyClicked();
					currentCool = 0f;
				}
				else if (Input.GetKey(KeyCode.DownArrow) && currentCool >= arrowCool)
				{
					OnDownArrowKeyClicked();
					currentCool = 0f;
				}
				else if (Input.GetKeyDown(KeyCode.Escape))
				{
					OnEscapeKeyClicked();
				}
				else if (Input.GetKeyDown(KeyCode.F10))
				{
					OnF10KeyClicked();
				}
				else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
				{
					OnEnterKeyClicked();
				}
			}
		}

		/// <summary>
		/// Scene을 변경합니다.
		/// </summary>
		/// <param name="sceneName">변경할 Scene 이름을 지정합니다.</param>
		public void ChangeScene(string sceneName)
		{
			previewSong.forceFadeout = true;
			active = false;
			SceneChange.Instance.ChangeScene(sceneName);
		}

		/// <summary>
		/// 잠깐의 시간을 두고 active 변수의 값을 총 2번 반전시킵니다.
		/// </summary>
		/// <returns>(active 값 반전 -> 일정 시간 대기 -> active 값 반전)</returns>
		public IEnumerator ToggleTwiceActiveDelay()
		{
			active = !active;
			yield return new WaitForSeconds(0.1f);
			active = !active;
		}

		/* Key Events */
		public void OnUpArrowKeyClicked()
		{
			songManager.SelectedUp();
		}
		public void OnDownArrowKeyClicked()
		{
			songManager.SelectedDown();
		}
		public void OnEscapeKeyClicked()
		{
			AlertManager.Instance.Show("확인", "모드 선택 화면으로 돌아갈까요?", AlertManager.AlertButtonType.Double, new string[] { "예", "아니요" }, () => { ChangeScene("ModeSelectScene"); }, () => { StartCoroutine(ToggleTwiceActiveDelay()); });
		}
		public void OnF10KeyClicked()
		{
			AlertManager.Instance.Show("확인", "게임 설정 화면으로 이동할까요?", AlertManager.AlertButtonType.Double, new string[] { "예", "아니요" }, () => { ChangeScene("ConfigScene"); }, () => { StartCoroutine(ToggleTwiceActiveDelay()); });
		}
		public void OnEnterKeyClicked()
		{
			songManager.Enter();
		}
	}
}
