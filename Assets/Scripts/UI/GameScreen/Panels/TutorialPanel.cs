using Bitfrost.Application;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bitfrost.Gameplay.UI
{

	public class TutorialPanel : SimpleSlideshow
	{

		#region Fields 

		[SerializeField]
		private LevelChannel m_levelChannel;

		[SerializeField]
		private ApplicationChannel m_applicationChannel;


		#endregion

		#region Methods 

		protected override void Awake()
		{
			base.Awake();

			m_previousButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
			m_startButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
			m_nextButton.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
		}

		protected void Start()
		{
			if (m_levelChannel != null)
				m_levelChannel.onDisplayTutorialPanel += Display;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (m_levelChannel != null)
				m_levelChannel.onDisplayTutorialPanel -= Display;
		}

		protected override void Display()
		{
			base.Display();
			m_applicationChannel.onSetCursor.Invoke(CursorType.Normal);
		}

		protected override void CallbackPreviousClicked()
		{
			base.CallbackPreviousClicked();
			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

		protected override void CallbackStartClicked()
		{
			base.CallbackStartClicked();
			m_levelChannel.onTutorialStartClicked.Invoke();
			SoundManager.PlaySFX(SoundDataID.U_I_START_CLICK);
		}

		protected override void CallbackNextClicked()
		{
			base.CallbackNextClicked();
			SoundManager.PlaySFX(SoundDataID.U_I_CLICK);
		}

		private void CallbackOnMouseOver(MouseOverEvent evt)
		{
			SoundManager.PlaySFX(SoundDataID.U_I_HOVER);
		}


		#endregion

	}
}