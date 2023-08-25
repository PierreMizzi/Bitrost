using System;
using UnityEngine;

namespace Bitfrost.Gameplay
{
	[RequireComponent(typeof(HealthEntity))]
	public class WeakSpot : MonoBehaviour
	{
		#region Fields 




		private HealthEntity m_healthEntity;

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
		}

		public void Initialize(float maxHealth)
		{
			m_healthEntity.Initialize(maxHealth);
		}

		public void Reset()
		{
			m_healthEntity.Reset();
			SetAlive();
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