using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace PierreMizzi.Useful.SceneManagement
{

	public class BaseTitlecardManager : MonoBehaviour
	{

		#region Fields 

		[Header("Channels")]
		[SerializeField]
		protected BaseAppChannel m_appChannel;

		[Header("UIDocument")]
		[SerializeField]
		protected UIDocument m_document;

		protected VisualElement m_root;

		protected const string k_startButton = "start-button";
		protected const string k_exitButton = "exit-button";

		protected Button m_startButton;
		protected Button m_exitButton;

		public bool isInteractable { get; set; } = true;

		#endregion

		#region Methods 

		protected virtual void Awake()
		{
			m_root = m_document.rootVisualElement;

			m_startButton = m_root.Q<Button>(k_startButton);
			m_exitButton = m_root.Q<Button>(k_exitButton);

			m_startButton.clicked += CallbackStartClicked;
			m_exitButton.clicked += CallbackExitClicked;
		}

		protected virtual void Start()
		{
			m_appChannel.onUnloadTitlecardScene += UnloadTitlecardSceneCoroutine;
		}

		protected virtual void OnDestroy()
		{
			m_startButton.clicked -= CallbackStartClicked;
			m_exitButton.clicked -= CallbackExitClicked;

			m_appChannel.onUnloadTitlecardScene -= UnloadTitlecardSceneCoroutine;
		}

		protected virtual void CallbackStartClicked()
		{
			if (isInteractable)
				m_appChannel.onTitlecardToGame.Invoke();
		}

		protected virtual void CallbackExitClicked()
		{
			if (isInteractable)
				Application.Quit();
		}

		protected virtual IEnumerator UnloadTitlecardSceneCoroutine()
		{
			Debug.Log("Unload Titlecard Scene Coroutine");
			yield return null;
		}

		#endregion

	}

}