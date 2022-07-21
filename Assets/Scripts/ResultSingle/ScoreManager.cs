using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.Score;
using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;

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
			scoreHistoryManager = GameObject.Find("ScoreHistoryManager").GetComponent<ScoreHistoryManager>();
			selectedSongInfo = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>();

			songInfo = PackageManager.Instance.GetSongInfo(selectedSongInfo.id);
			previousHistory = scoreHistoryManager.GetHistory(selectedSongInfo.id);

			scoreComboHistory.Add(new System.Tuple<uint, uint, bool>(previousHistory.score, selectedSongInfo.score, selectedSongInfo.score > previousHistory.score));
			scoreComboHistory.Add(new System.Tuple<uint, uint, bool>(previousHistory.maxCombo, selectedSongInfo.combo, selectedSongInfo.combo > previousHistory.maxCombo));

			playRank = PlayRank.D;

			SubmitData();
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
