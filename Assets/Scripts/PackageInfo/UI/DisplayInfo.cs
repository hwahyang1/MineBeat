using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.PackageInfo.Files;
using MineBeat.PackageInfo.Patterns;

namespace MineBeat.PackageInfo.UI
{
	/// <summary>
	/// 현재 패키지 정보를 노출합니다.
	/// </summary>
	public class DisplayInfo : MonoBehaviour
	{
		/* Body - Left */
		[SerializeField]
		private Image packageCover;
		[SerializeField]
		private Text songName;
		[SerializeField]
		private Text songAuthor;
		[SerializeField]
		private Text packageLevel;

		/* Body - Right */
		[SerializeField]
		private Text packagePath;
		[SerializeField]
		private Text packageID;
		[SerializeField]
		private Text[] notesInfos = new Text[3];
		[SerializeField]
		private Text[] notesInfos_ExceptImpactLine = new Text[3];

		private CalculatePattern calculatePattern;
		private CurrentPackageInfo currentPackageInfo;

		private void Start()
		{
			calculatePattern = GetComponent<CalculatePattern>();
			currentPackageInfo = GetComponent<CurrentPackageInfo>();
		}

		public void UpdateInfo()
		{
			CalculatedPatternInfo calculatedPatternInfo = calculatePattern.Calculate(currentPackageInfo.currentSongInfo.notes);

			/* Body - Left */
			packageCover.sprite = currentPackageInfo.currentSongCover;
			songName.text = currentPackageInfo.currentSongInfo.songName;
			songAuthor.text = currentPackageInfo.currentSongInfo.songAuthor;
			packageLevel.text = string.Format("Level {0:00}", currentPackageInfo.currentSongInfo.songLevel);

			/* Body - Right */
			packagePath.text = "Package Path: " + currentPackageInfo.currentPackagePath;
			packageID.text = "Pattern ID: " + currentPackageInfo.currentSongInfo.id;

			notesInfos[0].text = string.Format("Notes: {0} (Score: {1})", calculatedPatternInfo.NotesCount, calculatedPatternInfo.NotesScore);
			notesInfos[1].text = string.Format("Normal Notes: {0} (Score: {1})", calculatedPatternInfo.NormalNotesCount, calculatedPatternInfo.NormalNotesScore);
			notesInfos[2].text = string.Format("Vertical Notes: {0} (Score: {1})", calculatedPatternInfo.VerticalNotesCount, calculatedPatternInfo.VerticalNotesScore);

			notesInfos_ExceptImpactLine[0].text = string.Format("Notes (Except Impact Line): {0} (Score: {1})", calculatedPatternInfo.NotesCount_ExceptImpactLines, calculatedPatternInfo.NotesScore_ExceptImpactLines);
			notesInfos_ExceptImpactLine[1].text = string.Format("Normal Notes (Except Impact Line): {0} (Score: {1})", calculatedPatternInfo.NormalNotesCount_ExceptImpactLines, calculatedPatternInfo.NormalNotesScore_ExceptImpactLines);
			notesInfos_ExceptImpactLine[2].text = string.Format("Vertical Notes (Except Impact Line): {0} (Score: {1})", calculatedPatternInfo.VerticalNotesCount_ExceptImpactLines, calculatedPatternInfo.VerticalNotesScore_ExceptImpactLines);
		}
	}
}
