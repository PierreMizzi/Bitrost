using System;
using UnityEngine;

namespace PierreMizzi.Useful.SceneManagement
{

	[CreateAssetMenu(fileName = "BaseAppChannel", menuName = "PierreMizzi/UI/BaseApplicationChannel", order = 0)]
	public class BaseAppChannel : ScriptableObject
	{

		public Action onTitlecardToGame;
		public Action onGameToTitlecard;

		protected virtual void OnEnable()
		{
			onTitlecardToGame = () => { };
			onGameToTitlecard = () => { };
		}

	}
}