using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

/*
 * [Namespace] Minebeat.GameEditor.Notes
 * Desciption
 */
namespace MineBeat.GameEditor.Notes
{
	/*
	 * [Class] NotesVerifier
	 * 배치된 노트의 논리적 오류를 찾아냅니다.
	 */
	public class NotesVerifier : MonoBehaviour
	{
		[SerializeField]
		private Color colorSuccess;
		[SerializeField]
		private Color colorError;
	}
}
