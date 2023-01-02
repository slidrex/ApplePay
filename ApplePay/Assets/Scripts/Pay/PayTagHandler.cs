public static class PayTagHandler
{
    public static System.Collections.Generic.Dictionary<EntityTag, EntityTag> HunterTargets = new System.Collections.Generic.Dictionary<EntityTag, EntityTag>()
    {
        [EntityTag.PLAYER_HUNTER] = EntityTag.PLAYER,
        [EntityTag.AGGRESSIVE_HUNTER] = EntityTag.AGGRESSIVE,
        [EntityTag.NEUTRAL_HUNTER] = EntityTag.NEUTRAL,
        [EntityTag.PLAYER_HUNTER] = EntityTag.PLAYER,
    };
    public enum EntityTag
    {
        AGGRESSIVE,
        NEUTRAL,
        PLAYER_HUNTER,
        AGGRESSIVE_HUNTER,
        NEUTRAL_HUNTER,
        PLAYER,
    }
    public static Creature GetNearestTagEntity(Creature entity, EntityTag tag)
    {
        foreach(Creature mob in entity.CurrentRoom.EntityList)
        {
            foreach(EntityTag currentEntityTag in mob.EntityTags)
            {
                if(currentEntityTag == tag)
                {
                    return mob;
                }
            }
        }
        return null;
    }
}