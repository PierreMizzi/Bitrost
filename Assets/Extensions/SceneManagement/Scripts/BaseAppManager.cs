namespace PierreMizzi.Useful.SceneManagement
{
	using System.Collections;

	/*

		- Titlecard.scene
			- Start
				- LoadScreen.cs
					-> FadeIn
					-> Load GameScene Additive, progress, sceneActive
					-> Unload Titlecard
					-> FadeOut
			- Scoreboard
				
			- Exit	
				-> Quit the god damn game


		- ApplicationManager.cs
		- ApplicationChannel.cs
		- FadeScreen.cs
			- ScreenLoader.cs : ScreenFader
		- TitlecardManager.cs
		- TitlecardChannel.cs
	
	*/


	using UnityEngine;

	public class BaseAppManager : MonoBehaviour
	{

		#region Fields 

		[SerializeField]
		protected BaseAppChannel m_applicationChannel;

		[Header("Scene Management")]
		[SerializeField]
		protected SceneLoaderScreen m_loaderScreen;

		[SerializeField]
		protected Camera m_initialCamera = null;

		protected const string k_titlecardSceneName = "Titlecard";

		#endregion

		#region Methods 

		protected virtual void Start()
		{
			ApplicationToTitlecard();
		}

		protected virtual void ApplicationToTitlecard()
		{
			StartCoroutine(ApplicationToTitlecardCoroutine());
		}

		protected virtual IEnumerator ApplicationToTitlecardCoroutine()
		{
			// Display SceneLoaderScreen
			m_loaderScreen.Display();

			yield return SceneLoader.LoadScene(k_titlecardSceneName, true, m_loaderScreen.SetProgress);

			if (m_initialCamera != null)
				Destroy(m_initialCamera.gameObject);

			yield return new WaitForSeconds(1f);

			m_loaderScreen.HideProgressBar();
			bool hold = true;
			m_loaderScreen.FadeOut(3f, () => { hold = false; });

			while (hold)
				yield return null;

			Debug.Log("Titlecard scene loaded");
		}

		#endregion

	}

}