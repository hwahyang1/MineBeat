using System.IO;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.Preload.Config
 * Description
 */
namespace MineBeat.Preload.Config
{
	/*
	 * [Class] ConfigManager
	 * 설정 파일의 입출력과 설정 적용을 관리합니다.
	 */
	public class ConfigManager : Singleton<ConfigManager>
	{
		private string _configFilePath;
		private string configFilePath
		{
			get { return _configFilePath; }
			set { if (!protectModify) _configFilePath = value; }
		}
		private bool protectModify = false;

		FileStream configFileStream;

		protected override void Awake()
		{
			base.Awake();

			configFilePath = Application.persistentDataPath + @"\Minebeat.config";
			protectModify = true;
		}

		private void Start()
		{

		}

		private void Update()
		{

		}
	}
}
