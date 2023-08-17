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
			if (SceneManager.sceneCount == 1)
			{
				ApplicationToTitlecard();
			}
			else
			{
				m_loaderScreen.Awake();
				m_loaderScreen.Hide();
				DestroyImmediate(m_initialCamera.gameObject);
			}
		}

		protected virtual void Start()
		{
			if (m_appChannel != null)
				m_appChannel.onTitlecardToGame += TitlecardToGame;
		}

		protected virtual void OnDestroy()
		{
			if (m_appChannel != null)
				m_appChannel.onTitlecardToGame -= TitlecardToGame;
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

			if (m_initialCamera != null)
				Destroy(m_initialCamera.gameObject);

			yield return new WaitForSeconds(1f);

			m_loaderScreen.HideProgressBar();
			m_loaderScreen.FadeOut(3f, stopHold);

			while (hold)
				yield return null;

			Debug.Log("Titlecard scene loaded");
		}

		protected virtual void TitlecardToGame()
		{
			StartCoroutine(TitlecardToGameCoroutine());
		}

		protected virtual IEnumerator TitlecardToGameCoroutine()
		{
			// Fade In of screen
			bool hold = true;
			Action stopHold = () => { hold = false; };

			m_loaderScreen.FadeIn(1f, stopHold);

			while (hold)
				yield return null;

			m_loaderScreen.DisplProgressBar();
			yield return SceneLoader.UnloadScene(titlecardSceneName);
			yield return SceneLoader.LoadScene(gameSceneName, true, m_loaderScreen.SetProgress);

			m_loaderScreen.HideProgressBar();
			m_loaderScreen.FadeOut(1f, stopHold);

			while (hold)
				yield return null;

			Debug.Log("Game scene loaded");
		}

		#endregion

	}

}