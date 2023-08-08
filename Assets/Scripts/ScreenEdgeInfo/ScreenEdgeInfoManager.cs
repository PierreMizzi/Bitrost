using System.Collections.Generic;
using UnityEngine;

/*



*/

public class ScreenEdgeInfoManager : MonoBehaviour
{
	#region Fields 

	public new Camera camera { get; private set; }

	private Vector3 m_cameraCenterWorld;

	public Vector3 screenCenter
	{
		get; private set;
	}

	[SerializeField]
	private List<AScreenEdgeInfo> m_infos = new List<AScreenEdgeInfo>();

	[SerializeField]
	private AInfoSubject m_infoSubject = null;

	[SerializeField]
	private Transform m_infosContainer;

	#endregion

	#region Methods 

	#region MonoBehaviour

	private void Awake()
	{
		camera = Camera.main;
		screenCenter = new Vector3(camera.pixelWidth / 2f, camera.pixelHeight / 2f, 0f);
		Debug_CreateScreenEdgeInfo();
	}

	#endregion

	#region Behaviour

	public void CreateInfo(ScreenEdgeInfoType type, AInfoSubject subject)
	{
		AScreenEdgeInfo infoPrefab = m_infos.Find((AScreenEdgeInfo info) => info.type == type);

		if (infoPrefab == null)
			return;

		AScreenEdgeInfo info = Instantiate(infoPrefab, m_infosContainer);
		info.Initialize(this, subject);
	}

	public void DestroyInfo()
	{

	}

	#endregion

	public float MagnitudeToEdge(float angle)
	{
		return camera.MagnitudeToEdge(angle);
	}


	[ContextMenu("Debug")]
	public void Debug_CreateScreenEdgeInfo()
	{
		CreateInfo(ScreenEdgeInfoType.Turret, m_infoSubject);
	}

	#endregion
}