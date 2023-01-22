using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using MineBeat.GameEditor.Notes;

using MineBeat.Preload.UI;

namespace MineBeat.GameEditor.Song
{
	/// <summary>
	/// 곡의 재생 여부를 관리합니다.
	/// </summary>
	public enum PlayStatus
	{
		Playing,
		Paused,
		Stopped
	}

	/// <summary>
	/// 곡의 재생을 관리합니다.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class SongManager : MonoBehaviour
	{
		[HideInInspector]
		public AudioClip audioClip;
		[ReadOnly]
		public PlayStatus playStatus = PlayStatus.Stopped;

		private bool doNotPlayWhenError = true;

		private NotesVerifier notesVerifier;
		private AlertManager alertManager;
		private GameManager gameManager;
		private AudioSource audioSource;


		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			notesVerifier = managers.Find(target => target.name == "NoteManagers").GetComponent<NotesVerifier>();
			alertManager = managers.Find(target => target.name == "PreloadScene Managers").GetComponent<AlertManager>();
			gameManager = managers.Find(target => target.name == "GameManager").GetComponent<GameManager>();
			audioSource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (gameManager.blockInput) return;

			if (Input.GetKeyDown(KeyCode.Space))
			{
				switch (playStatus)
				{
					case PlayStatus.Playing:
						OnPauseButtonClicked();
						break;
					case PlayStatus.Paused:
					case PlayStatus.Stopped:
						OnPlayButtonClicked();
						break;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				OnStopButtonClicked();
			}
		}

		/// <summary>
		/// AudioSource의 playback time을 반환합니다.
		/// </summary>
		/// <returns>현재 재생중인 위치를 초로 반환합니다.</returns>
		public float GetCurrentTime()
		{
			return audioSource.time;
		}

		/// <summary>
		/// AudioSource의 playback time을 수정합니다.
		/// </summary>
		/// <param name="time">수정을 원하는 위치를 초로 입력합니다.</param>
		public void ChangeCurrentTime(float time)
		{
			audioSource.time = time;
		}

		/// <summary>
		/// 재생 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnPlayButtonClicked()
		{
			if (notesVerifier.IsError && doNotPlayWhenError)
			{
				alertManager.Show("알림", "\"노트 배치에 문제가 있어 재생을 할 수 없습니다.\n문제를 수정하거나 '에러가 있으면 재생하지 않기'를 끄고 다시 시도 해주세요.", AlertManager.AlertButtonType.Single, new string[] { "확인" }, () => { });
				return;
			}

			audioSource.Play();

			playStatus = PlayStatus.Playing;
		}

		/// <summary>
		/// 일시정지 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnPauseButtonClicked()
		{
			if (notesVerifier.IsError && doNotPlayWhenError)
			{
				OnStopButtonClicked();
				return;
			}

			audioSource.Pause();

			playStatus = PlayStatus.Paused;
		}

		/// <summary>
		/// 정지 버튼이 클릭되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void OnStopButtonClicked()
		{
			audioSource.Pause();

			playStatus = PlayStatus.Stopped;
		}

		/// <summary>
		/// 'Do not play when there is an error' 토글이 변경되었을 때 이벤트를 처리합니다.
		/// </summary>
		public void DoNotPlayWhenErrorToggleChanged()
		{
			doNotPlayWhenError = !doNotPlayWhenError;
		}
	}
}
