using System;
using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    [DefaultExecutionOrder((int) ExectionOrders.GoalManager)]
    public sealed class GoalManager : MonoBehaviour
    {
        public static event Action GoalsChanged
        {
            add
            {
                if (s_Instance != null) s_Instance.GoalsChangedInternal += value;
            }
            remove
            {
                if (s_Instance != null) s_Instance.GoalsChangedInternal -= value;
            }
        }

        public static void Add(GoalBase goal)
        {
            if (s_Instance != null)
                s_Instance.AddInternal(goal);
        }

        public static void Remove(GoalBase goal)
        {
            if (s_Instance != null)
                s_Instance.RemoveInternal(goal);
        }

        public static ICollection<string> GetGoalNames() => s_Instance.m_Goals.Keys;

        public static int GetGoalCount(string title) => s_Instance.m_Goals[title].Count;

        private event Action GoalsChangedInternal;

        private Dictionary<string, List<GoalBase>> m_Goals = new Dictionary<string, List<GoalBase>>();

        private static GoalManager s_Instance;

        private void OnEnable()
        {
            s_Instance = this;
            if (m_Goals == null)
                m_Goals = new Dictionary<string, List<GoalBase>>();
            m_Goals.Clear();
            foreach (GoalBase goal in FindObjectsOfType<GoalBase>())
                AddInternal(goal);
        }

        private void OnDisable()
        {
            s_Instance = null;
        }

        private void AddInternal(GoalBase goal)
        {
            if (!isActiveAndEnabled)
                return;
            if (m_Goals == null)
                m_Goals = new Dictionary<string, List<GoalBase>>();
            List<GoalBase> list;
            if (!m_Goals.TryGetValue(goal.Title, out list))
            {
                list = new List<GoalBase>();
                m_Goals.Add(goal.Title, list);
            }
            if (list.Contains(goal))
                return;
            list.Add(goal);

            goal.GoalStateChanged += HandleGoalChanged;
            OnGoalsChangedInternal();
        }

        private void RemoveInternal(GoalBase goal)
        {
            if (!isActiveAndEnabled)
                return;
            if (m_Goals == null)
                return;
            List<GoalBase> list;
            if (!m_Goals.TryGetValue(goal.Title, out list))
                return;
            if (list.Remove(goal))
            {
                if (list.Count == 0)
                    m_Goals.Remove(goal.Title);

                goal.GoalStateChanged -= HandleGoalChanged;
                OnGoalsChangedInternal();
            }
        }

        private void HandleGoalChanged()
        {
            OnGoalsChangedInternal();
        }

        private void OnGoalsChangedInternal()
        {
            GoalsChangedInternal?.Invoke();
        }
    }
}