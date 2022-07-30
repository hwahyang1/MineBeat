using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.GameEditor.Notes;

using MineBeat.Preload.UI;

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
			if (notesVerifier.isError && doNotPlayWhenError)
			{
				alertManager.Show("Alert", "Unable to play due to an error in the note placement.\nPlease try again after fixing the error or turn off the 'Do not play when there is an error' toggle.", AlertManager.AlertButtonType.Single, new string[] { "Close" }, () => { });
				return;
			}

			audioSource.Play();

			playStatus = PlayStatus.Playing;
		}

		/*
		 * [Method] OnPauseButtonClicked(): void
		 * 일시정지 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void OnPauseButtonClicked()
		{
			if (notesVerifier.isError && doNotPlayWhenError)
			{
				OnStopButtonClicked();
				return;
			}

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

		/*
		 * [Method] DoNotPlayWhenErrorToggleChanged(): void
		 * Do not play when there is an error 토글이 변경되었을 때 이벤트를 처리합니다.
		 */
		public void DoNotPlayWhenErrorToggleChanged()
		{
			doNotPlayWhenError = !doNotPlayWhenError;
		}
	}
}
