using PierreMizzi.Pause;
using PierreMizzi.Useful;
using UnityEngine;

namespace Bitfrost.Gameplay.Bullets
{

	public class BulletImpact : MonoBehaviour, IPausable
	{
		#region Fields 

		[SerializeField] private BulletChannel m_bulletChannel;

		[SerializeField] private Animator m_animator;

		[SerializeField] private SpriteRenderer m_spriteRenderer;

		public bool isPaused { get; set; }

		#endregion

		#region Methods 

		public virtual void Hide()
		{
			m_spriteRenderer.color = UtilsClass.Transparent;
		}

		public virtual void AnimationEnded()
		{
			m_bulletChannel.onReleaseImpact.Invoke(this);
		}

		#region Pause

		public void Pause()
		{
			isPaused = true;
			m_animator.speed = 0;
		}

		public void Resume()
		{
			isPaused = false;
			m_animator.speed = 1;
		}

		#endregion

		#endregion
	}

}