#if UNITY_EDITOR

using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using PierreMizzi.Useful.SceneManagement;

namespace PierreMizzi.Useful.SceneManagementEditor
{

	public static class SceneSetupShortcuts
	{

		private const string scenesPath = "Assets/Scenes/Application/";
		private const string scenePrefix = ".unity";

		[MenuItem("Scene Setup Shortcut/Titlecard")]
		public static void SetupTitlecard()
		{
			EditorSceneManager.OpenScene(PathFromName(BaseAppManager.titlecardSceneName));
			EditorSceneManager.OpenScene(PathFromName(BaseAppManager.applicationSceneName), OpenSceneMode.Additive);

			Scene activeScene = EditorSceneManager.GetSceneByName(BaseAppManager.titlecardSceneName);
			EditorSceneManager.SetActiveScene(activeScene);
		}

		[MenuItem("Scene Setup Shortcut/Game")]
		public static void SetupGame()
		{
			EditorSceneManager.OpenScene(PathFromName(BaseAppManager.gameSceneName));
			EditorSceneManager.OpenScene(PathFromName(BaseAppManager.applicationSceneName), OpenSceneMode.Additive);

			Scene activeScene = EditorSceneManager.GetSceneByName(BaseAppManager.gameSceneName);
			EditorSceneManager.SetActiveScene(activeScene);
		}

		public static void OpenAdditionnalScene(string path, params string[] sceneNames)
		{

			string fullPath;
			int length = sceneNames.Length;
			for (int i = 0; i < length; i++)
			{
				fullPath = path + sceneNames[i] + scenePrefix;
				EditorSceneManager.OpenScene(fullPath, OpenSceneMode.Additive);
			}
		}

		public static string PathFromName(string name)
		{
			return scenesPath + name + scenePrefix;
		}

	}

}

#endif