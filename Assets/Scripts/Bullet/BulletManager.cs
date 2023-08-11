using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PierreMizzi.Useful.PoolingObjects;

namespace Bitfrost.Gameplay.Bullets
{



	public class BulletManager : MonoBehaviour, IPausable
	{
		[Header("Channels")]

		[SerializeField]
		private BulletChannel m_bulletChannel = null;

		[SerializeField]
		private PoolingChannel m_poolingChannel = null;

		[SerializeField]
		private LevelChannel m_levelChannel = null;

		public bool isPaused { get; set; }

		// Bullets
		private List<Bullet> m_activeBullets = new List<Bullet>();

		// BulletImapcts
		private List<BulletImpact> m_activeImpacts = new List<BulletImpact>();

		#region MonoBehaviour

		private IEnumerator Start()
		{
			if (m_bulletChannel != null)
			{
				m_bulletChannel.onFireBullet += CallbackFireBullet;
				m_bulletChannel.onReleaseBullet += CallbackReleaseBullet;

				m_bulletChannel.onDisplayImpact += CallbackDisplayImpact;
				m_bulletChannel.onReleaseImpact += CallbackReleaseImpact;
			}

			if (m_levelChannel != null)
			{
				m_levelChannel.onPauseGame += Pause;
				m_levelChannel.onResumeGame += Resume;
				m_levelChannel.onReset += CallbackReset;
			}

			yield return new WaitForEndOfFrame();

			InitializePools();
		}

		private void OnDestroy()
		{
			if (m_bulletChannel != null)
			{
				m_bulletChannel.onFireBullet -= CallbackFireBullet;
				m_bulletChannel.onReleaseBullet -= CallbackReleaseBullet;
			}

			if (m_levelChannel != null)
			{
				m_levelChannel.onPauseGame -= Pause;
				m_levelChannel.onResumeGame -= Resume;
				m_levelChannel.onReset -= CallbackReset;
			}
		}

		#endregion

		private void InitializePools()
		{
			foreach (PoolConfig config in m_bulletChannel.bulletPoolConfigs)
				m_poolingChannel.onCreatePool.Invoke(config);

			foreach (PoolConfig config in m_bulletChannel.impactPoolConfigs)
			{
				config.onGetFromPool = ImpactGetFromPool;
				m_poolingChannel.onCreatePool.Invoke(config);
			}
		}

		#region Bullets

		public void CallbackFireBullet(
			BulletConfig config,
			IBulletLauncher launcher,
			Vector3 position,
			Vector3 orientation
		)
		{
			Bullet bullet = m_poolingChannel.onGetFromPool
				.Invoke(config.prefab.gameObject)
				.GetComponent<Bullet>();

			bullet.OufOfPool(config, launcher, position, orientation);

			if (!m_activeBullets.Contains(bullet))
				m_activeBullets.Add(bullet);
		}

		public void CallbackReleaseBullet(Bullet bullet)
		{
			if (m_activeBullets.Contains(bullet))
				m_activeBullets.Remove(bullet);

			m_poolingChannel.onReleaseToPool.Invoke(bullet.gameObject);
		}

		#endregion

		#region Bullet Impacts

		public void ImpactGetFromPool(GameObject gameObject)
		{
			BulletImpact impact = gameObject.GetComponent<BulletImpact>();
			impact.Hide();

			impact.gameObject.SetActive(true);
		}

		public void CallbackDisplayImpact(
			BulletImpact prefab,
			Vector3 position
		)
		{
			BulletImpact impact = m_poolingChannel.onGetFromPool
				.Invoke(prefab.gameObject)
				.GetComponent<BulletImpact>();

			impact.transform.position = position;

			if (!m_activeImpacts.Contains(impact))
				m_activeImpacts.Add(impact);
		}

		public void CallbackReleaseImpact(BulletImpact impact)
		{
			if (m_activeImpacts.Contains(impact))
				m_activeImpacts.Remove(impact);
			m_poolingChannel.onReleaseToPool.Invoke(impact.gameObject);
		}


		#endregion


		#region Reset

		public void CallbackReset()
		{
			foreach (Bullet bullet in m_activeBullets)
				m_poolingChannel.onReleaseToPool.Invoke(bullet.gameObject);

			m_activeBullets.Clear();
		}

		#endregion

		#region Pause

		public void Pause()
		{
			isPaused = true;
			foreach (Bullet bullet in m_activeBullets)
				bullet.Pause();

			foreach (BulletImpact impact in m_activeImpacts)
				impact.Pause();
		}

		public void Resume()
		{
			isPaused = false;
			foreach (Bullet bullet in m_activeBullets)
				bullet.Resume();

			foreach (BulletImpact impact in m_activeImpacts)
				impact.Resume();
		}

		#endregion
	}
}
