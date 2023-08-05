using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleSlideshow : APanel
{
	#region Fields 



	private const string k_slidesContainer = "slides-container";

	private const string k_previousButton = "previous-button";
	private const string k_startButton = "start-button";
	private const string k_nextButton = "next-button";

	private const string k_slideContainerName = "slide-container-";

	private VisualElement m_slidesContainer;

	private Button m_previousButton;
	private Button m_startButton;
	private Button m_nextButton;

	private int m_slideCount;
	private int m_slideIndex;

	private List<VisualElement> m_slides = new List<VisualElement>();

	private bool IsFirstSlide
	{
		get
		{
			return m_slideIndex == 0;
		}
	}

	private bool IsLastSlide
	{
		get
		{
			return m_slideIndex == m_slideCount - 1;
		}
	}

	#endregion

	#region Methods 

	#region MonoBehaviour

	protected override void Awake()
	{
		base.Awake();

		m_slidesContainer = m_root.Q(k_slidesContainer);

		m_previousButton = m_root.Q<Button>(k_previousButton);
		m_startButton = m_root.Q<Button>(k_startButton);
		m_nextButton = m_root.Query<Button>(k_nextButton);

		m_previousButton.clicked += CallbackPreviousClicked;
		m_startButton.clicked += CallbackStartClicked;
		m_nextButton.clicked += CallbackNextClicked;

		m_slideCount = m_slidesContainer.childCount;
		m_slideIndex = 0;

		StoreSlides();
		SelectSlide(m_slideIndex);
	}

	protected virtual void OnDestroy()
	{
		m_previousButton.clicked -= CallbackPreviousClicked;
		m_startButton.clicked -= CallbackStartClicked;
		m_nextButton.clicked -= CallbackNextClicked;
	}

	protected virtual void CallbackPreviousClicked()
	{
		m_slideIndex--;
		m_slideIndex = Mathf.Clamp(m_slideIndex, 0, m_slideCount);

		SelectSlide(m_slideIndex);
	}

	protected virtual void CallbackStartClicked()
	{
		Hide();
	}

	protected virtual void CallbackNextClicked()
	{
		m_slideIndex++;
		m_slideIndex = Mathf.Clamp(m_slideIndex, 0, m_slideCount);

		SelectSlide(m_slideIndex);
	}

	#endregion

	private void StoreSlides()
	{
		for (int i = 0; i < m_slideCount; i++)
		{
			VisualElement slide = m_slidesContainer.Q(k_slideContainerName + i);
			m_slides.Add(slide);
		}
	}

	private void SelectSlide(int slideIndex)
	{
		for (int i = 0; i < m_slideCount; i++)
		{
			VisualElement slide = m_slides[i];
			slide.style.display = slideIndex == i ? DisplayStyle.Flex : DisplayStyle.None;
		}

		ManageButtons();
	}

	private void ManageButtons()
	{
		m_previousButton.style.visibility = IsFirstSlide ? Visibility.Hidden : Visibility.Visible;
		m_startButton.style.visibility = IsLastSlide ? Visibility.Visible : Visibility.Hidden;
		m_nextButton.style.visibility = IsLastSlide ? Visibility.Hidden : Visibility.Visible;
	}

	#endregion
}