using System;
using System.Collections;
using UnityEngine;

namespace PierreMizzi.Useful.SceneManagement
{

	public delegate IEnumerator SceneUnloadingDelegate();


	/// <summary>
	/// Handles events to go from one scene to another accross the application
	/// </summary>
	[CreateAssetMenu(fileName = "BaseAppChannel", menuName = "PierreMizzi/Channels/Base Application Channel", order = 0)]
	public class BaseAppChannel : ScriptableObject
	{
		/// <summary>
		/// Event to go from Titlecard to Game Scene
		/// </summary>
		public Action onTitlecardToGame;

		/// <summary>
		/// Event to go from Game to Titlecard Scene
		/// </summary>
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