using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using MineBeat.Preload.UI;

/*
 * [Namespace] MineBeat.Preload.Song
 * Description
 */
namespace MineBeat.Preload.Song
{
	/*
	 * [Class] PackageManager
	 * 패키지 파일의 로드를 관리합니다.
	 */
	public class PackageManager : Singleton<PackageManager>
	{
		/*
		 * [Enum] SortType
		 * List의 정렬 방식을 정의합니다.
		 */
		public enum SortType
		{
			IdAsc,
			IdDesc,
			NameAsc,
			NameDesc,
			AuthorAsc,
			AuthorDesc,
			LevelAsc,
			LevelDesc
		}

		private const string tempFileRootFolderPath = @"C:\Temp\MineBeat_PackageManager_DoNotDelete\";
		private const string packageRootFolderPath = @"C:\Temp\MineBeat_PackageManager_DoNotDelete\Packages\";
		private const string tempPackageFolderPath = @"C:\Temp\MineBeat_PackageManager_DoNotDelete\TempPackage\"; // packageRootFolderPath로 옮기기 전에 곡 정보 읽어오는 용도

		private BinaryFormatter formatter = new BinaryFormatter();

		// 실제 정보 담아두는 위치
		/* ID, PackageStream, PatternStream, AudioStream, ImageStream */
		private List<System.Tuple<ulong, FileStream, FileStream, FileStream, FileStream>> packages = new List<System.Tuple<ulong, FileStream, FileStream, FileStream, FileStream>>();

		// 파일 미리 로드해서 빼놓음 (그때그때 로드하면 스크롤 애니메이션이 안굴러감)
		/* ID, Cover, Song */
		private List<System.Tuple<ulong, Sprite, AudioClip>> preloadMedias = new List<System.Tuple<ulong, Sprite, AudioClip>>();

		// 약간 packages의 포인터 같은 느낌, 정렬은 여기서 함
		/* ID, SongName, SongAuthor, SongLevel */
		private List<System.Tuple<ulong, string, string, ushort>> sortPackages = new List<System.Tuple<ulong, string, string, ushort>>();

		private async Task<Sprite> LoadSprite(string path)
		{
			Sprite sprite = null;
			if (new FileInfo(path).Length != 0L)
			{
				using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(path))
				{
					webRequest.SendWebRequest();

					while (!webRequest.isDone) await Task.Delay(5);

					if (webRequest.result == UnityWebRequest.Result.ConnectionError)
					{
						StartCoroutine("DelayedAlert", path);
					}
					else
					{
						Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
						sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
					}
				}
			}

			return sprite;
		}

