using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat
{
	/// <summary>
	/// 지정 가능한 랭크를 정의합니다.
	/// </summary>
	[System.Serializable]
	public enum PlayRank
	{
		S,
		A,
		B,
		C,
		D,
		X
	}

	/// <summary>
	/// 플레이어의 곡 플레이 기록을 담습니다.
	/// </summary>
	[System.Serializable]
	public class PlayHistory
	{
		public ulong songId;
		public ulong timecode;
		public uint score;
		public uint maxCombo;
		public PlayRank rank;
		public PlayHistory(ulong songId, ulong timecode, uint score, uint maxCombo, PlayRank rank)
		{
			this.songId = songId;
			this.timecode = timecode;
			this.score = score;
			this.maxCombo = maxCombo;
			this.rank = rank;
		}

	}
}