using System;
using PierreMizzi.Useful;
using TMPro;
using UnityEngine;

namespace Bitfrost.Gameplay.Turrets
{

    [ExecuteInEditMode]
    public class TurretTargeter : MonoBehaviour
    {
        #region Fields

        private ATarget currentTarget;

        private Animator m_animator;

        [SerializeField]
        private SpriteRenderer m_sprite;

        [SerializeField]
        private TextMeshProUGUI m_infoText;

        #endregion

        #region Methods

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            UnsetTarget();
        }



        private void Update()
        {
            if (currentTarget != null)
            {
                UpdateRotation();
                UpdatInfosPosition();
            }
        }

        private void UpdateRotation()
        {
            transform.rotation = currentTarget.GetTargeterRotation();
        }

        public void SetTarget(ATarget target)
        {
            currentTarget = target;

            // Visual
            m_sprite.color = currentTarget.targeterColor;
            m_infoText.text = currentTarget.GetInfos();

            // Transform
            transform.position = currentTarget.transform.position;
            transform.localScale = currentTarget.targeterScale;

            m_sprite.gameObject.SetActive(true);
        }

        public void UnsetTarget()
        {
            currentTarget = null;

            m_sprite.gameObject.SetActive(false);
        }

        #region Info Position

        [SerializeField]
        private float m_spriteRotation;

        private Transform m_infoContainer;

        [SerializeField]
        private Vector3 m_infoContainerStartingPosition;
        private Vector3 m_infoContainerPosition;

        private float rotationAngle;
        public void OnValidate()
        {
            if (!enabled)
                return;
            m_sprite.transform.rotation = Quaternion.Euler(0f, 0f, m_spriteRotation);
            UpdatInfosPosition();
        }

        private void UpdatInfosPosition()
        {
            m_sprite.transform.rotation = Quaternion.Euler(0f, 0f, m_spriteRotation);

            rotationAngle = (UtilsClass.ToFullAngle(m_spriteRotation) % 90) - 45;
            m_infoContainerPosition = Quaternion.AngleAxis(rotationAngle, Vector3.forward) * m_infoContainerStartingPosition;
            m_infoContainer.localPosition = m_infoContainerPosition * m_sprite.transform.localScale.x;

        }

        #endregion

        #endregion
    }
}
