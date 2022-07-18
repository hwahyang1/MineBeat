using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;

using MineBeat.InGameSingle.Song;

/*
 * [Namespace] MineBeat.InGameSingle.Notes
 * Description
 */
namespace MineBeat.InGameSingle.Notes
{
	/*
	 * [Class] PlaceNote
	 * 노트의 배치를 관리합니다.
	 */
	public class PlaceNote : MonoBehaviour
	{
		private ulong id;
		private List<Note> notes;

		private float previousTimecode = 0f;

		private void Awake()
		{
			id = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>().id;
			notes = PackageManager.Instance.GetSongInfo(id).notes;
		}

		private void Start()
		{

		}

		private void Update()
		{
			
		}
	}
}
