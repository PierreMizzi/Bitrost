using System;
using UnityEngine;

namespace Bitfrost.Gameplay
{
	[RequireComponent(typeof(HealthEntity))]
	public class WeakSpot : MonoBehaviour
	{

		private HealthEntity m_healthEntity;

		public Action onDestroyed;

		public bool isDestroyed { get; private set; }

		protected virtual void Awake()
		{
			m_healthEntity = GetComponent<HealthEntity>();
		}

		protected virtual void Start()
		{
			m_healthEntity.onNoHealth += CallbackDestroyed;
		}

		protected virtual void OnDestroy()
		{
			m_healthEntity.onNoHealth -= CallbackDestroyed;
		}

		private void CallbackDestroyed()
		{
			isDestroyed = true;
			// TODO : Change visual state
			onDestroyed.Invoke();
		}

		public void Initialize(float maxHealth)
		{
			m_healthEntity.Initialize(maxHealth);
		}

		public void Reset()
		{
			m_healthEntity.Reset();
		}
	}
}