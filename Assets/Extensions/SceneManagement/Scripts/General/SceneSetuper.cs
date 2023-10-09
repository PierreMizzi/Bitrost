using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

namespace PierreMizzi.Useful.SceneManagement
{
	[ExecuteInEditMode, Obsolete]
	public class SceneSetuper : MonoBehaviour
	{

		[SerializeField]
		private string m_scenesPath = "Assets/Scenes/Application/";
		public string scenesPath => m_scenesPath;

		[SerializeField]
		private List<string> m_additionalScenes = new List<string>();

		public List<string> additionalScenes => m_additionalScenes;

		private void OnEnable()
		{
#if UNITY_EDITOR
			if (SceneManager.sceneCount == 1)
				OpenAdditionnalScene(m_scenesPath, m_additionalScenes.ToArray());
			// SceneSetupShortcuts.OpenAdditionnalScene(m_scenesPath, m_additionalScenes.ToArray());
#endif
		}

		public void OpenAdditionnalScene(string path, params string[] sceneNames)
		{
			string fullPath;
			int length = sceneNames.Length;
			for (int i = 0; i < length; i++)
			{
				fullPath = path + sceneNames[i] + ".unity";
				// SceneManager.LoadScene(sceneNames[i], LoadSceneMode.Additive);
				SceneManager.LoadScene(sceneNames[i], LoadSceneMode.Additive);
			}
		}

	}
}