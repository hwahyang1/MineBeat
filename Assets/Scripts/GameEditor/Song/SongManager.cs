using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] Minebeat.GameEditor.Song
 * Desciption
 */
namespace MineBeat.GameEditor.Song
{
	/*
	 * [Enum] PlayStatus
	 * 곡의 재생 여부를 관리합니다.
	 */
	public enum PlayStatus
	{
		Playing,
		Paused,
		Stopped
	}

	/*
	 * [Class] SongManager
	 * 곡의 재생을 관리합니다.
	 */
	[RequireComponent(typeof(AudioSource))]
	public class SongManager : MonoBehaviour
	{
		[HideInInspector]
		public AudioClip audioClip;
		[HideInInspector]
		public PlayStatus playStatus = PlayStatus.Stopped;

		private AudioSource audioSource;

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
		}

		/*
		 * [Method] GetCurrentTime(): float
		 * AudioSource의 playback time을 반환합니다.
		 * 
		 * <RETURN: float>
		 * 현재 재생중인 위치를 초로 반환합니다.
		 */
		public float GetCurrentTime()
		{
			return audioSource.time;
		}

		/*
		 * [Method] ChangeCurrentTime(float time): void
		 * AudioSource의 playback time을 수정합니다.
		 * 
		 * <float time>
		 * 수정을 원하는 위치를 초로 입력합니다.
		 */
		public void ChangeCurrentTime(float time)
		{
			audioSource.time = time;
		}

		/*
		 * [Method] OnPlayButtonClicked(): void
		 * 재생 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnPlayButtonClicked()
		{
			audioSource.Play();

			playStatus = PlayStatus.Playing;
		}

		/*
		 * [Method] OnPauseButtonClicked(): void
		 * 일시정지 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnPauseButtonClicked()
		{
			audioSource.Pause();

			playStatus = PlayStatus.Paused;
		}

		/*
		 * [Method] OnStopButtonClicked(): void
		 * 정지 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnStopButtonClicked()
		{
			audioSource.Pause();

			playStatus = PlayStatus.Stopped;
		}
	}
}
