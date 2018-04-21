using MightyPirates;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Movement m_Movement;

    [SerializeField]
    private Weapon m_Weapon;

    [Header("Input")]
    [SerializeField]
    private string m_HorizontalInputAxis = "Horizontal";

    [SerializeField]
    private string m_VerticalInputAxis = "Vertical";

    [SerializeField]
    private string m_FireInputButton = "Fire1";

    private void Awake()
    {
        if (m_Camera == null)
            m_Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleShooting();
    }

    private void HandleMovement()
    {
        m_Movement.SetAcceleration(new Vector2(Input.GetAxis(m_HorizontalInputAxis), Input.GetAxis(m_VerticalInputAxis)));
    }

    private void HandleRotation()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = m_Camera.WorldToScreenPoint(transform.position);
        Vector2 lookVector = mousePosition - playerPosition;
        m_Movement.SetLookVector(lookVector);
    }

    private void HandleShooting()
    {
        if (!Input.GetButton(m_FireInputButton)) return;
        m_Weapon.TryShoot();
    }
}