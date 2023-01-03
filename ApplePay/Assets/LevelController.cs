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
}
