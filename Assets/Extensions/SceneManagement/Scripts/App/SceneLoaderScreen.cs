using PierreMizzi.Useful.UI;
using UnityEngine;
using UnityEngine.UIElements;
using ProgressBar = PierreMizzi.Useful.UIToolkit.ProgressBar;

namespace PierreMizzi.Useful.SceneManagement
{
	public class SceneLoaderScreen : FadeScreen
	{
		#region 

		public float fadeInDuration = 0.5f;

		private const string k_progressBar = "progress-bar-container";
		protected ProgressBar m_progressBar;

		#endregion

		public override void Awake()
		{
			base.Awake();
			m_progressBar = new ProgressBar(m_root, k_progressBar);
		}

		public override void Display()
		{
			base.Display();
			DisplayProgressBar();
		}

		public override void Hide()
		{
			base.Hide();
			HideProgressBar();
		}

		public void DisplayProgressBar()
		{
			m_progressBar.Display();
		}

		public void HideProgressBar()
		{
			m_progressBar.Hide();
		}

		public void SetProgress(float progress)
		{
			m_progressBar.SetProgress(progress);
		}

		[ContextMenu("Test FadeIn")]
		public void TestFadeIn()
		{
			FadeIn(1f);
		}

		[ContextMenu("Test FadeOut")]
		public void TestFadeOut()
		{
			FadeOut(1f);
		}


	}
}