using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MightyPirates.UI
{
    [DefaultExecutionOrder((int) ExectionOrders.GoalsUi)]
    public sealed class UiGoals : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Prefab;

        [SerializeField]
        private RectTransform m_Container;

        [SerializeField]
        private Image m_Background;

        private List<Text> m_GoalItems = new List<Text>();

        private void OnEnable()
        {
            GoalManager.GoalsChanged += HandleGoalsChanged;
            UpdateGoals();
        }

        private void OnDisable()
        {
            GoalManager.GoalsChanged -= HandleGoalsChanged;
        }

        private void HandleGoalsChanged()
        {
            UpdateGoals();
        }

        private void UpdateGoals()
        {
            if (m_GoalItems == null)
                m_GoalItems = new List<Text>();
            int goalIndex = 0;
            foreach (string goalName in GoalManager.GetGoalNames())
            {
                int goalCount = GoalManager.GetGoalCount(goalName);
                string goalText = goalName + " (<color=lime>" + goalCount + "</color>)";
                if (goalIndex < m_GoalItems.Count)
                {
                    Text item = m_GoalItems[goalIndex];
                    item.text = goalText;
                }
                else
                {
                    GameObject child = ObjectPool.Get(m_Prefab, Vector3.zero, Quaternion.identity, m_Container);
                    Text item = child.GetComponent<Text>();
                    m_GoalItems.Add(item);
                    item.text = goalText;
                }
                ++goalIndex;
            }

            for (int i = goalIndex; i < m_GoalItems.Count; ++i)
            {
                Text item = m_GoalItems[i];
                item.gameObject.Free();
            }
            m_GoalItems.RemoveRange(goalIndex, m_GoalItems.Count - goalIndex);

            m_Background.enabled = m_GoalItems.Count > 0;
        }
    }
}