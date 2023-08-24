using UnityEngine;

namespace Bitfrost.Application
{

    [RequireComponent(typeof(Animator))]
    public class TitlecardCameraController : MonoBehaviour
    {
        #region Fields 

        private Animator m_animator;

        private const string k_state = "State";

        #endregion

        #region Methods 

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void SetState(TitlecardStateType state)
        {
            m_animator.SetInteger(k_state, (int)state);
        }

        #endregion
    }

}