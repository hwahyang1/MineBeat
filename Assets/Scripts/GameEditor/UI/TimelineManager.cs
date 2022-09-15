using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MineBeat.GameEditor.Song;

namespace MineBeat.GameEditor.UI
{
	/// <summary>
	/// 곡의 재생 정보를 표기합니다.
	/// </summary>
	public class TimelineManager : MonoBehaviour
	{
		[Header("Timeline")]
		[SerializeField]
		private Slider timeline;
		[SerializeField]
		private TMP_InputField currentTimeCode;
		[SerializeField]
		private Text endTimeCode;

		[Header("Buttons")]
		[SerializeField, Tooltip("버튼 GameObject를 PlayStatus에 맞게 배치합니다.")]
		private Button[] buttons = new Button[3];

		/*[Header("Control")]
		[SerializeField]
		private float timelineMouseScrollSpeed;*/

		private bool hideUIOnPlay = false;

		private GameObject[] hideOnPlayObjects;

		private SongManager songManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			hideOnPlayObjects = GameObject.FindGameObjectsWithTag("HideOnPlay");
			songManager = managers.Find(target => target.name == "SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			for (int i = 0; i < 3; i++)
			{
				buttons[i].interactable = i != (int)songManager.playStatus;
			}

			if (hideUIOnPlay)
			{
				for (int i = 0; i < hideOnPlayObjects.Length; i++)
				{
					hideOnPlayObjects[i].SetActive(songManager.playStatus == PlayStatus.Stopped);
				}
			}

			if (songManager.playStatus == PlayStatus.Playing)
			{
				timeline.value = songManager.GetCurrentTime() / ConvertTimeCode(endTimeCode.text);
			}
			else if (songManager.playStatus == PlayStatus.Stopped)
			{
				buttons[(int)PlayStatus.Paused].interactable = false;
			}

			/*if (songManager.audioClip != null && Input.mouseScrollDelta.y != 0)
			{
				timeline.value = timeline.value * timelineMouseScrollSpeed * -Input.mouseScrollDelta.y;
			}*/
		}

		/// <summary>
		/// 'Hide UI while playing' 토글이 변경되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnHideUIWhenPlayToggleChanged()
		{
			hideUIOnPlay = !hideUIOnPlay;
		}

		/// <summary>
		/// float 형태의 초를 문자열의 TimeCode로 반환합니다.
		/// </summary>
		/// <param name="length">길이를 초 단위로 받습니다.</param>
		/// <returns>변환된 문자열 형태로 반환합니다.</returns>
		private string ConvertTimeCode(float length)
		{
			int front = Mathf.FloorToInt(length);
			float back = Mathf.FloorToInt((length - front) * 1000);

			int min = front / 60;
			int sec = front % 60;

			return string.Format("{0:00}:{1:00}:{2:000}", min, sec, back);
		}

		/// <summary>
		/// 문자열 형태의 TimeCode를 실수형의 초로 반환합니다.
		/// </summary>
		/// <param name="code">길이를 문자열로 받습니다.</param>
		/// <returns>변환된 실수 형태로 반환합니다.</returns>
		private float ConvertTimeCode(string code)
		{
			try
			{
				string[] split = code.Split(":");

				float min = float.Parse(split[0]);
				float sec = float.Parse(split[1]);
				float milisec = float.Parse(split[2]);

				return min * 60f + sec + milisec / 1000f;
			}
			catch (System.IndexOutOfRangeException) // 그냥 숫자만 때려넣은 경우 당연히 Split이 안됨
			{
				return float.Parse(code); // 시도해보고 안되면 말고
			}
		}

		/// <summary>
		/// AudioClip이 갱신되었을 때 정보를 갱신합니다.
		/// </summary>
		///
		public void UpdateAudioClip()
		{
			currentTimeCode.text = "00:00:000";
			timeline.value = 0f;
			endTimeCode.text = ConvertTimeCode(songManager.audioClip.length);
		}

		/// <summary>
		/// CurrentTimeCode의 값이 변경되었을 때 정보를 갱신합니다. (입력이 완전히 끝나야 호출됩니다)
		/// </summary>
		public void OnCurrentTimeCodeEditEnded()
		{
			float targetTimeCode = ConvertTimeCode(currentTimeCode.text);
			float maxTimeCode = ConvertTimeCode(endTimeCode.text);

			if (targetTimeCode > maxTimeCode)
			{
				targetTimeCode = maxTimeCode;
			}

			currentTimeCode.text = ConvertTimeCode(targetTimeCode);
			timeline.value = targetTimeCode / maxTimeCode;
		}
		
		/// <summary>
		/// Timeline의 값이 변경되었을 때 정보를 갱신합니다.
		/// </summary>
		public void OnTimelineChanged()
		{
			float currentValue = songManager.audioClip.length * timeline.value;

			currentTimeCode.text = ConvertTimeCode(currentValue);
			songManager.ChangeCurrentTime(currentValue);
		}

		/// <summary>
		/// 현재 TimeCode를 지정한 값으로 바꿉니다.
		/// </summary>
		/// <param name="timeCode">바꾸길 원하는 시간 값을 초 단위로 입력합니다.</param>
		public void ChangeTimeCode(float timeCode)
		{
			float maxTimeCode = ConvertTimeCode(endTimeCode.text);

			if (timeCode > maxTimeCode)
			{
				timeCode = maxTimeCode;
			}
			else if (timeCode < 0)
			{
				timeCode = 0;
			}

			currentTimeCode.text = ConvertTimeCode(timeCode);
			timeline.value = timeCode / maxTimeCode;
		}
	}
}
