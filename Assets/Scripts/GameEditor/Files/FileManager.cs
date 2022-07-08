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
using MineBeat.GameEditor.Notes;

/*
 * [Namespace] Minebeat.GameEditor.Files
 * Desciption
 */
namespace MineBeat.GameEditor.Files
{
	// SongName.mbt(패키지(압축) 파일) -> MineBeat.ptrn(패턴 파일) && MineBeat.adio(음원 파일) && MineBeat.covr(커버이미지)

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

		private string packageFilePath = @"C:\";
		private string packageFileName = "MineBeat.mbt";

		private readonly string tempFileRootFolderPath = @"C:\Temp\MineBeat_DoNotDelete\";
		private readonly string tempPatternFilePath = @"C:\Temp\MineBeat_DoNotDelete\MineBeat.ptrn";
		private readonly string tempAudioFilePath = @"C:\Temp\MineBeat_DoNotDelete\MineBeat.adio";
		private readonly string tempCoverImageFilePath = @"C:\Temp\MineBeat_DoNotDelete\MineBeat.covr";

		private FileStream packageFileStream = null;
		private FileStream tempPatternFileStream = null;
		private FileStream tempAudioFileStream = null;
		private FileStream tempCoverImageFileStream = null;

		private NotesManager notesManager;
		private AlertManager alertManager;
		private SongManager songManager;
		private GameManager gameManager;
		private SongCover songCover;

