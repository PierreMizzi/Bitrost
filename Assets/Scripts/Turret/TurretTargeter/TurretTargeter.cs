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
                UpdatInfos();
            }
        }

        private void UpdateRotation()
        {
            m_sprite.transform.rotation = currentTarget.GetTargeterRotation();
        }

        public void SetTarget(ATarget target)
        {
            currentTarget = target;

            // Visual
            m_sprite.color = currentTarget.targeterColor;
            m_infoText.color = currentTarget.targeterColor;

            // Transform
            transform.position = currentTarget.transform.position;
            m_sprite.transform.localScale = currentTarget.targeterScale;

            m_sprite.gameObject.SetActive(true);
            m_infoContainer.gameObject.SetActive(true);
        }

        public void UnsetTarget()
        {
            currentTarget = null;

            m_sprite.gameObject.SetActive(false);
            m_infoContainer.gameObject.SetActive(false);
        }

        #region Info Position

        [Header("Info Position")]
        [SerializeField]
        private Transform m_infoContainer;

        [SerializeField]
        private Vector3 m_infoContainerOffset;
        private Vector3 m_infoContainerPosition;

        private float rotationAngle;

        // [SerializeField]
        // private float m_spriteRotation;

        // public void OnValidate()
        // {
        //     if (!enabled)
        //         return;
        //     m_sprite.transform.rotation = Quaternion.Euler(0f, 0f, m_spriteRotation);
        //     UpdatInfosPosition();
        // }

        private void UpdatInfos()
        {
            // m_sprite.transform.rotation = Quaternion.Euler(0f, 0f, m_spriteRotation);

            // Transform
            rotationAngle = (UtilsClass.ToFullAngle(m_sprite.transform.rotation.eulerAngles.z) % 90) - 45;
            m_infoContainerPosition = Quaternion.AngleAxis(rotationAngle, Vector3.forward) * m_infoContainerOffset;
            m_infoContainer.localPosition = m_infoContainerPosition * m_sprite.transform.localScale.x;

            // Infos
            m_infoText.text = currentTarget.GetInfos();
        }

        #endregion

        #endregion
    }
}
