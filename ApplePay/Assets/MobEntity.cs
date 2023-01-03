public abstract class MobEntity : Creature
{
    [UnityEngine.HideInInspector] public Entity Target;
    public new MobMovement Movement => (MobMovement)base.Movement;
    public void SetTarget(Entity entity) => Target = entity;
    
    public Creature GetNearestHostileTarget()
    {
        float closestSqr = 0.0f;
        Creature closestEntity = null;
        
        foreach(Creature entity in CurrentRoom.EntityList)
        {
            if(entity != this)
            {
                foreach(PayTagHandler.EntityTag selfTag in EnemyTags)
                {
                    foreach(PayTagHandler.EntityTag selectedEntityTag in entity.Tags)
                    {
                        if(selfTag == selectedEntityTag)
                        {
                            UnityEngine.Vector2 distance = entity.transform.position - transform.position;
                            float currentEntitySqrDistance = distance.sqrMagnitude;

                            if(closestEntity == null || currentEntitySqrDistance < closestSqr)
                            {
                                closestSqr = currentEntitySqrDistance;
                                closestEntity = entity;
                            }
                        }
                    }
                }
            }
        }
        return closestEntity;
    }
}
