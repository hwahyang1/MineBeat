using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace MineBeat.PackageInfo.Files
{
	/// <summary>
	/// 현재 패키지 정보를 담습니다.
	/// </summary>
	public class CurrentPackageInfo : MonoBehaviour
	{
		[ReadOnly]
		public string currentPackagePath;
		[HideInInspector]
		public SongInfo currentSongInfo;
		[HideInInspector]
		public Sprite currentSongCover;
		[HideInInspector]
		public AudioClip currentAudioClip;
	}
}
