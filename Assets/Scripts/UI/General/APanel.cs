using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PierreMizzi.Useful.UI
{

	public abstract class APanel : MonoBehaviour
	{
		#region Fields 

		[Header("Panel")]
		[SerializeField]
		protected UIDocument m_document;

		protected VisualElement m_root;

		[SerializeField]
		protected string m_rootName = null;

		#endregion

		#region Methods 

		#region MonoBehaviour

		protected virtual void Awake()
		{
			m_root = m_document.rootVisualElement.Q(m_rootName);
			Hide();
		}

		#endregion

		protected virtual void Display()
		{
			m_root.style.display = DisplayStyle.Flex;
		}

		protected virtual void Hide()
		{
			m_root.style.display = DisplayStyle.None;
		}

		#endregion
	}
}
