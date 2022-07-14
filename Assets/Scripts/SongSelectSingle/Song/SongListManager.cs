using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

using MineBeat;

/*
 * [Namespace] MineBeat.SongSelectSingle.Song
 * Description
 */
namespace MineBeat.SongSelectSingle.Song
{
	/*
	 * [Class] SongListManager
	 * 전체 곡 목록을 관리합니다.
	 */
	public class SongListManager : MonoBehaviour
	{
		private readonly string tempFileRootFolderPath = @"C:\Temp\MineBeat_SongSelect_DoNotDelete\";
		private readonly string packageRootFolderPath = @"C:\Temp\MineBeat_SongSelect_DoNotDelete\Packages\";
		private readonly string tempPackageFolderPath = @"C:\Temp\MineBeat_SongSelect_DoNotDelete\TempPackage\"; // packageRootFolderPath로 옮기기 전에 곡 정보 읽어오는 용도

		private void Start()
		{
			if (Directory.Exists(tempFileRootFolderPath)) Directory.Delete(tempFileRootFolderPath, true);
			Directory.CreateDirectory(tempFileRootFolderPath);
			Directory.CreateDirectory(packageRootFolderPath);

			BinaryFormatter formatter = new BinaryFormatter();

			string[] files = Directory.GetFiles(Application.dataPath + @"\.Patterns", "*.mbt");
			for (int i = 0; i < files.Length; i++)
			{
				Directory.CreateDirectory(tempPackageFolderPath);
				ZipFile.ExtractToDirectory(files[i], tempPackageFolderPath, true);

				FileStream tempPatternFileStream = new FileStream(tempPackageFolderPath+"MineBeat.ptrn", FileMode.Open, FileAccess.Read);
				SongInfo data = formatter.Deserialize(tempPatternFileStream) as SongInfo;
				tempPatternFileStream.Close();

				Directory.Move(tempPackageFolderPath, packageRootFolderPath + data.id);
			}
		}

		private void Update()
		{

		}

		private void OnDestroy()
		{
			Directory.Delete(tempFileRootFolderPath, true);
		}
	}
}
