using UnityEngine;
using UnityEngine.UIElements;

namespace PierreMizzi.Useful.SceneManagement
{

	public class BaseTitlecardManager : MonoBehaviour
	{

		#region Fields 

		[Header("Channels")]
		[SerializeField]
		private BaseAppChannel m_appChannel;

		[Header("UIDocument")]
		[SerializeField]
		private UIDocument m_document;

		private VisualElement m_root;

		private const string k_startButton = "start-button";
		private const string k_exitButton = "exit-button";

		private Button m_startButton;
		private Button m_exitButton;

		#endregion

		#region Methods 

		protected void Awake()
		{
			m_root = m_document.rootVisualElement;

			m_startButton = m_root.Q<Button>(k_startButton);
			m_exitButton = m_root.Q<Button>(k_exitButton);

			m_startButton.clicked += CallbackStartClicked;
			m_exitButton.clicked += CallbackExitClicked;
		}

		protected void OnDestroy()
		{
			m_startButton.clicked -= CallbackStartClicked;
			m_exitButton.clicked -= CallbackExitClicked;
		}

		private void CallbackStartClicked()
		{
			m_appChannel.onTitlecardToGame.Invoke();
		}

		private void CallbackExitClicked()
		{
			Application.Quit();
		}

		#endregion

	}

}