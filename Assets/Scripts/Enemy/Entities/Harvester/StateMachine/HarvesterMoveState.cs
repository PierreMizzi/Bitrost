using System;
using System.Collections;
using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using Unity.Mathematics;
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

        private IEnumerator m_approachCoroutine;


        protected override void DefaultEnter()
        {
            base.DefaultEnter();
            m_harvester.SearchTargetSpot();
            m_harvester.targetSpot.isAvailable = false;

            // Using Coroutines
            StartApproachPlayer();

        }

        #region Coroutine


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
            yield return ReachTargetSpotReworked();
            ChangeState((int)EnemyStateType.Attack);
        }

        private IEnumerator ReachTargetSpotReworked()
        {
            Transform targetTransform = m_harvester.targetSpot.transform;

            Vector3 fromDirection;
            Vector3 fromPosition = m_harvester.transform.position;
            Vector3 harvesterToSpot = fromPosition - targetTransform.position;
            float distance = harvesterToSpot.magnitude;

            float totalTime = distance / m_harvester.settings.speed;
            float currentTime = 0;
            float progress = 0;
            float directionProgress = 0;

            while (progress < 1f)
            {
                currentTime += Time.deltaTime;
                progress = currentTime / totalTime;

                // Position
                m_harvester.transform.position = Vector3.Lerp(fromPosition, targetTransform.position, progress);

                // Direction
                directionProgress = ProgressToDirectionProgress(progress);
                fromDirection = (targetTransform.position - m_harvester.transform.position).normalized;
                m_harvester.transform.up = Vector3.Lerp(fromDirection, targetTransform.up, directionProgress);

                yield return null;
            }

            m_harvester.transform.position = targetTransform.position;
            m_harvester.transform.up = targetTransform.up;

            yield return null;
        }

        private float ProgressToDirectionProgress(float progress)
        {
            progress = math.remap(m_harvester.settings.rotationProgressBeginning,
                                  1f,
                                  0f,
                                  1f,
                                  progress);

            progress = Mathf.Clamp01(progress);
            return progress;
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


        }

        public override void Pause()
        {
            base.Pause();

            PauseApproachCoroutine();

        }

        public override void Resume()
        {
            base.Resume();

            ResumeApproachCoroutine();


        }

        public void OnCompleteMovement()
        {
            ChangeState((int)EnemyStateType.Attack);
        }

    }
}