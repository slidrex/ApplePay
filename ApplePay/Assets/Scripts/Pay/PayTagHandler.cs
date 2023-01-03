public static class PayTagHandler
{
    public enum EntityTag
    {
        AGGRESSIVE,
        NEUTRAL,
        PLAYER,
    }
    public static Creature GetNearestRoomTagEntity(Creature entity, EntityTag tag)
    {
        float closestSqr = 0.0f;
        Creature creature = null;

        foreach(Creature mob in entity.CurrentRoom.EntityList)
        {
            foreach(EntityTag currentEntityTag in mob.Tags)
            {
                if(currentEntityTag == tag)
                {
                    float curSqrDist = (mob.transform.position - entity.transform.position).sqrMagnitude;
                    
                    if(curSqrDist < closestSqr || creature == null)
                    {
                        creature = mob;
                        closestSqr = curSqrDist;
                    }
                }
            }
        }
        return creature;
    }
    public static bool IsHostile(Creature current, Creature target)
    {
        foreach(PayTagHandler.EntityTag selfTag in current.EnemyTags)
        {
            foreach(PayTagHandler.EntityTag selectedEntityTag in target.Tags)
            {
                    if(selfTag == selectedEntityTag)
                    {
                        return true;
                    }
                }
            }
        return false;
    }
    public static bool IsAlly(Creature current, Creature target)
    {
        foreach(PayTagHandler.EntityTag selfTag in current.AllyTags)
        {
            foreach(PayTagHandler.EntityTag selectedEntityTag in target.Tags)
            {
                if(selfTag == selectedEntityTag)
                {
                    return true;
                }
            }
        }
        return false;
    }
}