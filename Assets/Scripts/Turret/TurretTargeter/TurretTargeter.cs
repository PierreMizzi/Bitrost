using UnityEngine;


public class TurretTargeter : MonoBehaviour
{
    #region Fields

    private Animator m_animator;

    [SerializeField]
    private GameObject m_sprite;

    #endregion

    #region Methods

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        Hide();
    }

    public void Target(ATarget target)
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;

        transform.localScale = target.targeterScale;

        m_sprite.SetActive(true);
    }

    public void Hide()
    {
        m_sprite.SetActive(false);
    }

    #endregion
}
