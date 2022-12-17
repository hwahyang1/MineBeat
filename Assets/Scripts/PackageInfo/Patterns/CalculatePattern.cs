using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.PackageInfo.Patterns
{
	/// <summary>
	/// 연산 결과를 반환하는 클래스입니다.
	/// </summary>
	public class CalculatedPatternInfo
	{
		/* All Notes */
		public int NotesCount;
		public int NotesScore;
		public int NormalNotesCount;
		public int NormalNotesScore;
		public int VerticalNotesCount;
		public int VerticalNotesScore;

		/* Except Impact Lines */
		public int NotesCount_ExceptImpactLines;
		public int NotesScore_ExceptImpactLines;
		public int NormalNotesCount_ExceptImpactLines;
		public int NormalNotesScore_ExceptImpactLines;
		public int VerticalNotesCount_ExceptImpactLines;
		public int VerticalNotesScore_ExceptImpactLines;
	}

	/// <summary>
	/// 패턴과 관련된 연산을 수행합니다.
	/// </summary>
	public class CalculatePattern : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("DefineNote.NoteColor와 동일한 순서로 Normal Note의 점수를 입력합니다.")]
		private List<int> scores = new List<int>();

		public CalculatedPatternInfo Calculate(List<Note> original)
		{
			CalculatedPatternInfo returnData = new CalculatedPatternInfo();

			List<Note> notes = original.FindAll(target => target.type == NoteType.Normal || target.type == NoteType.Vertical);
			List<Note> notes_ExceptImpactLine = new List<Note>();

			// Impact Line 유무
			Note impactLine = original.Find(target => target.type == NoteType.ImpactLine);
			if (impactLine == null)
			{
				// 없으면 사용하지 않음
				notes_ExceptImpactLine = null;
			}
			else
			{
				// 있으면 Impact Line 제외하고 조건 충족하는 노트만 가져옴
				notes_ExceptImpactLine = notes.FindAll(target => target.timeCode < impactLine.timeCode);
			}

			/* 전체 노트 연산 */
			foreach (Note note in notes)
			{
				switch (note.type)
				{
					case NoteType.Normal:
						returnData.NotesScore += scores[(int)note.color];
						returnData.NotesCount++;

						returnData.NormalNotesScore += scores[(int)note.color];
						returnData.NormalNotesCount++;
						break;
					case NoteType.Vertical:
						//returnData.NotesScore += scores[(int)note.color];
						returnData.NotesCount++;

						//returnData.VerticalNotesScore += scores[(int)note.color];
						returnData.VerticalNotesCount++;
						break;
				}
			}

			/* Impact Line 제외 노트 연산 */
			if (notes_ExceptImpactLine == null) // Impact Line 없으면 값 복사
			{
				returnData.NotesCount_ExceptImpactLines = returnData.NotesCount;
				returnData.NotesScore_ExceptImpactLines = returnData.NotesScore;
				returnData.NormalNotesCount_ExceptImpactLines = returnData.NormalNotesCount;
				returnData.NormalNotesScore_ExceptImpactLines = returnData.NormalNotesScore;
				returnData.VerticalNotesCount_ExceptImpactLines = returnData.VerticalNotesCount;
				returnData.VerticalNotesScore_ExceptImpactLines = returnData.VerticalNotesScore;
			}
			else
			{
				foreach (Note note in notes_ExceptImpactLine)
				{
					switch (note.type)
					{
						case NoteType.Normal:
							returnData.NotesScore_ExceptImpactLines += scores[(int)note.color];
							returnData.NotesCount_ExceptImpactLines++;

							returnData.NormalNotesScore_ExceptImpactLines += scores[(int)note.color];
							returnData.NormalNotesCount_ExceptImpactLines++;
							break;
						case NoteType.Vertical:
							//returnData.NotesScore_ExceptImpactLines += scores[(int)note.color];
							returnData.NotesCount_ExceptImpactLines++;

							//returnData.VerticalNotesScore_ExceptImpactLines += scores[(int)note.color];
							returnData.VerticalNotesCount_ExceptImpactLines++;
							break;
					}
				}
			}

			return returnData;
		}
	}
}
