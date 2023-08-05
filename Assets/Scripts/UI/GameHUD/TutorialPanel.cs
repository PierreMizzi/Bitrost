using UnityEngine;

public class TutorialPanel : SimpleSlideshow
{

	#region Fields 

	[SerializeField]
	private LevelChannel m_levelChannel;

	[SerializeField]
	private ApplicationChannel m_applicationChannel;


	#endregion

	#region Methods 

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

	protected override void CallbackStartClicked()
	{
		base.CallbackStartClicked();
		m_applicationChannel.onSetCursor.Invoke(CursorType.Attack);

		m_levelChannel.onResumeGame.Invoke();
	}


	#endregion

}