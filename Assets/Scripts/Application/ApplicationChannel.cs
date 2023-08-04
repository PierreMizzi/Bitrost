using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "Bitrost/Architecture/ApplicationChannel", order = 0)]
public class ApplicationChannel : ScriptableObject
{

	public CursorDelegate onSetCursor;

	private void OnEnable()
	{
		onSetCursor = (CursorType type) => { };
	}

}