using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.GameEditor.Files;

/*
 * [Namespace] Minebeat.GameEditor.UI
 * Desciption
 */
namespace MineBeat.GameEditor.UI
{
	/*
	 * [Class] SongCover
	 * 앨범 커버의 표시를 관리합니다.
	 */
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
			fileManager = GameObject.Find("FileManager").GetComponent<FileManager>();
		}

		/*
		 * [Method] OnImageClicked(): void
		 * 이미지가 클릭되었을 때 파일 선택 창을 띄웁니다.
		 */
		public void OnImageClicked()
		{
			StartCoroutine(fileManager.OpenImageFileCoroutine());
		}

		/*
		 * [Method] UpdateImage(Sprite sprite = null): void
		 * 이미지를 갱신합니다.
		 * 
		 * <Sprite sprite = null>
		 * 갱신할 이미지를 지정합니다.
		 * 이미지를 지정하지 않을 경우(null), defaultImage가 출력됩니다.
		 */
		public void UpdateImage(Sprite sprite = null)
		{
			currentImage = sprite;
			image.sprite = currentImage == null ? defaultImage : currentImage;
		}

		/*
		 * [Method] GetImage(): Sprite
		 * 현재 이미지를 반환합니다.
		 * 
		 * <RETURN: Sprite>
		 * 현재 이미지를 반환합니다.
		 * 이미지가 지정되어 있지 않을 경우, defaultImage를 반환합니다.
		 */
		public Sprite GetImage()
		{
			return currentImage == null ? defaultImage : currentImage;
		}
	}
}
