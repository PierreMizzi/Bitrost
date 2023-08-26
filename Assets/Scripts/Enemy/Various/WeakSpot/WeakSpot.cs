using System;
using UnityEngine;

namespace Bitfrost.Gameplay
{
	[RequireComponent(typeof(HealthEntity))]
	public class WeakSpot : MonoBehaviour
	{
		#region Fields 

		private HealthEntity m_healthEntity;

		private Collider2D m_collider;

		public Action onDestroyed;

		public bool isDestroyed { get; private set; }

		[SerializeField]
		private SpriteRenderer m_spriteRenderer;

		[SerializeField]
		private Color m_aliveColor = Color.yellow;
		[SerializeField]
		private Color m_deadColor = Color.gray;

		#endregion

		#region Methods 

		#region MonoBehaviour

		protected virtual void Awake()
		{
			m_healthEntity = GetComponent<HealthEntity>();
			m_collider = GetComponent<Collider2D>();
		}

		protected virtual void Start()
		{
			m_healthEntity.onNoHealth += CallbackDestroyed;
			SetAlive();
		}

		protected virtual void OnDestroy()
		{
			m_healthEntity.onNoHealth -= CallbackDestroyed;
		}

		#endregion

		private void CallbackDestroyed()
		{
			isDestroyed = true;
			onDestroyed.Invoke();
			SetDead();
			SetNonHittable();
		}

		public void Initialize(float maxHealth)
		{
			m_healthEntity.Initialize(maxHealth);
		}

		public void Reset()
		{
			isDestroyed = false;
			m_healthEntity.Reset();
			SetAlive();
		}

		public void SetHittable()
		{
			m_collider.enabled = true;
		}

		public void SetNonHittable()
		{
			m_collider.enabled = false;
		}

		private void SetAlive()
		{
			m_spriteRenderer.color = m_aliveColor;
		}

		private void SetDead()
		{
			m_spriteRenderer.color = m_deadColor;
		}

		#endregion

	}
}