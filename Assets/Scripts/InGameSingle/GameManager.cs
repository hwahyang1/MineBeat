using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.InGameSingle.HP;
using MineBeat.InGameSingle.Song;
using MineBeat.InGameSingle.Score;
using MineBeat.InGameSingle.Player;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Scene;

namespace MineBeat.InGameSingle
{
	/// <summary>
	/// 게임의 전반적인 실행을 관리합니다.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[SerializeField, Tooltip("DefineNote.NoteColor 순서대로 점수의 변동치를 입력합니다.")]
		private uint[] scoreAdjust;
		[SerializeField, Tooltip("Normal Note, Vertical Note, Purple Color 순서대로 총 세개의 체력 변동치를 입력합니다.")]
		private short[] hpAdjust;

		private HPManager hpManager;
		private ScoreManager scoreManager;
		private SongPlayManager songPlayManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			hpManager = managers.Find(target => target.name == "HPManager").GetComponent<HPManager>();
			scoreManager = managers.Find(target => target.name == "ScoreManager").GetComponent<ScoreManager>();
			songPlayManager = managers.Find(target => target.name == "SongManager").GetComponent<SongPlayManager>();
		}

		private void Update()
		{
			if (hpManager.Hp <= 0)
			{
				SceneChange.Instance.ChangeScene("SongSelectSingleScene");

				Destroy(gameObject);
			}

			if (!songPlayManager.IsPlaying)
			{
				SelectedSongInfo selectedSongInfo = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>();
				selectedSongInfo.Score = scoreManager.Score;
				selectedSongInfo.Combo = scoreManager.MaxCombo;

				SceneChange.Instance.ChangeScene("ResultSingleScene");

				Destroy(gameObject);
			}
		}

		/// <summary>
		/// 체력 변경 이벤트를 처리합니다.
		/// </summary>
		/// <param name="noteType">플레이어가 맞은 노트의 종류를 입력합니다.</param>
		/// <param name="noteColor">플레이어가 맞은 노트의 색상을 입력합니다.</param>
		public void ChangeHP(NoteType noteType, NoteColor noteColor)
		{
			// Purple Note면 체력 또는 점수 변경, 아니면은 체력 변경
			if (noteColor == NoteColor.Purple)
			{
				if (hpManager.Hp == hpManager.MaxHp)
				{
					ChangeScore(noteColor);
				}
				else
				{
					hpManager.Hp += hpAdjust[2];
				}
			}
			else
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StartCoolTime();
				hpManager.Hp += hpAdjust[(int)noteType];
				scoreManager.ChangeCombo(0);
			}
		}

		/// <summary>
		/// 점수 변경 이벤트를 처리합니다.
		/// </summary>
		/// <param name="noteColor">플레이어가 피한 노트의 색상을 입력합니다.</param>
		public void ChangeScore(NoteColor noteColor)
		{
			scoreManager.AddScore(scoreAdjust[(int)noteColor]);
			if (noteColor != NoteColor.Purple) scoreManager.ChangeCombo(scoreManager.Combo + 1);
		}
	}
}
