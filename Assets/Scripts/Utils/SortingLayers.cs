using UnityEngine;

namespace MightyPirates
{
    public static class SortingLayers
    {
        private static bool s_IsInitialized;

        private static int s_PlayerShots;
        private static int s_EnemyShots;

        public static int PlayerShots
        {
            get
            {
                Initialize();
                return s_PlayerShots;
            }
        }

        public static int EnemyShots
        {
            get
            {
                Initialize();
                return s_EnemyShots;
            }
        }

        private static void Initialize()
        {
            if (s_IsInitialized) return;
            s_IsInitialized = true;

            s_PlayerShots = SortingLayer.NameToID(nameof(PlayerShots));
            s_EnemyShots = SortingLayer.NameToID(nameof(EnemyShots));
        }
    }
}