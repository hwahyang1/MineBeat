using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleFileBrowser;

/*
 * [Namespace] Minebeat.GameEditor
 * Desciption
 */
namespace MineBeat.GameEditor
{
	/*
	 * [Class] FileManager
	 * 에디터에서 사용하는 외부 파일의 입/출력을 관리합니다.
	 */
	public class FileManager : MonoBehaviour
	{
		// https://github.com/yasirkula/UnitySimpleFileBrowser

		[SerializeField]
		private GameObject coverCanvas;

		private void Start()
		{
			coverCanvas.SetActive(false);

			SavePatternFile("PatternName.mbt");
		}

		/*
		 * [Method] OpenMediaFile(): void
		 * 레벨에서 사용할 음악 파일을 선택하는 창을 엽니다.
		 */
		public void OpenMediaFile()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Audio Files", ".mp3", ".wav", ".ogg"));
			FileBrowser.SetDefaultFilter(".mp3");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			StartCoroutine(ShowLoadDialog("Select Music..."));
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

			StartCoroutine(ShowLoadDialog("Select Pattern File..."));
		}

		private IEnumerator ShowLoadDialog(string title)
		{
			coverCanvas.SetActive(true);
			yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, title, "Load");
			coverCanvas.SetActive(false);

			Debug.Log(FileBrowser.Success);

			if (FileBrowser.Success)
			{
				// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
				for (int i = 0; i < FileBrowser.Result.Length; i++)
					Debug.Log(FileBrowser.Result[i]);

				// Read the bytes of the first file via FileBrowserHelpers
				// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
				byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

				// Or, copy the first file to persistentDataPath
				string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
				FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
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
			coverCanvas.SetActive(true);
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, "C:\\", fileName, title, "Save" );
			coverCanvas.SetActive(false);

			Debug.Log(FileBrowser.Success);

			if (FileBrowser.Success)
			{
				// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
				for (int i = 0; i < FileBrowser.Result.Length; i++)
					Debug.Log(FileBrowser.Result[i]);

				// Read the bytes of the first file via FileBrowserHelpers
				// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
				byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

				// Or, copy the first file to persistentDataPath
				string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
				FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
			}
		}
	}
}
