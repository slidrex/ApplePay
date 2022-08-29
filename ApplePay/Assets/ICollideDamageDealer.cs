public interface ICollideDamageDealer
{
    public int CollideDamage {get; set;}
    public void DealCollideDamage(Entity entity, int damage, Creature dealer);
}
