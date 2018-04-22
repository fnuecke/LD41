namespace MightyPirates.Powerups
{
    public sealed class IncreaseMinionCount : Powerup
    {
        public override void Activate(Player player)
        {
            Spawner spawner = player.GetComponentInChildren<Spawner>();
            if (spawner == null)
                return;
            spawner.MaxAlive++;
        }
    }
}