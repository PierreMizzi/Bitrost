using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace PierreMizzi.Useful.UI
{
	public class FadeScreen : MonoBehaviour
	{
		#region Fields 

		[SerializeField]
		protected UIDocument m_document;

		protected VisualElement m_root;

		protected const string k_background = "fade-screen-background";

		protected VisualElement m_background;

		#endregion

		#region Methods 

		protected virtual void Awake()
		{
			m_root = m_document.rootVisualElement;
			m_background = m_root.Q<VisualElement>(k_background);
		}

		public virtual void Display()
		{
			m_background.style.opacity = 1f;
			m_background.style.display = DisplayStyle.Flex;
		}

		public virtual Tween FadeIn(float duration, Action onComplete = null)
		{
			if (m_background == null)
				return null;

			m_background.style.opacity = 0f;
			m_background.style.display = DisplayStyle.Flex;

			return DOVirtual
				.Float(
					0f,
					1f,
					duration,
					(float value) =>
					{
						m_background.style.opacity = value;
						Debug.Log(value);
					}
				)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					onComplete?.Invoke();
				});
		}

		public virtual void Hide()
		{
			m_background.style.opacity = 0f;
			m_background.style.display = DisplayStyle.None;
		}

		public virtual Tween FadeOut(float duration, Action onComplete = null)
		{
			if (m_background == null)
				return null;

			m_background.style.opacity = 1f;

			return DOVirtual
				.Float(
					1f,
					0f,
					duration,
					(float value) =>
					{
						m_background.style.opacity = value;
					}
				)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					m_background.style.opacity = 0f;
					m_background.style.display = DisplayStyle.None;
					onComplete?.Invoke();
				});
		}

		#endregion

	}
}