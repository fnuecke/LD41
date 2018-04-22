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
            int childIndex = 0;
            foreach (string goalName in GoalManager.GetGoalNames())
            {
                int goalCount = GoalManager.GetGoalCount(goalName);
                string goalText = goalName + " (<color=lime>" + goalCount + "</color>)";
                if (childIndex < m_Container.childCount)
                {
                    Transform child = m_Container.GetChild(childIndex);
                    child.GetComponent<Text>().text = goalText;
                }
                else
                {
                    GameObject child = ObjectPool.Get(m_Prefab, Vector3.zero, Quaternion.identity, m_Container);
                    child.GetComponent<Text>().text = goalText;
                }
                ++childIndex;
            }
            for (int i = m_Container.childCount - 1; i >= childIndex; i--)
            {
                Destroy(m_Container.GetChild(i).gameObject);
            }
        }
    }
}