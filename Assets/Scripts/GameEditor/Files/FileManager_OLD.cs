using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

using SimpleFileBrowser;

using MineBeat.GameEditor.UI;
using MineBeat.GameEditor.Song;

/*
 * [Namespace] Minebeat.GameEditor.Files
 * Desciption
 */
namespace MineBeat.GameEditor.Files
{
	/*
	 * [Class] FileManager_OLD
	 * 에디터에서 사용하는 외부 파일의 입/출력을 관리합니다.
	 */
	public class FileManager_OLD : MonoBehaviour
	{
		// https://github.com/yasirkula/UnitySimpleFileBrowser

		[SerializeField]
		private GameObject windowCanvas;

		private SongManager songManager;
		private GameManager gameManager;

		private void Start()
		{
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			windowCanvas.SetActive(false);

			OpenMediaFile();
		}

		/*
		 * [Method] OpenMediaFile(): void
		 * 레벨에서 사용할 음악 파일을 선택하는 창을 엽니다.
		 */
		public void OpenMediaFile()
		{
			Destroy(songManager.GetComponent<AudioSource>().clip); // WWW 모듈 한정: 메모리 누수 방지를 위해 해줘야함 ( https://blog.naver.com/indra1469/220963432353 )

			FileBrowser.SetFilters(true, new FileBrowser.Filter("Audio Files", ".mp3", ".wav", ".ogg"));
			FileBrowser.SetDefaultFilter(".mp3");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			StartCoroutine(ShowLoadDialog("Select Music...", true));
		}

		/*
		 * [Method] OpenPatternFile(): void
		 * 레벨 정보가 있는 패턴 파일을 선택하는 창을 엽니다.
		 */
		public void OpenPatternFile()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Pattern Files", ".mbt"));
			FileBrowser.SetDefaultFilter(".mbt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			StartCoroutine(ShowLoadDialog("Select Pattern File...", false));
		}

		private IEnumerator ShowLoadDialog(string title, bool isAudioFile)
		{
			windowCanvas.SetActive(true);
			yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, title, "Load");

			if (FileBrowser.Success)
			{
				string filePath = FileBrowser.Result[0];
				if (isAudioFile)
				{
					WWW www = new WWW(filePath);
					yield return www;
					songManager.audioClip = www.GetAudioClip();
					songManager.GetComponent<AudioSource>().clip = songManager.audioClip;

					GameObject.Find("UIManagers").GetComponent<TimelineManager>().UpdateAudioClip();

					songManager.OnPlayButtonClicked();
				}
				else
				{
					if (File.Exists(filePath))
					{
						BinaryFormatter formatter = new BinaryFormatter();
						FileStream stream = new FileStream(filePath, FileMode.Open);

						SongInfo data = formatter.Deserialize(stream) as SongInfo;

						stream.Close();

						gameManager.SetSongInfo(data);
					}
				}
				windowCanvas.SetActive(false);
			}
			else
			{
				if (isAudioFile) OpenMediaFile();
				else OpenPatternFile();
			}
		}

		/*
		 * [Method] SavePatternFile(string fileName): void
		 * 레벨 정보가 있는 패턴 파일을 저장하는 창을 엽니다.
		 * 
		 * <string fileName>
		 * 저장할 기본 파일명을 지정합니다.
		 */
		public void SavePatternFile(string fileName)
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Pattern Files", ".mbt"));
			FileBrowser.SetDefaultFilter(".mbt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			StartCoroutine(ShowSaveDialog("Save Pattern File to...", fileName));
		}

		IEnumerator ShowSaveDialog(string title, string fileName)
		{
			windowCanvas.SetActive(true);
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, "C:\\", fileName, title, "Save");
			windowCanvas.SetActive(false);

			if (FileBrowser.Success)
			{
				string filePath = FileBrowser.Result[0];

				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(filePath, FileMode.Create);

				formatter.Serialize(stream, gameManager.GetSongInfo());
				stream.Close();
			}
		}
	}
}
