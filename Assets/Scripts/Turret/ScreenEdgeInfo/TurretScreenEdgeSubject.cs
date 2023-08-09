using Bitfrost.Gameplay.ScreenEdgeInfo;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{

	public class TurretScreenEdgeSubject : ScreenEdgeSubject
	{
		#region Fields 

		#region Miniature

		[SerializeField]
		private Camera m_miniatureCamera = null;

		private TurretScreenEdgeRenderer screenEdgeRenderer
		{
			get { return m_screenEdgeRenderer as TurretScreenEdgeRenderer; }
		}

		#endregion

		#endregion

		#region Methods 

		public override void Initialize(ScreenEdgeInfoManager manager)
		{
			base.Initialize(manager);

			RenderTexture renderTex = new RenderTexture(512, 512, 1);
			m_miniatureCamera.targetTexture = renderTex;

			screenEdgeRenderer.SetMiniature(renderTex);
		}

		#endregion
	}
}