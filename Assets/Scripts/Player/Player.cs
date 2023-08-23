using System;
using System.Collections.Generic;
using PierreMizzi.SoundManager;
using UnityEngine;

namespace Bitfrost.Gameplay.Players
{

    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(HealthEntity))]
    public class Player : MonoBehaviour, IPausable
    {
        #region Fields

        [SerializeField]
        private LevelChannel m_levelChannel;

        private Animator m_animator;

        private const string k_isDead = "IsDead";

        private HealthEntity m_healthEntity;

        public HealthEntity healthEntity { get { return m_healthEntity; } }

        public bool isPaused { get; set; }

        [SerializeField]
        private PlayerSettings m_settings;

        private PlayerController m_controller;

        private List<string> m_hitSounds = new List<string>();

        #endregion

        #region Methods

        private void Awake()
        {
            m_levelChannel.player = this;

            m_controller = GetComponent<PlayerController>();
            m_healthEntity = GetComponent<HealthEntity>();

            m_hitSounds = new List<string>(){
                SoundDataID.PLAYER_HIT_01,
                SoundDataID.PLAYER_HIT_02,
            };

            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_healthEntity.Initialize(m_settings.maxHealth);
            SubscribeHealthEntity();

            if (m_levelChannel != null)
            {
                m_levelChannel.onReset += CallbackReset;
                m_levelChannel.onPauseGame += Pause;
                m_levelChannel.onResumeGame += Resume;

                m_levelChannel.onTurretRetrieved += CallbackTurretRetrieved;
            }
        }

        private void OnDestroy()
        {
            UnsubscribeHealthEntity();

            if (m_levelChannel != null)
            {
                m_levelChannel.onReset -= CallbackReset;
                m_levelChannel.onPauseGame -= Pause;
                m_levelChannel.onResumeGame -= Resume;

                m_levelChannel.onTurretRetrieved -= CallbackTurretRetrieved;
            }
        }

        #region Health

        private void SubscribeHealthEntity()
        {
            m_healthEntity.onLostHealth += CallbackLostHealth;
            m_healthEntity.onHealedHealth += CallbackHealedHealth;
            m_healthEntity.onNoHealth += CallbackNoHealth;
        }

        private void UnsubscribeHealthEntity()
        {
            m_healthEntity.onLostHealth -= CallbackLostHealth;
            m_healthEntity.onHealedHealth -= CallbackHealedHealth;
            m_healthEntity.onNoHealth -= CallbackNoHealth;
        }

        private void CallbackHealedHealth()
        {
            SoundManager.PlaySFX(SoundDataID.PLAYER_HEALED);
        }

        private void CallbackLostHealth()
        {
            SoundManager.PlayRandomSFX(m_hitSounds);
        }

        [ContextMenu("CallbackNoHealth")]
        private void CallbackNoHealth()
        {
            SetDead();
            m_controller.enabled = false;

            SoundManager.PlaySFX(SoundDataID.PLAYER_DEATH);
        }

        #endregion

        #region Death

        public void SetDead()
        {
            m_animator.SetBool(k_isDead, true);
        }

        public void SetAlive()
        {
            m_animator.SetBool(k_isDead, false);
        }

        // Function called at the end of Dead.anim
        public void CallbackDeadAnimation()
        {
            m_levelChannel.onPlayerDead.Invoke();
        }

        #endregion

        #region Reset - Restart

        public void CallbackReset()
        {
            SetAlive();
            m_healthEntity.Reset();
            transform.position = Vector3.zero;
            m_controller.enabled = true;
        }

        public void Pause()
        {
            m_controller.Pause();
            m_animator.speed = 0;
        }

        public void Resume()
        {
            m_controller.Resume();
            m_animator.speed = 1;
        }

        #endregion

        private void CallbackTurretRetrieved(int storedEnergy)
        {
            if (!m_healthEntity.isMaxHealth && storedEnergy > 0)
                m_healthEntity.HealHealth(storedEnergy * m_settings.healedHealthPerStoredEnergy);
        }

        #endregion
    }
}
