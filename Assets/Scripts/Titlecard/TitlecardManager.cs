using UnityEngine;
using UnityEngine.UIElements;

namespace Bitfrost.Application.UI
{

	public class TitlecardManager : MonoBehaviour
	{

		#region Fields 

		[SerializeField]
		private UIDocument m_document = null;

		private const string k_startButton = "start-button";
		private const string k_scoresButton = "score-button";
		private const string k_exitButton = "exit-button";

		private VisualElement m_startButton;
		private VisualElement m_scoreButton;
		private VisualElement m_exitButton;

		#endregion

		#region Methods 

		protected void Awake()
		{

		}

		#endregion

	}

}