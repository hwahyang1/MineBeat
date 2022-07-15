using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * [Namespace] MineBeat.SongSelectSingle.Song
 * Description
 */
namespace MineBeat.SongSelectSingle.Song
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
			//IdDesc,
			NameAsc,
			//NameDesc,
			AuthorAsc,
			//AuthorDesc,
			LevelAsc,
			//LevelDesc
		}

		private readonly string tempFileRootFolderPath = @"C:\Temp\MineBeat_Single_DoNotDelete\";
		private readonly string packageRootFolderPath = @"C:\Temp\MineBeat_Single_DoNotDelete\Packages\";
		private readonly string tempPackageFolderPath = @"C:\Temp\MineBeat_Single_DoNotDelete\TempPackage\"; // packageRootFolderPath로 옮기기 전에 곡 정보 읽어오는 용도

		private BinaryFormatter formatter = new BinaryFormatter();

		// 실제 정보 담아두는 위치
		/* ID, PackageStream, PatternStream, AudioStream, ImageStream */
		private List<System.Tuple<ulong, FileStream, FileStream, FileStream, FileStream>> packages = new List<System.Tuple<ulong, FileStream, FileStream, FileStream, FileStream>>();

		// 약간 packages의 포인터 같은 느낌, 정렬은 여기서 함
		/* ID, SongName, SongAuthor, SongLevel */
		private List<System.Tuple<ulong, string, string, ushort>> sortPackages = new List<System.Tuple<ulong, string, string, ushort>>();

		protected override void Awake()
		{
			base.Awake();

			DontDestroyOnLoad(gameObject);

			if (Directory.Exists(tempFileRootFolderPath)) Directory.Delete(tempFileRootFolderPath, true);
			Directory.CreateDirectory(tempFileRootFolderPath);
			Directory.CreateDirectory(packageRootFolderPath);

			string[] files = Directory.GetFiles(Application.dataPath + @"\.Patterns", "*.mbt");
			foreach (string filePath in files)
			{
				Directory.CreateDirectory(tempPackageFolderPath);
				ZipFile.ExtractToDirectory(filePath, tempPackageFolderPath, true);

				FileStream tempPatternFileStream = new FileStream(tempPackageFolderPath + "MineBeat.ptrn", FileMode.Open, FileAccess.Read);
				SongInfo data = formatter.Deserialize(tempPatternFileStream) as SongInfo;
				tempPatternFileStream.Close();

				Directory.Move(tempPackageFolderPath, packageRootFolderPath + data.id);

				FileStream packageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				FileStream patternFileStream = new FileStream(packageRootFolderPath + data.id + @"\MineBeat.ptrn", FileMode.Open, FileAccess.Read);
				FileStream audioFileStream = new FileStream(packageRootFolderPath + data.id + @"\MineBeat.adio", FileMode.Open, FileAccess.Read);
				FileStream imageFileStream = new FileStream(packageRootFolderPath + data.id + @"\MineBeat.covr", FileMode.Open, FileAccess.Read);

				packages.Add(new System.Tuple<ulong, FileStream, FileStream, FileStream, FileStream>(data.id, packageFileStream, patternFileStream, audioFileStream, imageFileStream));
				sortPackages.Add(new System.Tuple<ulong, string, string, ushort>(data.id, data.songName, data.songAuthor, data.songLevel));
			}

			Sort(SortType.NameAsc);
		}

		private void Update()
		{
			if (SceneManager.GetActiveScene().name != "SongSelectSingleScene" &&
				SceneManager.GetActiveScene().name != "InGameSingleScene" &&
				SceneManager.GetActiveScene().name != "ResultSingleScene")
				Destroy(gameObject);
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
					sortPackages.OrderBy(target => target.Item1);
					break;
				case SortType.NameAsc:
					sortPackages.OrderBy(target => target.Item2);
					break;
				case SortType.AuthorAsc:
					sortPackages.OrderBy(target => target.Item3);
					break;
				case SortType.LevelAsc:
					sortPackages.OrderBy(target => target.Item4);
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
		 * [Method] GetFileStream(ulong id): SongInfo
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
