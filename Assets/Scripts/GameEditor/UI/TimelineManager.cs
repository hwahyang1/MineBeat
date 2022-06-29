using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MineBeat.GameEditor.Song;

/*
 * [Namespace] Minebeat.GameEditor.UI
 * Desciption
 */
namespace MineBeat.GameEditor.UI
{
	/*
	 * [Class] TimelineManager
	 * 곡의 재생 정보를 표기합니다.
	 */
	public class TimelineManager : MonoBehaviour
	{
		[Header("Timeline")]
		[SerializeField]
		private Slider timeline;
		[SerializeField]
		private TMP_InputField currentTimeCode;
		[SerializeField]
		private TextMeshProUGUI endTimeCode;

		[Header("Buttons")]
		[SerializeField, Tooltip("버튼 GameObject를 PlayStatus에 맞게 배치합니다.")]
		private Button[] buttons = new Button[3];

		private GameObject[] hideOnPlayObjects;

		private SongManager songManager;

		private void Start()
		{
			hideOnPlayObjects = GameObject.FindGameObjectsWithTag("HideOnPlay");
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			// 이부분 코드 다시 짜야함

			for (int i = 0; i < 3; i++)
			{
				buttons[i].interactable = true;
			}

			buttons[(int)songManager.playStatus].interactable = false;

			for (int i = 0; i < hideOnPlayObjects.Length; i++)
			{
				hideOnPlayObjects[i].SetActive(songManager.playStatus == PlayStatus.Stopped);
			}

			switch (songManager.playStatus)
			{
				case PlayStatus.Playing:
					timeline.value = songManager.GetCurrentTime() / ConvertTimeCode(endTimeCode.text);
					break;
				case PlayStatus.Paused:
					break;
				case PlayStatus.Stopped:
					buttons[(int)PlayStatus.Paused].interactable = false;
					break;
			}
		}

		/*
		 * [Method] ConvertTimeCode(float length): string
		 * float 형태의 초를 문자열의 TimeCode로 반환합니다.
		 * 
		 * <float length>
		 * 길이를 초 단위로 받습니다.
		 * 
		 * <RETURN: string>
		 * 변환된 문자열 형태로 반환합니다.
		 */
		private string ConvertTimeCode(float length)
		{
			int front = Mathf.FloorToInt(length);
			float back = Mathf.FloorToInt((length - front) * 1000);

			int min = front / 60;
			int sec = front % 60;

			return string.Format("{0:00}:{1:00}:{2:000}", min, sec, back);
		}

		/*
		 * [Method] ConvertTimeCode(string code): float
		 * 문자열 형태의 TimeCode를 실수형의 초로 반환합니다.
		 * 
		 * <string code>
		 * 길이를 문자열로 받습니다.
		 * 
		 * <RETURN: float>
		 * 변환된 실수 형태로 반환합니다.
		 */
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
			catch (System.IndexOutOfRangeException) // 그냥 숫자만 때려넣은 경우
			{
				return float.Parse(code);
			}
		}

		/*
		 * [Method] UpdateAudioClip(): void
		 * AudioClip이 갱신되었을 때 정보를 갱신합니다.
		 */
		public void UpdateAudioClip()
		{
			currentTimeCode.text = "00:00:000";
			timeline.value = 0f;
			endTimeCode.text = ConvertTimeCode(songManager.audioClip.length);
		}

		/*
		 * [Method] OnCurrentTimeCodeEditEnded(): void
		 * CurrentTimeCode의 값이 변경되었을 때 정보를 갱신합니다. (입력이 완전히 끝나야 호출됩니다)
		 */
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

		/*
		 * [Method] OnTimelineChanged(): void
		 * Timeline의 값이 변경되었을 때 정보를 갱신합니다.
		 */
		public void OnTimelineChanged()
		{
			float currentValue = songManager.audioClip.length * timeline.value;

			currentTimeCode.text = ConvertTimeCode(currentValue);
			songManager.ChangeCurrentTime(currentValue);
		}
	}
}
