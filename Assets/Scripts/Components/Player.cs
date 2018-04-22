using MightyPirates;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class Player : MonoBehaviour
{
    [SerializeField]
    private Movement m_Movement;

    [SerializeField]
    private WeaponSlot[] m_WeaponSlots;

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

    [SerializeField]
    private string m_Equip0InputButton = "Equip0";

    [SerializeField]
    private string m_Equip1InputButton = "Equip1";

    [SerializeField]
    private string m_Equip2InputButton = "Equip2";

    [SerializeField]
    private string m_Equip3InputButton = "Equip3";

    private Camera m_Camera;
    private readonly Collider2D[] m_PickupScanResults = new Collider2D[4];
    private Pickupable m_Pickupable;

    public Pickupable Pickupable => m_Pickupable;

    private void Update()
    {
        if (m_Camera == null)
            m_Camera = Camera.main;

        HandleMovement();
        HandleDodging();
        HandleRotation();
        HandleShooting();
        HandleCommands();
        HandlePickup();
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
        m_Movement.AddLookAt(m_Camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private void HandleShooting()
    {
        if (!Input.GetButton(m_FireInputButton))
            return;
        foreach (WeaponSlot weaponSlot in m_WeaponSlots)
            weaponSlot.TryShoot();
    }

    private void HandleCommands()
    {
        HandleAttackCommand();
        HandleMoveCommand();
    }

    private void HandleAttackCommand()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

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

    private void HandleMoveCommand()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 worldPosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            foreach (Minion minion in Minion.All)
            {
                MoveToPosition moveTo = minion.GetComponent<MoveToPosition>();
                if (moveTo != null)
                    moveTo.SetTarget(worldPosition);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            foreach (Minion minion in Minion.All)
            {
                MoveToPosition moveTo = minion.GetComponent<MoveToPosition>();
                if (moveTo == null)
                    continue;

                moveTo.ClearTarget();
            }
        }
    }

    private void HandlePickup()
    {
        m_Pickupable = null;

        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, 4, m_PickupScanResults, Layers.PickupMask);
        if (hitCount == 0)
            return;

        Collider2D bestColl = m_PickupScanResults[0];
        float bestDist = Vector2.Distance(transform.position, bestColl.transform.position);
        for (int i = 1; i < hitCount; i++)
        {
            Collider2D coll = m_PickupScanResults[i];
            float dist = Vector2.Distance(transform.position, coll.transform.position);
            if (dist < bestDist)
            {
                bestColl = coll;
                bestDist = dist;
            }
        }

        Pickup pickup = bestColl.GetComponent<Pickup>();
        if (pickup == null)
            return;

        m_Pickupable = pickup.Value as Pickupable;

        Weapon weapon = pickup.Value as Weapon;
        if (weapon != null)
        {
            int equipSlot = -1;
            if (Input.GetButtonDown(m_Equip0InputButton))
            {
                equipSlot = 0;
            }
            else if (Input.GetButtonDown(m_Equip1InputButton))
            {
                equipSlot = 1;
            }
            else if (Input.GetButtonDown(m_Equip2InputButton))
            {
                equipSlot = 2;
            }
            else if (Input.GetButtonDown(m_Equip3InputButton))
            {
                equipSlot = 3;
            }
            if (equipSlot < 0)
                return;

            Weapon oldWeapon = m_WeaponSlots[equipSlot].Weapon;
            m_WeaponSlots[equipSlot].Weapon = weapon;
            pickup.Value = oldWeapon;

            if (pickup.Value == null)
                pickup.gameObject.Free();
        }
        Powerup powerup = pickup.Value as Powerup;
        if (powerup != null)
        {
            powerup.Activate(this);
            pickup.gameObject.Free();
        }
    }
}