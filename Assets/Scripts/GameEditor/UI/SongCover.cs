using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Files;

namespace MineBeat.GameEditor.UI
{
	/// <summary>
	/// 앨범 커버의 표시를 관리합니다.
	/// </summary>
	public class SongCover : MonoBehaviour
	{
		[SerializeField]
		private Image image;

		[SerializeField]
		private Sprite defaultImage;

		private Sprite currentImage = null;

		private FileManager fileManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			fileManager = managers.Find(target => target.name == "FileManager").GetComponent<FileManager>();
		}

		/// <summary>
		/// 이미지가 클릭되었을 때 파일 선택 창을 띄웁니다.
		/// </summary>
		public void OnImageClicked()
		{
			StartCoroutine(fileManager.OpenImageFileCoroutine());
		}

		/// <summary>
		/// 이미지를 갱신합니다.
		/// </summary>
		/// <param name="sprite">갱신할 이미지를 지정합니다. 이미지를 지정하지 않을 경우(null), defaultImage가 출력됩니다.</param>
		public void UpdateImage(Sprite sprite = null)
		{
			currentImage = sprite;
			image.sprite = currentImage == null ? defaultImage : currentImage;
		}

		/// <summary>
		/// 현재 이미지를 반환합니다.
		/// </summary>
		/// <returns>현재 이미지를 반환합니다. 이미지가 지정되어 있지 않을 경우, defaultImage를 반환합니다.</returns>
		public Sprite GetImage()
		{
			return currentImage == null ? defaultImage : currentImage;
		}
	}
}
