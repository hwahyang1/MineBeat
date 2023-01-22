using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using SimpleFileBrowser;

using DisplayInfo = MineBeat.PackageInfo.UI.DisplayInfo;

namespace MineBeat.PackageInfo.Files
{
	/// <summary>
	/// 패키지의 입/출력을 관리합니다.
	/// </summary>
	public class FileManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject backgroundCover;

		private const string TempPackageRootFolderPath = @"C:\Temp\MineBeat_PackageInfo_DoNotDelete\";
		private const string TempPatternFilePath = @"C:\Temp\MineBeat_PackageInfo_DoNotDelete\MineBeat.ptrn";
		private const string TempAudioFilePath = @"C:\Temp\MineBeat_PackageInfo_DoNotDelete\MineBeat.adio";
		private const string TempCoverImageFilePath = @"C:\Temp\MineBeat_PackageInfo_DoNotDelete\MineBeat.covr";

		private FileStream packageFileStream = null;
		private FileStream tempPatternFileStream = null;
		private FileStream tempAudioFileStream = null;
		private FileStream tempCoverImageFileStream = null;

		private DisplayInfo displayInfo;
		private CurrentPackageInfo currentPackageInfo;

		/// <summary>
		/// 모든 핸들을 엽니다.
		/// </summary>
		/// <param name="mode">핸들의 오픈 형식을 지정합니다.</param>
		/// <param name="access">핸들의 접근 형식을 지정합니다.</param>
		/// <param name="packageFilePath">packageFileStream의 핸들을 열 경우 파일의 경로를 입력합니다.</param>
		private void OpenAllFileStream(FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, string packageFilePath = "")
		{
			if (packageFilePath != "") packageFileStream = new FileStream(packageFilePath, mode);
			tempPatternFileStream = new FileStream(TempPatternFilePath, mode, access);
			tempAudioFileStream = new FileStream(TempAudioFilePath, mode, access);
			tempCoverImageFileStream = new FileStream(TempCoverImageFilePath, mode, access);
		}

		/// <summary>
		/// 열려있는 모든 핸들을 닫습니다.
		/// </summary>
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

		private void Start()
		{
			backgroundCover.SetActive(false);

			displayInfo = GetComponent<DisplayInfo>();
			currentPackageInfo = GetComponent<CurrentPackageInfo>();

			if (Directory.Exists(TempPackageRootFolderPath)) Directory.Delete(TempPackageRootFolderPath, true);
			Directory.CreateDirectory(TempPackageRootFolderPath);

			//OpenAllFileStream(FileMode.OpenOrCreate);
		}

		/// <summary>
		/// 새로운 패키지를 선택하는 창을 엽니다.
		/// </summary>
		public void OpenPackageSelector()
		{
			backgroundCover.SetActive(true);
			StartCoroutine("OpenPackageSelectorCoroutine");
		}

		public IEnumerator OpenPackageSelectorCoroutine()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Package Files", ".mbt"));
			FileBrowser.SetDefaultFilter(".mbt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, @"C:\", "", "Select Pattern File...", "Load");

			if (FileBrowser.Success)
			{
				string filePath = FileBrowser.Result[0];
				currentPackageInfo.currentPackagePath = filePath;

				CloseAllFileStream();

				ZipFile.ExtractToDirectory(filePath, TempPackageRootFolderPath, true);

				OpenAllFileStream(FileMode.Open, FileAccess.ReadWrite, filePath);

				tempPatternFileStream.Close();
				SongInfo data = JsonUtility.FromJson<SongInfo>(File.ReadAllText(TempPatternFilePath));
				tempPatternFileStream = new FileStream(TempPatternFilePath, FileMode.Open, FileAccess.ReadWrite);
				currentPackageInfo.currentSongInfo = data;

				if (new FileInfo(TempCoverImageFilePath).Length == 0L) // 커버이미지 등록이 되어 있지 않은경우
				{
					currentPackageInfo.currentSongCover = null;
				}
				else
				{
					UnityWebRequest imageWebRequest = UnityWebRequestTexture.GetTexture(TempCoverImageFilePath);
					yield return imageWebRequest.SendWebRequest();
					if (imageWebRequest.result == UnityWebRequest.Result.ConnectionError)
					{
						yield return null;
					}
					else
					{
						Texture2D texture = ((DownloadHandlerTexture)imageWebRequest.downloadHandler).texture;
						Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
						currentPackageInfo.currentSongCover = sprite;
					}
				}

				using (UnityWebRequest audioWebRequest = UnityWebRequestMultimedia.GetAudioClip(TempAudioFilePath, AudioType.MPEG))
				{
					yield return audioWebRequest.SendWebRequest();
					if (audioWebRequest.result == UnityWebRequest.Result.ConnectionError)
					{
						//
					}
					else
					{
						currentPackageInfo.currentAudioClip = DownloadHandlerAudioClip.GetContent(audioWebRequest);
					}
				}
			}

			displayInfo.UpdateInfo();
			backgroundCover.SetActive(false);
		}

		private void OnDestroy()
		{
			CloseAllFileStream();

			Directory.Delete(TempPackageRootFolderPath, true);
		}
	}
}
