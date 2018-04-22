namespace MightyPirates
{
    public abstract class Powerup : Pickupable
    {
        public abstract void Activate(Player player);
    }
}