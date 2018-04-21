using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    public sealed class Minion : MonoBehaviour
    {
        public static readonly LinkedList<Minion> All = new LinkedList<Minion>();

        private LinkedListNode<Minion> m_Node;

        private void OnEnable()
        {
            m_Node = All.AddLast(this);
        }

        private void OnDisable()
        {
            All.Remove(m_Node);
        }
    }
}