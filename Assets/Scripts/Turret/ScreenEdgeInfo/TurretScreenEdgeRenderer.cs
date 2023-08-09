using Bitfrost.Gameplay.ScreenEdgeInfo;
using PierreMizzi.Useful;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{
	public class TurretScreenEdgeRenderer : ScreenEdgeRenderer
	{
		#region Fields 

		[SerializeField]
		private Material m_miniatureMaterial;

		[SerializeField]
		private Renderer m_miniatureRenderer = null;

		#endregion

		#region Methods 

		public void SetMiniature(RenderTexture texture)
		{
			Material miniatureMaterial = new Material(m_miniatureMaterial);
			miniatureMaterial.SetTexture("_MainTexMat", texture);

			m_miniatureRenderer.material = miniatureMaterial;
		}

		#endregion
	}
}