		/*
		 * [Method] OpenAllFileStream(FileMode mode=FileMode.Open, FileAccess access=FileAccess.ReadWrite, string packageFilePath=""): void
		 * 모든 핸들을 엽니다.
		 * 
		 * <FileMode mode=FileMode.Open>
		 * 핸들의 오픈 형식을 지정합니다.
		 * 
		 * <FileAccess access=FileAccess.ReadWrite>
		 * 핸들의 접근 형식을 지정합니다.
		 * 
		 * <string packageFilePath="">
		 * packageFileStream의 핸들을 열 경우 파일의 경로를 입력합니다.
		 */
		private void OpenAllFileStream(FileMode mode=FileMode.Open, FileAccess access=FileAccess.ReadWrite, string packageFilePath = "")
		{
			if (packageFilePath != "") packageFileStream = new FileStream(packageFilePath, mode);
			tempPatternFileStream = new FileStream(tempPatternFilePath, mode, access);
			tempAudioFileStream = new FileStream(tempAudioFilePath, mode, access);
			tempCoverImageFileStream = new FileStream(tempCoverImageFilePath, mode, access);
		}

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
			if (tempCoverImageFileStream != null)
			{
				tempCoverImageFileStream.Close();
				tempCoverImageFileStream = null;
			}
		}

		/*
		 * [Coroutine] DelayedStart()
		 * 스크립트의 준비를 위해 Start() Method의 일부 코드를 조금 늦게 실행합니다.
		 */
		public IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(0.1f);
			while (true)
			{
				int res = alertManager.Show("Choose one", "Do you want to create the new pattern file?\nOr do you want to load the existing pattern file?", AlertManager.AlertButtonType.Double, new string[] { "Create new", "Load exist" }, OnNewButtonClicked, OpenPackageFileWorker);
				if (res == 0) break;
			}
		}

		private void Start()
		{
			notesManager = GameObject.Find("NoteManagers").GetComponent<NotesManager>();
			alertManager = GameObject.Find("UIManagers").GetComponent<AlertManager>();
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			songCover = GameObject.Find("UIManagers").GetComponent<SongCover>();

			if (Directory.Exists(tempFileRootFolderPath)) Directory.Delete(tempFileRootFolderPath, true);
			Directory.CreateDirectory(tempFileRootFolderPath);

			OpenAllFileStream(FileMode.OpenOrCreate);

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
					int res = alertManager.Show("Warning!", "This action initializes all progress.\nDo you really want to continue?", AlertManager.AlertButtonType.Double, new string[] { "Yes", "No" }, OpenSongFileWorker, () => { });
					if (res == 0) return;
				}
			}
			OpenSongFileWorker();
		}
		public void OpenSongFileWorker() // 함수명 이상하게 지어놨네 이거 유니티에서 쓰는 게 아님
		{
			if (!isFirst)
			{
				songManager.OnStopButtonClicked();
			}
			StartCoroutine("OpenSongFileCoroutine");
		}

		public void OpenPackageFileButtonClicked()
		{
			if (!isFirst)
			{
				while (true)
				{
					int res = alertManager.Show("Warning!", "This action initializes all progress.\nDid you save the file you were working on?", AlertManager.AlertButtonType.Double, new string[] { "Yes (Load)", "No (Close)" }, OpenPackageFileWorker, () => { });
					if (res == 0) return;
				}
			}
			OpenPackageFileWorker();
		}
		public void OpenPackageFileWorker()
		{
			if (!isFirst)
			{
				songManager.OnStopButtonClicked();
			}
			StartCoroutine("OpenPackageFileCoroutine");
		}

		public void SavePackageFileButtonClicked()
		{
			songManager.OnStopButtonClicked();
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
				yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, @"C:\", null, "Select Song File...", "Load");

				if (FileBrowser.Success)
				{
					string filePath = FileBrowser.Result[0];

					tempAudioFileStream.Close();
					File.Copy(filePath, tempAudioFilePath, true);
					tempAudioFileStream = new FileStream(tempAudioFilePath, FileMode.Open, FileAccess.ReadWrite);

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

							notesManager.RemoveAll();
							break;
						}
					}
				}
			}
		}

		public IEnumerator OpenPackageFileCoroutine()
		{
			isFirst = false;

			FileBrowser.SetFilters(true, new FileBrowser.Filter("Package Files", ".mbt"));
			FileBrowser.SetDefaultFilter(".mbt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			maintainCanvas = true;
			while (true)
			{
				yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, packageFilePath, "", "Select Pattern File...", "Load");

				if (FileBrowser.Success)
				{
					string filePath = FileBrowser.Result[0];

					packageFilePath = Path.GetDirectoryName(filePath);
					packageFileName = Path.GetFileName(filePath);

					CloseAllFileStream();

					ZipFile.ExtractToDirectory(filePath, tempFileRootFolderPath, true);

					OpenAllFileStream(FileMode.Open, FileAccess.ReadWrite, filePath);

					SongInfo data = formatter.Deserialize(tempPatternFileStream) as SongInfo;
					gameManager.SetSongInfo(data);

					UnityWebRequest imageWebRequest = UnityWebRequestTexture.GetTexture(tempCoverImageFilePath);
					yield return imageWebRequest.SendWebRequest();
					if (imageWebRequest.result == UnityWebRequest.Result.ConnectionError)
					{
						yield return null;
					}
					else
					{
						Texture2D texture = ((DownloadHandlerTexture)imageWebRequest.downloadHandler).texture;
						Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
						songCover.UpdateImage(sprite);
					}

					using (UnityWebRequest audioWebRequest = UnityWebRequestMultimedia.GetAudioClip(tempAudioFilePath, AudioType.MPEG))
					{
						yield return audioWebRequest.SendWebRequest();
						if (audioWebRequest.result == UnityWebRequest.Result.ConnectionError)
						{
							continue;
						}
						else
						{
							songManager.audioClip = DownloadHandlerAudioClip.GetContent(audioWebRequest);
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

		public IEnumerator SavePackageFileCoroutine()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Package Files", ".mbt"));
			FileBrowser.SetDefaultFilter(".mbt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			maintainCanvas = true;
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, packageFilePath, packageFileName, "Save Pattern File to...", "Save");

			if (FileBrowser.Success)
			{
				string filePath = FileBrowser.Result[0];

				packageFilePath = Path.GetDirectoryName(filePath);
				packageFileName = Path.GetFileName(filePath);

				formatter.Serialize(tempPatternFileStream, gameManager.GetSongInfo());

				if (File.Exists(filePath)) File.Delete(filePath);

				CloseAllFileStream();

				ZipFile.CreateFromDirectory(tempFileRootFolderPath, filePath);

				OpenAllFileStream(FileMode.Open, FileAccess.ReadWrite, filePath);
			}

			maintainCanvas = false;
			canvas.SetActive(false);
		}

		public IEnumerator OpenImageFileCoroutine()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Image Files", ".jpg", ".png"));
			FileBrowser.SetDefaultFilter(".jpg");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			maintainCanvas = true;
			yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, @"C:\", null, "Select Image File...", "Load");

			if (FileBrowser.Success)
			{
				string filePath = FileBrowser.Result[0];

				tempCoverImageFileStream.Close();
				File.Copy(filePath, tempCoverImageFilePath, true);
				tempCoverImageFileStream = new FileStream(tempCoverImageFilePath, FileMode.Open, FileAccess.ReadWrite);

				UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(tempCoverImageFilePath);
				yield return webRequest.SendWebRequest();
				if (webRequest.result == UnityWebRequest.Result.ConnectionError)
				{
					yield return null;
				}
				else
				{
					Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
					Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
					songCover.UpdateImage(sprite);
				}
			}

			maintainCanvas = false;
			canvas.SetActive(false);
		}

		private void OnDestroy()
		{
			CloseAllFileStream();

			// 생각해보니까 폴더 전체를 지울거면 이걸 쓸 이유가 없음
			/*File.Delete(tempPatternFilePath);
			File.Delete(tempAudioFilePath);*/

			Directory.Delete(tempFileRootFolderPath, true);
		}
	}
}
