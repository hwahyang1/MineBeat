using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.InGameSingle.HP;
using MineBeat.InGameSingle.Song;
using MineBeat.InGameSingle.Score;
using MineBeat.InGameSingle.Player;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Scene;

/*
 * [Namespace] MineBeat.InGameSingle
 * Description
 */
namespace MineBeat.InGameSingle
{
	/*
	 * [Class] GameManager
	 * 게임의 전반적인 실행을 관리합니다.
	 */
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
			hpManager = GameObject.Find("HPManager").GetComponent<HPManager>();
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
			songPlayManager = GameObject.Find("SongManager").GetComponent<SongPlayManager>();
		}

		private void Update()
		{
			if (!songPlayManager.isPlaying)
			{
				SelectedSongInfo selectedSongInfo = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>();
				selectedSongInfo.score = scoreManager.score;
				selectedSongInfo.combo = scoreManager.maxCombo;

				SceneChange.Instance.ChangeScene("ResultSingleScene");

				Destroy(gameObject);
			}
		}

		/*
		 * [Method] ChangeHP(NoteColor noteColor): void
		 * 체력 변경 이벤트를 처리합니다.
		 * 
		 * <NoteType noteType>
		 * 플레이어가 맞은 노트의 종류를 입력합니다.
		 * 
		 * <NoteColor noteColor>
		 * 플레이어가 맞은 노트의 색상을 입력합니다.
		 */
		public void ChangeHP(NoteType noteType, NoteColor noteColor)
		{
			if (noteColor == NoteColor.Purple)
			{
				if (hpManager.hp == hpManager.maxHp)
				{
					ChangeScore(noteColor);
				}
				else
				{
					hpManager.hp += hpAdjust[2];
				}
			}
			else
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StartCoolTime();
				hpManager.hp += hpAdjust[(int)noteType];
				scoreManager.ChangeCombo(0);
			}
		}

		/*
		 * [Method] ChangeScore(NoteColor noteColor): void
		 * 점수 변경 이벤트를 처리합니다.
		 * 
		 * <NoteColor noteColor>
		 * 플레이어가 피한 노트의 색상을 입력합니다.
		 */
		public void ChangeScore(NoteColor noteColor)
		{
			scoreManager.AddScore(scoreAdjust[(int)noteColor]);
			if (noteColor != NoteColor.Purple) scoreManager.ChangeCombo(scoreManager.combo + 1);
		}
	}
}
