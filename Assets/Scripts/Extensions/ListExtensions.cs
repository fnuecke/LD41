using System.Collections.Generic;
using UnityEngine;

namespace MightyPirates
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> self)
        {
            for (int i = 0; i < self.Count; i++)
            {
                int j = Random.Range(0, i + 1);
                T tmp = self[i];
                self[i] = self[j];
                self[j] = tmp;
            }
        }
    }
}