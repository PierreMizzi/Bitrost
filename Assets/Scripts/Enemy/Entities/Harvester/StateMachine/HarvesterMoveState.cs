using System.Collections;
using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace Bitfrost.Gameplay.Enemies
{

    public class HarvesterMoveState : EnemyMoveState
    {
        public HarvesterMoveState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            m_harvester = m_stateMachine.gameObject.GetComponent<Harvester>();
        }

        private Harvester m_harvester;

        private Sequence m_approachSpot;

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            m_harvester.SearchTargetSpot();
            m_harvester.targetSpot.isAvailable = false;

            // Using a Sequence
            // Vector3 direction = m_harvester.targetSpot.transform.position - m_harvester.transform.position;
            // float distance = direction.magnitude;
            // float durationMove = distance / m_harvester.settings.speed;

            // m_harvester.transform.up = direction.normalized;

            // m_approachSpot = DOTween.Sequence();
            // m_approachSpot.Append(MoveTween(durationMove));
            // m_approachSpot.AppendCallback(() =>
            // {
            //     // TODO COMPRENDRE CETTE MERDE
            //     Debug.Log(m_harvester.transform.position);
            // });
            // m_approachSpot.Append(RotateTween());
            // m_approachSpot.AppendCallback(OnCompleteMovement);

            // Using Coroutines
            StartApproachPlayer();

        }

        #region Tweens & Sequence

        private Tween MoveTween(float duration)
        {
            Vector3 fromPosition = m_harvester.transform.position;
            return DOVirtual.Float(0, 1, duration, (float value) =>
            {
                m_harvester.transform.position = Vector3.Lerp(fromPosition, m_harvester.targetSpot.transform.position, value);
            });
        }

        private Tween RotateTween()
        {
            Vector3 fromDirection = m_harvester.transform.up;
            Vector3 toDirection = m_harvester.targetSpot.transform.up;

            Vector3 fromPosition = m_harvester.transform.position;

            return DOVirtual.Float(0, 1, 1f, (float value) =>
            {
                m_harvester.transform.up = Vector3.Lerp(fromDirection, toDirection, value);
                m_harvester.transform.position = Vector3.Lerp(fromPosition, m_harvester.targetSpot.transform.position, value);
            });
        }

        #endregion

        #region Coroutine

        private IEnumerator m_approachCoroutine;

        private void StartApproachPlayer()
        {
            if (m_approachCoroutine == null)
            {
                m_approachCoroutine = ApproachAsteroid();
                m_harvester.StartCoroutine(m_approachCoroutine);
            }
        }

        private void StopApproachPlayer()
        {
            if (m_approachCoroutine != null)
            {
                m_harvester.StopCoroutine(m_approachCoroutine);
                m_approachCoroutine = null;
            }
        }

        private void PauseApproachCoroutine()
        {
            if (m_approachCoroutine != null)
                m_harvester.StopCoroutine(m_approachCoroutine);
        }

        private void ResumeApproachCoroutine()
        {
            if (m_approachCoroutine != null)
                m_harvester.StartCoroutine(m_approachCoroutine);
        }

        private IEnumerator ApproachAsteroid()
        {
            yield return ReachTargetSpot();
            yield return AlignToTargetSpot();

            ChangeState((int)EnemyStateType.Attack);
        }

        private IEnumerator ReachTargetSpot()
        {
            Transform targetTransform = m_harvester.targetSpot.transform;

            Vector3 fromPosition = m_harvester.transform.position;
            Vector3 harvesterToSpot = fromPosition - targetTransform.position;
            float distance = harvesterToSpot.magnitude;

            float totalTime = distance / m_harvester.settings.speed;
            float currentTime = 0;
            float progress = 0;

            while (progress < 1f)
            {
                currentTime += Time.deltaTime;
                progress = currentTime / totalTime;

                m_harvester.transform.position = Vector3.Lerp(fromPosition, targetTransform.position, progress);
                m_harvester.transform.up = (targetTransform.position - m_harvester.transform.position).normalized;

                yield return null;
            }

            m_harvester.transform.position = targetTransform.position;

            yield return null;
        }

        private IEnumerator AlignToTargetSpot()
        {
            Vector3 fromPosition = m_harvester.transform.position;

            Vector3 fromDirection = m_harvester.transform.up;

            float totalTime = 1f;
            float currentTime = 0;
            float progress = 0;

            while (progress < 1f)
            {
                currentTime += Time.deltaTime;
                progress = currentTime / totalTime;

                m_harvester.transform.position = Vector3.Lerp(fromPosition, m_harvester.targetSpot.transform.position, progress);
                m_harvester.transform.up = Vector3.Lerp(fromDirection, m_harvester.targetSpot.transform.up, progress);

                yield return null;
            }

            m_harvester.transform.position = m_harvester.targetSpot.transform.position;
            m_harvester.transform.up = m_harvester.targetSpot.transform.up;

            yield return null;
        }

        #endregion

        public override void Update()
        {
            if (!m_harvester.isTargetValid)
                ChangeState((int)EnemyStateType.Idle);
        }

        public override void Exit()
        {
            StopApproachPlayer();

            if (m_approachSpot != null && m_approachSpot.IsPlaying())
                m_approachSpot.Kill();
        }

        public override void Pause()
        {
            base.Pause();

            PauseApproachCoroutine();

            if (m_approachSpot != null && m_approachSpot.IsPlaying())
                m_approachSpot.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            ResumeApproachCoroutine();

            if (m_approachSpot != null && !m_approachSpot.IsPlaying())
                m_approachSpot.Play();
        }

        public void OnCompleteMovement()
        {
            ChangeState((int)EnemyStateType.Attack);
        }

    }
}