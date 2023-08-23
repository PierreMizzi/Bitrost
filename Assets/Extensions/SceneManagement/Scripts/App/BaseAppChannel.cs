using System;
using System.Collections;
using UnityEngine;

namespace PierreMizzi.Useful.SceneManagement
{

	public delegate IEnumerator SceneUnloadingDelegate();


	[CreateAssetMenu(fileName = "BaseAppChannel", menuName = "PierreMizzi/UI/BaseApplicationChannel", order = 0)]
	public class BaseAppChannel : ScriptableObject
	{

		public Action onTitlecardToGame;
		public Action onGameToTitlecard;

		public SceneUnloadingDelegate onUnloadTitlecardScene;
		public SceneUnloadingDelegate onUnloadGameScene;

		protected virtual void OnEnable()
		{
			onTitlecardToGame = () => { };
			onGameToTitlecard = () => { };

			onUnloadTitlecardScene = () => { return null; };
			onUnloadGameScene = () => { return null; };
		}

	}
}