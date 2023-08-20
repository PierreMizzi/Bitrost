namespace PierreMizzi.Useful.SceneManagement
{
	using System;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	[ExecuteInEditMode]
	public class BaseAppManager : MonoBehaviour
	{

		#region Fields 

		[SerializeField]
		protected BaseAppChannel m_appChannel;

		[Header("Scene Management")]
		[SerializeField]
		protected SceneLoaderScreen m_loaderScreen;

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
			bool hold = true;
			Action stopHold = () => { hold = false; };

			m_loaderScreen.Display();

			yield return SceneLoader.LoadScene(titlecardSceneName, true, m_loaderScreen.SetProgress);

			m_initialCamera.gameObject.SetActive(false);

			yield return new WaitForSeconds(1f);

			m_loaderScreen.HideProgressBar();
			m_loaderScreen.FadeOut(3f, stopHold);

			while (hold)
				yield return null;

			Debug.Log("Titlecard scene loaded");
		}

		protected virtual void TitlecardToGame()
		{
			IEnumerator transition = SceneTransitionCoroutine(titlecardSceneName, gameSceneName);
			StartCoroutine(transition);
		}

		private void GameToTitlecard()
		{
			IEnumerator transition = SceneTransitionCoroutine(gameSceneName, titlecardSceneName);
			StartCoroutine(transition);
		}

		protected virtual IEnumerator SceneTransitionCoroutine(string previousSceneName, string newSceneName)
		{
			bool hold = true;
			Action stopHold = () => { hold = false; };

			// Fade In
			m_loaderScreen.HideProgressBar();
			m_loaderScreen.FadeIn(1, stopHold);

			while (hold)
				yield return null;

			m_initialCamera.gameObject.SetActive(true);
			m_loaderScreen.DisplayProgressBar();
			// Unload Game Scene
			yield return SceneLoader.UnloadScene(previousSceneName);
			GC.Collect();

			// Loads Titlecard scene
			yield return SceneLoader.LoadScene(newSceneName, true, m_loaderScreen.SetProgress);
			m_initialCamera.gameObject.SetActive(false);
			m_loaderScreen.HideProgressBar();

			// Fade Out
			hold = true;
			m_loaderScreen.FadeOut(1, stopHold);

			while (hold)
				yield return null;

			Debug.Log("Titlecard scene loaded");
		}

		#endregion

	}

}