using MightyPirates;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class Player : MonoBehaviour
{
    [SerializeField]
    private Movement m_Movement;

    [SerializeField]
    private Weapon m_Weapon;
    
    [SerializeField]
    private float m_DodgeStrength = 5;

    [Header("Input")]
    [SerializeField]
    private string m_HorizontalInputAxis = "Horizontal";

    [SerializeField]
    private string m_VerticalInputAxis = "Vertical";

    [SerializeField]
    private string m_DodgeInputButton = "Dodge";

    [SerializeField]
    private string m_FireInputButton = "Fire1";

    private Camera m_Camera;

    private void Update()
    {
        HandleMovement();
        HandleDodging(); // D:
        HandleRotation();
        HandleShooting();
        HandleCommands();
    }

    private void HandleMovement()
    {
        m_Movement.AddAcceleration(new Vector2(Input.GetAxis(m_HorizontalInputAxis), Input.GetAxis(m_VerticalInputAxis)));
    }

    private void HandleDodging()
    {
        if (!Input.GetButtonDown(m_DodgeInputButton)) return;
        m_Movement.AddImpulse(new Vector2(Input.GetAxis(m_HorizontalInputAxis), Input.GetAxis(m_VerticalInputAxis)).normalized * m_DodgeStrength);
    }

    private void HandleRotation()
    {
        if (m_Camera == null)
            m_Camera = Camera.main;
        m_Movement.AddLookAt(m_Camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private void HandleShooting()
    {
        if (!Input.GetButton(m_FireInputButton))
            return;
        m_Weapon.TryShoot();
    }

    private void HandleCommands()
    {
        if (!Input.GetMouseButtonDown(1))
            return;
        if (m_Camera == null)
            m_Camera = Camera.main;

        Vector3 worldPosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D target = Physics2D.OverlapPoint(worldPosition, Layers.PickingMask);
        if (target == null)
            return;

        Entity entity = target.GetComponentInParent<Entity>();
        if (entity == null)
            return;

        if (entity.GetComponent<Health>() == null)
            return;

        foreach (Minion minion in Minion.All)
        {
            TargetTracker targetTracker = minion.GetComponent<TargetTracker>();
            if (targetTracker == null)
                continue;

            targetTracker.Target = entity.gameObject;
        }
    }
}