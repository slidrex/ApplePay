public abstract class MobEntity : Creature
{
    [UnityEngine.HideInInspector] public Entity Target;
    public new MobMovement Movement => (MobMovement)base.Movement;
    public void SetTarget(Entity entity) => Target = entity;
    public bool IsHostile(Creature entity)
    {
        foreach(PayTagHandler.EntityTag selfTag in EntityTags)
        {
            if(PayTagHandler.HunterTargets.ContainsKey(selfTag))
            {
                PayTagHandler.HunterTargets.TryGetValue(selfTag, out PayTagHandler.EntityTag tag);
                foreach(PayTagHandler.EntityTag selectedEntityTag in entity.EntityTags)
                {
                    if(tag == selectedEntityTag)
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }
    public Creature GetNearestHostileTarget()
    {
        float closestSqr = 0.0f;
        Creature closestEntity = null;
        
        foreach(Creature entity in CurrentRoom.EntityList)
        {
            if(entity != this)
            {
                foreach(PayTagHandler.EntityTag selfTag in EntityTags)
                {
                    PayTagHandler.HunterTargets.TryGetValue(selfTag, out PayTagHandler.EntityTag tag);
                    
                    foreach(PayTagHandler.EntityTag selectedEntityTag in entity.EntityTags)
                    {
                        if(tag == selectedEntityTag)
                        {
                            UnityEngine.Vector2 distance = entity.transform.position - transform.position;
                            float currentEntityDistance = distance.sqrMagnitude;
                            if(closestEntity == null || distance.sqrMagnitude < closestSqr)
                            {
                                closestSqr = currentEntityDistance;
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
