using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class Player : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    [SerializeField]
    private Rigidbody2D m_Body;

    [SerializeField]
    private string m_HorizontalInputAxis = "Horizontal";

    [SerializeField]
    private string m_VerticalInputAxis = "Vertical";

    [SerializeField]
    private float m_AccelerationPower = 5f;

    private void Awake()
    {
        if (m_Camera == null)
            m_Camera = GetComponent<Camera>();
        if (m_Body == null)
            m_Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
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
}