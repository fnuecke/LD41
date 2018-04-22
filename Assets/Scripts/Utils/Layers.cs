using UnityEngine;

namespace MightyPirates
{
    public static class Layers
    {
        private static bool s_IsInitialized;

        private static int s_Player;
        private static int s_Minions;
        private static int s_Enemies;
        private static int s_PlayerShots;
        private static int s_EnemyShots;
        private static int s_Picking;
        private static int s_Pickup;

        private static int s_PlayerMask;
        private static int s_EnemyMask;
        private static int s_PickingMask;
        private static int s_PickupMask;

        public static int PlayerMask
        {
            get
            {
                Initialize();
                return s_PlayerMask;
            }
        }

        public static int EnemyMask
        {
            get
            {
                Initialize();
                return s_EnemyMask;
            }
        }

        public static int PickingMask
        {
            get
            {
                Initialize();
                return s_PickingMask;
            }
        }

        public static int PickupMask
        {
            get
            {
                Initialize();
                return s_PickupMask;
            }
        }

        public static int Player
        {
            get
            {
                Initialize();
                return s_Player;
            }
        }

        public static int Minions
        {
            get
            {
                Initialize();
                return s_Minions;
            }
        }

        public static int Enemies
        {
            get
            {
                Initialize();
                return s_Enemies;
            }
        }

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

        public static int Picking
        {
            get
            {
                Initialize();
                return s_Picking;
            }
        }

        public static int Pickup
        {
            get
            {
                Initialize();
                return s_Pickup;
            }
        }

        public static bool IsPlayer(int layer)
        {
            return ((1 << layer) & PlayerMask) != 0;
        }

        public static bool IsEnemy(int layer)
        {
            return ((1 << layer) & EnemyMask) != 0;
        }

        private static void Initialize()
        {
            if (s_IsInitialized) return;
            s_IsInitialized = true;

            s_Player = LayerMask.NameToLayer(nameof(Player));
            s_Minions = LayerMask.NameToLayer(nameof(Minions));
            s_Enemies = LayerMask.NameToLayer(nameof(Enemies));
            s_PlayerShots = LayerMask.NameToLayer(nameof(PlayerShots));
            s_EnemyShots = LayerMask.NameToLayer(nameof(EnemyShots));
            s_Picking = LayerMask.NameToLayer(nameof(Picking));
            s_Pickup = LayerMask.NameToLayer(nameof(Pickup));

            s_PlayerMask = LayerMask.GetMask(nameof(Player), nameof(Minions));
            s_EnemyMask = LayerMask.GetMask(nameof(Enemies));
            s_PickingMask = LayerMask.GetMask(nameof(Picking));
            s_PickupMask = LayerMask.GetMask(nameof(Pickup));
        }
    }
}