using MightyPirates;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Rigidbody2D m_Body;

    [SerializeField]
    private float m_AccelerationPower = 5f;

    [Header("Attack")]
    [SerializeField]
    private ObjectPool m_BulletPool;

    [SerializeField]
    private float m_ShotFrequency = 0.1f;

    private float m_TimeLastShotFired;

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
        if (m_Body == null)
            m_Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis(m_HorizontalInputAxis);
        float vertical = Input.GetAxis(m_VerticalInputAxis);

        Vector2 acceleration = new Vector2(horizontal, vertical);

        float accelerationMagnitude = Mathf.Clamp01(acceleration.magnitude);
        if (Mathf.Approximately(accelerationMagnitude, 0f)) return;
        acceleration /= Mathf.Clamp01(acceleration.magnitude);
        acceleration *= m_AccelerationPower;

        m_Body.AddForce(acceleration, ForceMode2D.Force);
    }

    private void HandleShooting()
    {
        if (!Input.GetButton(m_FireInputButton)) return;
        if (Time.time - m_TimeLastShotFired < m_ShotFrequency) return;

        m_TimeLastShotFired = Time.time;
        m_BulletPool.Get(transform.position, transform.rotation);
    }
}