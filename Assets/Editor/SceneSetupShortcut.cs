using UnityEngine.SceneManagement;
using UnityEditor;
using PierreMizzi.Useful.SceneManagement;
using UnityEditor.SceneManagement;

public static class SceneSetupShortcuts
{

	private const string scenesPath = "Assets/Scenes/Application/";
	private const string scenePrefix = ".unity";

	[MenuItem("Overcore/Scene Setup/Titlecard")]
	public static void SetupTitlecard()
	{
		EditorSceneManager.OpenScene(PathFromName(BaseAppManager.titlecardSceneName));
		EditorSceneManager.OpenScene(PathFromName(BaseAppManager.applicationSceneName), OpenSceneMode.Additive);

		Scene activeScene = EditorSceneManager.GetSceneByName(BaseAppManager.titlecardSceneName);
		EditorSceneManager.SetActiveScene(activeScene);
	}

	[MenuItem("Overcore/Scene Setup/Game")]
	public static void SetupGame()
	{
		EditorSceneManager.OpenScene(PathFromName(BaseAppManager.gameSceneName));
		EditorSceneManager.OpenScene(PathFromName(BaseAppManager.applicationSceneName), OpenSceneMode.Additive);

		Scene activeScene = EditorSceneManager.GetSceneByName(BaseAppManager.gameSceneName);
		EditorSceneManager.SetActiveScene(activeScene);
	}

	public static string PathFromName(string name)
	{
		return scenesPath + name + scenePrefix;
	}

}