using UnityEngine;

public class LevelController : MonoBehaviour
{
    public RoomSpawner LevelRoomSpawner;
    public void UpdateRoomEntityList()
    {
        Creature[] entities = MonoBehaviour.FindObjectsOfType<Creature>();
        Room[] rooms = LevelRoomSpawner.ActiveLevelRooms;

        foreach(Room room in rooms) room.EntityList = new System.Collections.Generic.List<Creature>();

        foreach(Creature entity in entities) DefineCurrentRoom(entity);
        
    }
    public void DefineCurrentRoom(Creature creature)
    {
        creature.CurrentRoom = null;
        
        foreach(Room room in LevelRoomSpawner.ActiveLevelRooms)
        {
            if(!room.EntityList.Contains(creature) && room.RoomConfiners.IsInsideBound(creature.transform.position) && !creature.isDead)
            {
                room.EntityList.Add(creature);
                creature.CurrentRoom = room;
                
            }
        }
    }
    internal static class EntityTagHandler
    {
        ///<summary>
        ///Gets the nearest entity that contains specified tag.
        ///</summary>
        internal static Creature GetNearestTagEntity(Room room, Creature creature, string tag)
        {
            float closestSqr = -1;
            Creature closestCreature = null;

            foreach(Creature currentCreature in room.EntityList)
            {
                foreach(string curTag in currentCreature.Tags)
                {
                    if(curTag == tag)
                    {
                        float currentSqrDistance = Vector2.SqrMagnitude(currentCreature.transform.position - creature.transform.position);
                        if(currentSqrDistance < closestSqr || closestSqr == -1)
                        {
                            closestCreature = currentCreature;
                            closestSqr = currentSqrDistance;
                        }
                        break;
                    }
                }
            }
            
            return closestCreature;
        }
    } 
}
