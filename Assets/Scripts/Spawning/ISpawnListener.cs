using UnityEngine;

namespace MightyPirates
{
    public interface ISpawnListener
    {
        void HandleSpawned(GameObject spawner);
    }
}