using MightyPirates;
using UnityEngine;

[DisallowMultipleComponent]
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
        HandleCommands();
    }

    private void HandleMovement()
    {
        m_Movement.AddAcceleration(new Vector2(Input.GetAxis(m_HorizontalInputAxis), Input.GetAxis(m_VerticalInputAxis)));
    }

    private void HandleRotation()
    {
        m_Movement.AddLookAt(m_Camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private void HandleShooting()
    {
        if (!Input.GetButton(m_FireInputButton)) return;
        m_Weapon.TryShoot();
    }

    private void HandleCommands()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        Vector3 worldPosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D target = Physics2D.OverlapPoint(worldPosition, Layers.PickingMask);
        if (target == null)
            return;

        Health health = target.GetComponentInParent<Health>();
        if (health == null)
            return;

        foreach (Minion minion in Minion.All)
        {
            TargetTracker targetTracker = minion.GetComponent<TargetTracker>();
            if (targetTracker == null)
                continue;

            targetTracker.Target = health.gameObject;
        }
    }
}