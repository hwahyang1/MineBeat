using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Networking;

using SimpleFileBrowser; // https://github.com/yasirkula/UnitySimpleFileBrowser

using MineBeat.GameEditor.UI;
using MineBeat.GameEditor.Song;

/*
 * [Namespace] Minebeat.GameEditor.Files
 * Desciption
 */
namespace MineBeat.GameEditor.Files
{
	// SongName.mbt(패키지(압축) 파일) -> MineBeat.ptrn(패턴 파일) && MineBeat.adio(음원 파일)

	/*
	 * [Class] FileManager
	 * 에디터에서 사용하는 외부 파일의 입/출력을 관리합니다.
	 */
	public class FileManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject canvas;
		private bool maintainCanvas = false;

		private bool isFirst = true;

		BinaryFormatter formatter = new BinaryFormatter();

		private string packageFileName = "MineBeat.mbt";

		private readonly string tempFileRootFolderPath = @"C:\Temp\MineBeat_DoNotDelete\";
		private readonly string tempPatternFilePath = @"C:\Temp\MineBeat_DoNotDelete\MineBeat.ptrn";
		private readonly string tempAudioFilePath = @"C:\Temp\MineBeat_DoNotDelete\MineBeat.adio";

		private FileStream packageFileStream = null;
		private FileStream tempPatternFileStream = null;
		private FileStream tempAudioFileStream = null;

		private AlertManager alertManager;
		private SongManager songManager;
		private GameManager gameManager;

		/*
		 * [Coroutine] DelayedStart()
		 * 스크립트의 준비를 위해 Start() Method의 일부 코드를 조금 늦게 실행합니다.
		 */
		public IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(0.1f);
			while (true)
			{
				int res = alertManager.Show("Choose one", "Do you want to create the new pattern file?\nOr do you want to load the existing pattern file?", AlertManager.AlertButtonType.Double, new string[] { "Create new", "Load exist" }, OnNewButtonClicked, () => { });
				if (res == 0) break;
			}
		}

		private void Start()
		{
			alertManager = GameObject.Find("UIManagers").GetComponent<AlertManager>();
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

			if (!Directory.Exists(tempFileRootFolderPath)) Directory.CreateDirectory(tempFileRootFolderPath);

			tempPatternFileStream = new FileStream(tempPatternFilePath, FileMode.Create);
			tempAudioFileStream = new FileStream(tempAudioFilePath, FileMode.Create);

			StartCoroutine(DelayedStart());
		}

		private void Update()
		{
			if (maintainCanvas)
			{
				canvas.SetActive(true);
			}
		}

		/*
		 * [Method] OnNewButtonClicked(): void
		 * New 버튼이 클릭되었을 때 이벤트를 처리합니다.
		 * 파일을 새로 불러오면 패턴 또한 초기화 됩니다.
		 */
		public void OnNewButtonClicked()
		{
			if (!isFirst)
			{
				while (true)
				{
					int res = alertManager.Show("Warning!", "This action initializes all progress.\nDo you really want to continue?", AlertManager.AlertButtonType.Double, new string[] { "Yes", "No" }, OpenSongFileButtonClicked, () => { });
					if (res == 0) return;
				}
			}
			OpenSongFileButtonClicked();
		}

		public void OpenSongFileButtonClicked()
		{
			if (!isFirst)
			{
				songManager.OnStopButtonClicked();
			}
			StartCoroutine("OpenSongFileCoroutine");
		}
		public void OpenPackageFileButtonClicked()
		{
			songManager.OnStopButtonClicked();
			StartCoroutine("OpenPackageFileCoroutine");
		}
		public void SavePackageFileButtonClicked()
		{
			StartCoroutine("SavePackageFileCoroutine");
		}

		public IEnumerator OpenSongFileCoroutine()
		{
			isFirst = false;

			FileBrowser.SetFilters(true, new FileBrowser.Filter("Audio Files", ".mp3"));
			FileBrowser.SetDefaultFilter(".mp3");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			maintainCanvas = true;
			while (true)
			{
				yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, @"C:\", null, "Select Pattern File...", "Load");

				if (FileBrowser.Success)
				{
					string filePath = FileBrowser.Result[0];

					tempAudioFileStream.Close();
					File.Copy(filePath, tempAudioFilePath, true);
					tempAudioFileStream = new FileStream(tempAudioFilePath, FileMode.Open);

					using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(tempAudioFilePath, AudioType.MPEG))
					{
						yield return webRequest.SendWebRequest();
						if (webRequest.result == UnityWebRequest.Result.ConnectionError)
						{
							continue;
						}
						else
						{
							songManager.audioClip = DownloadHandlerAudioClip.GetContent(webRequest);
							songManager.GetComponent<AudioSource>().clip = songManager.audioClip;

							alertManager.GetComponent<TimelineManager>().UpdateAudioClip();

							maintainCanvas = false;
							canvas.SetActive(false);

							songManager.OnPlayButtonClicked();

							break;
						}
					}
				}
			}
		}

		public IEnumerator OpenPackageFileCoroutine()
		{
			yield return null;
		}

		public IEnumerator SavePackageFileCoroutine()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Package Files", ".mbt"));
			FileBrowser.SetDefaultFilter(".mbt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			maintainCanvas = true;
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, @"C:\", packageFileName, "Save Pattern File to...", "Save");

			if (FileBrowser.Success)
			{
				string filePath = FileBrowser.Result[0]; // 여기서 파일명 빼다가 packageFileName 넣어놔야 함

				formatter.Serialize(tempPatternFileStream, gameManager.GetSongInfo());

				if (packageFileStream != null)
				{
					packageFileStream.Close();
					packageFileStream = null;
				}

				if (File.Exists(filePath)) File.Delete(filePath);

				ZipFile.CreateFromDirectory(tempFileRootFolderPath, filePath); // 개인 Discord To-Do 참고. 아무래도 핸들 다 닫았다가 다시 열어야 할 수도 있을듯. 그럴거면 메소드로 빼고.
				packageFileStream = new FileStream(filePath, FileMode.Open);
			}

			maintainCanvas = false;
			canvas.SetActive(false);
		}

		/* 삭제 예정 */
		public static bool ZipDirectory(string directoryPath, string outputZipPath)
		{
			try
			{
				if (File.Exists(outputZipPath))
				{
					File.Delete(outputZipPath);
				}

				ZipFile.CreateFromDirectory(directoryPath, outputZipPath);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool UnzipFile(string zipPath, string unzipPath)
		{
			try
			{
				if (Directory.Exists(unzipPath))
				{
					Directory.Delete(unzipPath);
				}

				ZipFile.ExtractToDirectory(zipPath, unzipPath);

				return true;
			}
			catch
			{
				return false;
			}
		}
		/* 삭제 예정 */

		/*
		 * [Method] CloaseAllFileSteam(): void
		 * 열려있는 모든 핸들을 닫습니다.
		 */
		private void CloseAllFileStream()
		{
			if (packageFileStream != null)
			{
				packageFileStream.Close();
				packageFileStream = null;
			}
			if (tempPatternFileStream != null)
			{
				tempPatternFileStream.Close();
				tempPatternFileStream = null;
			}
			if (tempAudioFileStream != null)
			{
				tempAudioFileStream.Close();
				tempAudioFileStream = null;
			}
		}

		private void OnDestroy()
		{
			CloseAllFileStream();

			File.Delete(tempPatternFilePath);
			File.Delete(tempAudioFilePath);

			Directory.Delete(tempFileRootFolderPath);
		}
	}
}
