using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using MineBeat.SongSelectSingle.Score;
using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;
using MineBeat.Preload.Config;

namespace MineBeat.ResultSingle
{
	/// <summary>
	/// 결과 점수를 처리합니다.
	/// </summary>
	public class ScoreManager : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("DefineNote.NoteColor와 동일한 순서로 Normal Note의 점수를 입력합니다.")]
		private List<int> scores = new List<int>();

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

		/// <summary>
		/// 랭크를 계산합니다.
		/// </summary>
		private void CalculateRank()
		{
			// 패턴 데이터
			List<Note> original = songInfo.notes;
			// 점수 계산 대상
			List<Note> targets = new List<Note>();

			// ImpactLine 유무
			Note impactLine = original.Find(target => target.type == NoteType.ImpactLine);
			if (impactLine == null)
			{
				// 없으면 전구간에서 조건 충족하는 노트만 가져옴
				targets = original.FindAll(target => target.type == NoteType.Normal && target.color != NoteColor.Purple);
			}
			else
			{
				// 있으면 ImpactLine 제외하고 조건 충족하는 노트만 가져옴
				targets = original.FindAll(target => target.timeCode < impactLine.timeCode &&
											target.type == NoteType.Normal && target.color != NoteColor.Purple);
			}

			int maxScore = 0;

			foreach (Note note in targets)
			{
				maxScore += scores[(int)note.color];
			}

			float rate = scoreComboHistory[0].Item2 / (maxScore * 1.0f);

			if (rate >= 0.98f) playRank = PlayRank.S;
			else if (rate >= 0.9f) playRank = PlayRank.A;
			else if (rate >= 0.8f) playRank = PlayRank.B;
			else if (rate >= 0.7f) playRank = PlayRank.C;
			else playRank = PlayRank.D;
		}

		/// <summary>
		/// 점수를 갱신합니다.
		/// 무적 모드가 켜져있을 경우, 무시됩니다.
		/// </summary>
		private void SubmitData()
		{
			if (ConfigManager.Instance.GetConfig().undeadMode) return;

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

		/// <summary>
		/// 현재 곡의 정보를 반환합니다.
		/// </summary>
		/// <returns>현재 곡의 정보를 반환합니다.</returns>
		public SongInfo GetSongInfo() => songInfo;

		/// <summary>
		/// 점수와 콤보의 이전 기록, 현재 기록과 최고기록 갱신 여부를 반환합니다.
		/// </summary>
		/// <returns>점수와 콤보의 이전 기록, 현재 기록과 최고기록 갱신 여부를 반환합니다.</returns>
		public List<System.Tuple<uint, uint, bool>> GetScoreComboHistory() => scoreComboHistory;

		/// <summary>
		/// 현재 점수 기준 랭크를 반환합니다.
		/// </summary>
		/// <returns>현재 점수 기준 랭크를 반환합니다.</returns>
		public PlayRank GetPlayRank() => playRank;
	}
}
