using UnityEngine;

namespace MightyPirates
{
    [DefaultExecutionOrder((int) ExectionOrders.PlayerCamera + 1)]
    [RequireComponent(typeof(Canvas))]
    public sealed class AssignMainCamera : MonoBehaviour
    {
        private void Update()
        {
            if (Camera.main == null)
                return;

            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null)
                canvas.worldCamera = Camera.main;

            enabled = false;
        }
    }
}