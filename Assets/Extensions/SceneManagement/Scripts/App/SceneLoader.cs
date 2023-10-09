using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace PierreMizzi.Useful.SceneManagement
{
	public static class SceneLoader
	{

		/// <summary>
		/// Loads a scene asynchronously
		/// </summary>
		/// <param name="sceneName">Name of the scene to load</param>
		/// <param name="isActiveScene">When loading is completed, is it the active one ?</param>
		/// <param name="onProgress">Loading progress callback</param>
		/// <param name="onComplete">Loading completed callback</param>
		/// <returns></returns>
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

		/// <summary>
		/// Unloads a scene asynchronously
		/// </summary>
		/// <param name="sceneName">Name of the scene to load</param>
		/// <param name="onProgress">Loading progress callback</param>
		/// <param name="onComplete">Loading completed callback</param>
		/// <returns></returns>
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