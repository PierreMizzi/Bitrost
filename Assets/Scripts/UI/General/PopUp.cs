using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

namespace PierreMizzi.Useful.UI
{

	public class PopUp : MonoBehaviour
	{

		#region Fields 

		[SerializeField]
		private UIDocument m_document = null;

		[SerializeField]
		protected string m_containerName;

		protected VisualElement m_container;

		[SerializeField]
		private List<string> m_visualElementNames = new List<string>();

		protected List<VisualElement> m_visualElements = new List<VisualElement>();

		[Header("Animation")]
		[SerializeField]
		protected float m_fadeInDuration = 5;

		[SerializeField]
		protected float m_duration = 2;

		[SerializeField]
		protected float m_fadeOutDuration = 5;

		protected IEnumerator m_displayCoroutine;

		#endregion

		#region Methods 

		#region MonoBehaviour

		protected virtual void Awake()
		{
			Initialize();
		}

		#endregion

		protected virtual void Initialize()
		{
			m_container = m_document.rootVisualElement.Q(m_containerName);

			foreach (string name in m_visualElementNames)
			{
				if (!string.IsNullOrEmpty(name))
				{
					VisualElement element = m_container.Q(name);

					if (!m_visualElements.Contains(element))
						m_visualElements.Add(element);
				}
			}
			AddSpecificClassToElements(UIToolkitUtils.fadeIn);
		}

		#region Lifecycle

		[ContextMenu("DisplayThenHide")]
		public virtual void DisplayThenHide()
		{
			StartCoroutine(DisplayThenHideCoroutine());
		}

		public virtual IEnumerator DisplayThenHideCoroutine()
		{
			// fade-in animate
			AddClassToElements(UIToolkitUtils.animate);
			yield return new WaitForSeconds(m_fadeInDuration);

			// set into fade-out state
			RemoveClassToElements(UIToolkitUtils.animate);
			RemoveSpecificClassToElements(UIToolkitUtils.fadeIn);
			AddSpecificClassToElements(UIToolkitUtils.fadeOut);

			// Display duraation
			yield return new WaitForSeconds(m_duration);

			// Fade Out
			AddClassToElements(UIToolkitUtils.animate);
			yield return new WaitForSeconds(m_fadeOutDuration);

			// Revert back classes, back to fade-in state
			AddClassToElements(UIToolkitUtils.reset);
			RemoveClassToElements(UIToolkitUtils.animate);
			RemoveSpecificClassToElements(UIToolkitUtils.fadeOut);

			// There is an unwanted transition back to fade-in state, but reset prevents unwanted visuals
			AddSpecificClassToElements(UIToolkitUtils.fadeIn);
			yield return new WaitForSeconds(m_fadeInDuration);

			// Remove reset class
			RemoveClassToElements(UIToolkitUtils.reset);
		}

		#endregion

		public virtual void AddSpecificClassToElements(string className)
		{
			string fadeInClassName;
			foreach (VisualElement element in m_visualElements)
			{
				fadeInClassName = string.Format("{0}-{1}", element.name, className);
				element.AddToClassList(fadeInClassName);
			}
		}

		public virtual void RemoveSpecificClassToElements(string className)
		{
			string fadeInClassName;
			foreach (VisualElement element in m_visualElements)
			{
				fadeInClassName = string.Format("{0}-{1}", element.name, className);
				element.RemoveFromClassList(fadeInClassName);
			}
		}

		public virtual void AddClassToElements(string className)
		{
			foreach (VisualElement element in m_visualElements)
				element.AddToClassList(className);
		}

		public virtual void RemoveClassToElements(string className)
		{
			foreach (VisualElement element in m_visualElements)
				element.RemoveFromClassList(className);
		}

		public virtual void LogClasses()
		{
			string log;
			foreach (VisualElement element in m_visualElements)
			{
				log = "#" + element.name + "\r\n";
				List<string> cssClasses = element.GetClasses() as List<string>;
				foreach (string cssClass in cssClasses)
					log += cssClass + "\r\n";

				Debug.Log(log);
			}
		}

		[ContextMenu("TestFadeIn")]
		public virtual void TestFadeIn()
		{
			AddClassToElements(UIToolkitUtils.animate);
			LogClasses();
		}

		#endregion

	}
}