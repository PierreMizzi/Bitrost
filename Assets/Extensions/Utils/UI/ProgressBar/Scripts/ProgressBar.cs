using UnityEngine.UIElements;

namespace PierreMizzi.Useful.UIToolkit
{

	public class ProgressBar
	{

		#region Fields 

		protected VisualElement m_root;
		private const string k_progressFill = "progress-bar-fill";
		private const string k_progressText = "progress-bar-text";

		protected VisualElement m_progressFill;
		protected Label m_progressText;

		protected Length m_progressBarLength;

		public float progress { get; private set; }

		#endregion

		#region Methods 

		public ProgressBar(VisualElement container, string m_rootName)
		{
			m_root = container.Q(m_rootName);

			m_progressBarLength = new Length(0, LengthUnit.Percent);

			m_progressFill = m_root.Q<VisualElement>(k_progressFill);
			m_progressText = m_root.Q<Label>(k_progressText);

			SetProgress(0);
		}

		public virtual void SetProgress(float progress)
		{
			this.progress = progress;

			// Fill
			m_progressBarLength.value = (1f - this.progress) * 100;
			m_progressFill.style.right = m_progressBarLength;

			// Text
			m_progressText.text = this.progress * 100f + "%";
		}


		public virtual void Display()
		{
			m_root.style.display = DisplayStyle.Flex;
		}


		public virtual void Hide()
		{
			m_root.style.display = DisplayStyle.None;
		}


		#endregion

	}

}