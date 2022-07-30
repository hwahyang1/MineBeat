using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using MineBeat.SongSelectSingle.Score;
using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;
using MineBeat.Preload.Config;

/*
 * [Namespace] MineBeat.ResultSingle
 * Description
 */
namespace MineBeat.ResultSingle
{
	/*
	 * [Class] ScoreManager
	 * 결과 점수를 처리합니다.
	 */
	public class ScoreManager : MonoBehaviour
	{
		private ScoreHistoryManager scoreHistoryManager;
		private SelectedSongInfo selectedSongInfo;

		private SongInfo songInfo;
		private PlayHistory previousHistory;

		/* (이전 기록, 현재 기록, 최고기록 갱신 여부)를 한 세트로 점수와 콤보의 기록을 저장함 */
		private List<System.Tuple<uint, uint, bool>> scoreComboHistory = new List<System.Tuple<uint, uint, bool>>();

		private PlayRank playRank;

		private void Awake()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			scoreHistoryManager = managers.Find(target => target.name == "ScoreHistoryManager").GetComponent<ScoreHistoryManager>();
			selectedSongInfo = managers.Find(target => target.name == "SelectedSongInfo").GetComponent<SelectedSongInfo>();

			songInfo = PackageManager.Instance.GetSongInfo(selectedSongInfo.id);
			previousHistory = scoreHistoryManager.GetHistory(selectedSongInfo.id);

			scoreComboHistory.Add(new System.Tuple<uint, uint, bool>(previousHistory.score, selectedSongInfo.score, selectedSongInfo.score > previousHistory.score));
			scoreComboHistory.Add(new System.Tuple<uint, uint, bool>(previousHistory.maxCombo, selectedSongInfo.combo, selectedSongInfo.combo > previousHistory.maxCombo));

			CalculateRank();

			SubmitData();

			if (ConfigManager.Instance.GetConfig().skipResult) SceneManager.LoadScene("SongSelectSingleScene");
		}

		/*
		 * [Method] CalculateRank(): void
		 * 랭크를 계산합니다.
		 */
		private void CalculateRank()
		{
			float impactLineTimecode;
			try
			{
				impactLineTimecode = songInfo.notes.Find(target => target.type == NoteType.ImpactLine).timeCode;
			}
			catch (System.NullReferenceException)
			{
				impactLineTimecode = songInfo.notes[songInfo.notes.Count - 1].timeCode + 1f;
			}
			List<Note> targets = songInfo.notes.FindAll(target => target.timeCode < impactLineTimecode && target.type == NoteType.Normal && target.color != NoteColor.Purple);

			uint maxScore = 0;

			foreach (Note note in targets)
			{
				switch (note.color)
				{
					case NoteColor.White:
						maxScore += 20;
						break;
					case NoteColor.Skyblue:
						maxScore += 40;
						break;
					case NoteColor.Blue:
						maxScore += 50;
						break;
					case NoteColor.Green:
						maxScore += 70;
						break;
					case NoteColor.Orange:
						maxScore += 100;
						break;
				}
			}

			float rate = scoreComboHistory[0].Item2 / (maxScore * 1.0f);

			if (rate >= 0.98f) playRank = PlayRank.S;
			else if (rate >= 0.9f) playRank = PlayRank.A;
			else if (rate >= 0.8f) playRank = PlayRank.B;
			else if (rate >= 0.7f) playRank = PlayRank.C;
			else playRank = PlayRank.D;
		}

		/*
		 * [Method] SubmitData(): void
		 * 점수를 갱신합니다.
		 */
		private void SubmitData()
		{
			if (!(scoreComboHistory[0].Item3 || scoreComboHistory[1].Item3)) return;

			PlayHistory history = new PlayHistory(
				songInfo.id,
				(ulong)System.DateTimeOffset.Now.ToUnixTimeSeconds(),
				scoreComboHistory[0].Item3 ? scoreComboHistory[0].Item2 : scoreComboHistory[0].Item1,
				scoreComboHistory[1].Item3 ? scoreComboHistory[1].Item2 : scoreComboHistory[1].Item1,
				playRank
			);

			scoreHistoryManager.AddHistory(history);
		}

		/*
		 * [Method] GetSongInfo(): SongInfo
		 * 현재 곡의 정보를 반환합니다.
		 * 
		 * <RETURN: SongInfo>
		 * 현재 곡의 정보를 반환합니다.
		 */
		public SongInfo GetSongInfo()
		{
			return songInfo;
		}

		/*
		 * [Method] GetScoreComboHistory(): List<System.Tuple<ulong, ulong, bool>>
		 * 점수와 콤보의 이전 기록, 현재 기록과 최고기록 갱신 여부를 반환합니다.
		 * 
		 * <RETURN: List<System.Tuple<ulong, ulong, bool>>>
		 * 점수와 콤보의 이전 기록, 현재 기록과 최고기록 갱신 여부를 반환합니다.
		 */
		public List<System.Tuple<uint, uint, bool>> GetScoreComboHistory()
		{
			return scoreComboHistory;
		}

		/*
		 * [Method] GetPlayRank(): PlayRank
		 * 현재 점수 기준 랭크를 반환합니다.
		 * 
		 * <RETURN: PlayRank>
		 * 현재 점수 기준 랭크를 반환합니다.
		 */
		public PlayRank GetPlayRank()
		{
			return playRank;
		}
	}
}
