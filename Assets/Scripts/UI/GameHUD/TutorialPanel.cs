using PierreMizzi.SoundManager;
using UnityEngine;
using UnityEngine.UIElements;

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

		m_root.RegisterCallback<MouseOverEvent>(CallbackOnMouseOver);
	}

	protected void Start()
	{
		if (m_levelChannel != null)
			m_levelChannel.onDisplayTutorial += Display;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (m_levelChannel != null)
			m_levelChannel.onDisplayTutorial -= Display;
	}

	protected override void Display()
	{
		base.Display();
		m_applicationChannel.onSetCursor.Invoke(CursorType.Normal);

		m_levelChannel.onPauseGame.Invoke();
	}

	protected override void CallbackPreviousClicked()
	{
		base.CallbackPreviousClicked();
		SoundManager.PlaySound(SoundDataIDStatic.U_I_CLICK);

	}

	protected override void CallbackStartClicked()
	{
		base.CallbackStartClicked();
		m_applicationChannel.onSetCursor.Invoke(CursorType.Attack);

		m_levelChannel.onResumeGame.Invoke();

		SoundManager.PlaySound(SoundDataIDStatic.U_I_START_CLICK);
	}

	protected override void CallbackNextClicked()
	{
		base.CallbackNextClicked();
		SoundManager.PlaySound(SoundDataIDStatic.U_I_CLICK);
	}

	private void CallbackOnMouseOver(MouseOverEvent evt)
	{
		VisualElement element = (VisualElement)evt.currentTarget;

		if (element == m_previousButton ||
			element == m_startButton ||
			element == m_nextButton
		)
		{
			SoundManager.PlaySound(SoundDataIDStatic.U_I_HOVER);
		}
	}


	#endregion

}