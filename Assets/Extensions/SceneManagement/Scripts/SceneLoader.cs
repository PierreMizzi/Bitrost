using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace PierreMizzi.Useful.SceneManagement
{
	public static class SceneLoader
	{

		public static IEnumerator LoadScene(string sceneName, bool isActiveScene, Action<float> onProgress = null, Action onComplete = null)
		{
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!operation.isDone)
			{
				onProgress?.Invoke(operation.progress);
				yield return null;
			}

			onComplete?.Invoke();

			if (isActiveScene)
			{
				Scene scene = SceneManager.GetSceneByName(sceneName);
				SceneManager.SetActiveScene(scene);
			}
		}

		public static IEnumerator UnloadScene(string sceneName, Action<float> onProgress = null, Action onComplete = null)
		{
			AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
			while (!operation.isDone)
			{
				onProgress?.Invoke(operation.progress);
				yield return null;
			}

			onComplete?.Invoke();
		}

	}
}