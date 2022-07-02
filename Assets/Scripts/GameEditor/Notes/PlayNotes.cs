using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.GameEditor.Song;

/*
 * [Namespace] Minebeat.GameEditor.Notes
 * Desciption
 */
namespace MineBeat.GameEditor.Notes
{
	/*
	 * [Class] PlayNotes
	 * TimeCode에 따른 노트의 위치를 계산하고, 노트를 배치 및 제거합니다.
	 */
	public class PlayNotes : MonoBehaviour
	{
		[Header("Tilemap")]
		[SerializeField]
		private Tilemap noteWarningTilemap;

		[SerializeField]
		private TileBase normalWarning;
		[SerializeField, Tooltip("노트의 위치를 기준으로 상, 하, 좌, 우에서 보일 TileBase를 입력하세요.")]
		private TileBase[] verticalWarning = new TileBase[4];

		[Header("Prefab")]
		[SerializeField]
		private GameObject whiteBox;

		[Header("Parent")]
		[SerializeField]
		private Transform noteParent;

		private NotesManager notesManager;
		private SongManager songManager;

		private void Start()
		{
			notesManager = gameObject.GetComponent<NotesManager>();
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		private void Update()
		{
			for (int i = 0; i < noteParent.childCount; i++)
			{
				Destroy(noteParent.GetChild(i));
			}

			List<Note> currentNotes = notesManager.GetList();

			/* 
			 * 현재 Time을 0초라 했을 때
			 * ~0초: 노트를 해당되는 위치에 배치
			 * 0~0.5초: Warning을 해당되는 위치에 배치
			 */
			
		}
	}
}
