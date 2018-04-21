using UnityEngine;

namespace MightyPirates
{
    [RequireComponent(typeof(TargetTracker))]
    public abstract class TargetTrackingBehaviour : MonoBehaviour
    {
        protected TargetTracker m_TargetTracker;

        protected virtual void Awake()
        {
            if (m_TargetTracker == null)
                m_TargetTracker = GetComponent<TargetTracker>();
        }
    }
}