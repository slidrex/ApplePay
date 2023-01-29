using UnityEngine;

public class LevelController : MonoBehaviour
{
    public RoomSpawner LevelRoomSpawner;
    public void UpdateRoomEntityList()
    {
        Creature[] entities = MonoBehaviour.FindObjectsOfType<Creature>();
        RoomSpawner.SpawnerRoom[] rooms = LevelRoomSpawner.ActiveLevelRooms;
        
        foreach(RoomSpawner.SpawnerRoom room in rooms) room.room.EntityList = new System.Collections.Generic.List<Creature>();
        
        foreach(Creature entity in entities) DefineCurrentRoom(entity);
        
    }
    public void DefineCurrentRoom(Creature creature)
    {
        Room oldRoom = creature.CurrentRoom;
        creature.CurrentRoom = null;
        
        foreach(RoomSpawner.SpawnerRoom room in LevelRoomSpawner.ActiveLevelRooms)
        {
            if(!room.room.EntityList.Contains(creature) && room.room.RoomConfiners.IsInsideBound(creature.transform.position) && !creature.isDead)
            {
                room.room.EntityList.Add(creature);
                creature.CurrentRoom = room.room;
                if(oldRoom != creature.CurrentRoom) creature.RoomChange(room.room, oldRoom);
            }
        }
    }
}
