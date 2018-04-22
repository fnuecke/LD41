using System;
using UnityEngine;

namespace MightyPirates
{
    [DefaultExecutionOrder((int) ExectionOrders.Goal)]
    public abstract class GoalBase : MonoBehaviour
    {
        public event Action GoalStateChanged;

        public abstract string Title { get; }

        private void OnEnable()
        {
            GoalManager.Add(this);
        }

        private void OnDisable()
        {
            GoalManager.Remove(this);

            GoalStateChanged = null;
        }

        protected void OnGoalStateChanged()
        {
            GoalStateChanged?.Invoke();
        }
    }
}