using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PierreMizzi.Useful.SceneManagement
{

	/// <summary> 
	///	Scene to go from one scene to another
	/// </summary>
	[ExecuteInEditMode]
	public class BaseAppManager : MonoBehaviour
	{

		#region Fields 

		[SerializeField]
		protected BaseAppChannel m_appChannel;

		[Header("Scene Management")]
		[SerializeField]
		protected SceneLoaderScreen m_loaderScreen;

		/// <summary>
		/// Camera displaying loading screen between scene loading
		/// </summary>
		[SerializeField]
		protected Camera m_initialCamera = null;

		public static string applicationSceneName = "Application";
		public static string titlecardSceneName = "Titlecard";
		public static string gameSceneName = "Game";

		#endregion

		#region Methods 

		protected virtual void OnEnable()
		{
			StartCoroutine(SceneSetup());
		}

		protected virtual IEnumerator SceneSetup()
		{
			// When booting the game, loads the Titlecard scene
			if (SceneManager.sceneCount == 1)
			{
				m_initialCamera.gameObject.SetActive(true);

				if (Application.isPlaying)
					ApplicationToTitlecard();
			}
			else
			{
				yield return new WaitForEndOfFrame();
				m_loaderScreen.Awake();
				m_loaderScreen.Hide();

				m_initialCamera.gameObject.SetActive(false);
			}

			yield return null;
		}

		protected virtual void Start()
		{
			if (m_appChannel != null)
			{
				m_appChannel.onTitlecardToGame += TitlecardToGame;
				m_appChannel.onGameToTitlecard += GameToTitlecard;
			}
		}

		protected virtual void OnDestroy()
		{
			if (m_appChannel != null)
			{
				m_appChannel.onTitlecardToGame -= TitlecardToGame;
				m_appChannel.onGameToTitlecard -= GameToTitlecard;
			}

		}

		protected virtual void ApplicationToTitlecard()
		{
			StartCoroutine(ApplicationToTitlecardCoroutine());
		}

		protected virtual IEnumerator ApplicationToTitlecardCoroutine()
		{
			// Display SceneLoaderScreen
			m_loaderScreen.Display();

			yield return SceneLoader.LoadScene(titlecardSceneName, true, m_loaderScreen.SetProgress);

			m_initialCamera.gameObject.SetActive(false);

			yield return new WaitForSeconds(1f);

			m_loaderScreen.HideProgressBar();
			m_loaderScreen.FadeOut(3f);
		}

		protected virtual void TitlecardToGame()
		{
			IEnumerator transition = SceneTransitionCoroutine(titlecardSceneName, gameSceneName, m_appChannel.onUnloadTitlecardScene.Invoke());
			StartCoroutine(transition);
		}

		private void GameToTitlecard()
		{
			IEnumerator transition = SceneTransitionCoroutine(gameSceneName, titlecardSceneName, m_appChannel.onUnloadGameScene.Invoke());
			StartCoroutine(transition);
		}

		/// <summary>
		/// Transition from one scene to another
		/// </summary>
		/// <param name="previousSceneName"></param>
		/// <param name="newSceneName"></param>
		/// <param name="previousSceneUnloading">previous scene unloading behaviour</param>
		/// <returns></returns>
		protected virtual IEnumerator SceneTransitionCoroutine(string previousSceneName, string newSceneName, IEnumerator previousSceneUnloading = null)
		{
			bool hold = true;
			Action stopHold = () => { hold = false; };

			// Fade in to hide the current scene
			m_loaderScreen.HideProgressBar();
			m_loaderScreen.FadeIn(1, stopHold);

			while (hold)
				yield return null;

			if (previousSceneUnloading != null)
				yield return previousSceneUnloading;

			// Loading screen displays loading progress
			m_initialCamera.gameObject.SetActive(true);
			m_loaderScreen.DisplayProgressBar();

			// Unload previous scene
			yield return SceneLoader.UnloadScene(previousSceneName);
			GC.Collect();

			// Loads next scene
			yield return SceneLoader.LoadScene(newSceneName, true, m_loaderScreen.SetProgress);
			m_initialCamera.gameObject.SetActive(false);
			m_loaderScreen.HideProgressBar();

			// Fade Out, display newly loaded scene
			hold = true;
			m_loaderScreen.FadeOut(1);
		}

		#endregion

	}

}