		private async Task<AudioClip> LoadClip(string path)
		{
			AudioClip clip = null;
			using (UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
			{
				webRequest.SendWebRequest();

				while (!webRequest.isDone) await Task.Delay(5);

				if (webRequest.result == UnityWebRequest.Result.ConnectionError)
				{
					StartCoroutine("DelayedAlert", path);
				}
				else
				{
					clip = DownloadHandlerAudioClip.GetContent(webRequest);
				}
			}

			return clip;
		}

		protected override async void Awake()
		{
			base.Awake();

			if (Directory.Exists(tempFileRootFolderPath)) Directory.Delete(tempFileRootFolderPath, true);
			Directory.CreateDirectory(tempFileRootFolderPath);
			Directory.CreateDirectory(packageRootFolderPath);

			if (!Directory.Exists(Application.dataPath + @"\.Patterns"))
			{
				StartCoroutine("DelayedAlert", Application.dataPath + @"/.Patterns");
				return;
			}
			string[] files = Directory.GetFiles(Application.dataPath + @"\.Patterns", "*.mbt");
			if (files.Length == 0)
			{
				StartCoroutine("DelayedAlert", Application.dataPath + @"/.Patterns");
				return;
			}
			foreach (string filePath in files)
			{
				// UnPack
				Directory.CreateDirectory(tempPackageFolderPath);
				ZipFile.ExtractToDirectory(filePath, tempPackageFolderPath, true);

				FileStream tempPatternFileStream = new FileStream(tempPackageFolderPath + "MineBeat.ptrn", FileMode.Open, FileAccess.Read);
				SongInfo data = formatter.Deserialize(tempPatternFileStream) as SongInfo;
				tempPatternFileStream.Close();

				Directory.Move(tempPackageFolderPath, packageRootFolderPath + data.id);

				// Open Handle
				FileStream packageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				FileStream patternFileStream = new FileStream(packageRootFolderPath + data.id + @"\MineBeat.ptrn", FileMode.Open, FileAccess.Read);
				FileStream audioFileStream = new FileStream(packageRootFolderPath + data.id + @"\MineBeat.adio", FileMode.Open, FileAccess.Read);
				FileStream imageFileStream = new FileStream(packageRootFolderPath + data.id + @"\MineBeat.covr", FileMode.Open, FileAccess.Read);

				// Get Medias
				Sprite sprite = await LoadSprite(imageFileStream.Name);
				AudioClip audioClip = await LoadClip(audioFileStream.Name);

				// Add List
				packages.Add(new System.Tuple<ulong, FileStream, FileStream, FileStream, FileStream>(data.id, packageFileStream, patternFileStream, audioFileStream, imageFileStream));
				preloadMedias.Add(new System.Tuple<ulong, Sprite, AudioClip>(data.id, sprite, audioClip));
				sortPackages.Add(new System.Tuple<ulong, string, string, ushort>(data.id, data.songName, data.songAuthor, data.songLevel));
			}

			Sort(SortType.NameAsc);
		}

		public IEnumerator DelayedAlert(string path)
		{
			while (SceneManager.GetActiveScene().name != "ModeSelectScene")
			{
				yield return null;
			}
			AlertManager.Instance.Show("에러", "곡 정보를 불러오지 못했습니다.\n\nPath: " + path, AlertManager.AlertButtonType.Single, new string[] { "닫기" }, CloseProcess);
		}

		private void Update()
		{
			if (SceneManager.GetActiveScene().name == "GameEditorScene") Destroy(gameObject);
		}

		/*
		 * [Method] Sort(SortType sortType): void
		 * 곡 목록을 정렬합니다.
		 * 
		 * <SortType sortType>
		 * 정렬 기준을 입력합니다.
		 */
		public void Sort(SortType sortType)
		{
			switch (sortType)
			{
				case SortType.IdAsc:
					sortPackages = sortPackages.OrderBy(target => target.Item1).ToList();
					break;
				case SortType.IdDesc:
					sortPackages = sortPackages.OrderByDescending(target => target.Item1).ToList();
					break;
				case SortType.NameAsc:
					sortPackages = sortPackages.OrderBy(target => target.Item2).ToList();
					break;
				case SortType.NameDesc:
					sortPackages = sortPackages.OrderByDescending(target => target.Item2).ToList();
					break;
				case SortType.AuthorAsc:
					sortPackages = sortPackages.OrderBy(target => target.Item3).ToList();
					break;
				case SortType.AuthorDesc:
					sortPackages = sortPackages.OrderByDescending(target => target.Item3).ToList();
					break;
				case SortType.LevelAsc:
					sortPackages = sortPackages.OrderBy(target => target.Item4).ToList();
					break;
				case SortType.LevelDesc:
					sortPackages = sortPackages.OrderByDescending(target => target.Item4).ToList();
					break;
			}
		}

		/*
		 * [Method] GetAllPackageId(): List<ulong>
		 * 현재 로드된 패키지에 대한 ID를 반환합니다.
		 * 
		 * <RETURN: List<ulong>>
		 * 패키지에 대한 ID를 담습니다.
		 */
		public List<ulong> GetAllPackageId()
		{
			List<ulong> returnValue = new List<ulong>();

			foreach (var sortPackage in sortPackages)
			{
				returnValue.Add(packages.Find(target => target.Item1 == sortPackage.Item1).Item1);
			}

			return returnValue;
		}

		/*
		 * [Method] GetSongInfo(ulong id): SongInfo
		 * 특정한 곡의 SongInfo를 반환합니다.
		 * 
		 * <ulong id>
		 * 찾을 곡의 ID를 입력합니다.
		 * 
		 * <RETURN: SongInfo>
		 * 해당되는 ID를 가진 곡의 SongInfo를 반환합니다.
		 */
		public SongInfo GetSongInfo(ulong id)
		{
			var target = packages.Find(target => target.Item1 == id);
			target.Item3.Position = 0;
			SongInfo data = formatter.Deserialize(target.Item3) as SongInfo;
			return data;
		}

		/*
		 * [Method] GetFileStream(ulong id): System.Tuple<Sprite, AudioClip>
		 * 특정한 곡의 커버이미지와 AudioClip을 반환합니다.
		 * 
		 * <ulong id>
		 * 찾을 곡의 ID를 입력합니다.
		 * 
		 * <RETURN: System.Tuple<Sprite, AudioClip>>
		 * 해당되는 ID를 가진 곡의 커버이미지와 AudioClip을 반환합니다.
		 */
		public System.Tuple<Sprite, AudioClip> GetMedias(ulong id)
		{
			var target = preloadMedias.Find(target => target.Item1 == id);
			return new System.Tuple<Sprite, AudioClip>(target.Item2, target.Item3);
		}

		/*
		 * [Method] GetFileStream(ulong id): List<FileStream>
		 * 특정한 곡의 열려있는 모든 핸들을 반환합니다.
		 * 
		 * <ulong id>
		 * 찾을 곡의 ID를 입력합니다.
		 * 
		 * <RETURN: List<FileStream>>
		 * 해당되는 ID를 가진 곡의 모든 핸들을 반환합니다.
		 */
		public List<FileStream> GetFileStream(ulong id)
		{
			var target = packages.Find(target => target.Item1 == id);
			return new List<FileStream>() { target.Item2, target.Item3, target.Item4, target.Item5 };
		}

		/*
		 * [Method] CloseProcess(): void
		 * 게임을 종료합니다.
		 */
		public void CloseProcess()
		{
			#if UNITY_EDITOR
				EditorApplication.ExecuteMenuItem("Edit/Play");
			#else
				Application.Quit();
			#endif
		}

		private void OnDestroy()
		{
			foreach (var package in packages)
			{
				package.Item2.Close();
				package.Item3.Close();
				package.Item4.Close();
				package.Item5.Close();
			}

			Directory.Delete(tempFileRootFolderPath, true);
		}
	}
}
