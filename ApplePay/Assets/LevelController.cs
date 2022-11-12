using UnityEngine;

public class LevelController : MonoBehaviour
{
    public RoomSpawner LevelRoomSpawner;
    private void Awake()
    {
        TagsSetup();
    }
    public System.Collections.Generic.Dictionary<string, string[]> Tags;
    [SerializeField] private EntityTag[] tags;
    [System.Serializable]
    public struct EntityTag
    {
        public string Tag;
        public string[] HostileTags;
    }
    private void TagsSetup()
    {
        Tags = new System.Collections.Generic.Dictionary<string, string[]>(tags.Length);
        for(int i = 0; i < tags.Length; i++)
        {
            Tags.Add(tags[i].Tag, tags[i].HostileTags);
        }
    }
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
        internal static bool AreHostiles(Creature first, Creature second)
        {   
            string[] firstTags = first.Tags;
            foreach(string firstTag in firstTags)
            {
                string[] hostileTags;
                first.LevelController.Tags.TryGetValue(firstTag, out hostileTags);
                
                    foreach(string secondTag in second.Tags)
                    {
                            foreach(string hostileTag in hostileTags)
                            {
                                if(secondTag.Equals(hostileTag))
                                {
                                    return true;
                                }
                            }
                    }
            }
            return false;
        }
        internal static Creature GetNearestHostile(Room room, Creature creature)
        {
            Creature[] hostiles = GetHostiles(room, creature);
            if(hostiles.Length == 0) return null;
            float closest = Vector2.Distance(creature.transform.position, hostiles[0].transform.position);
            int closestIndex = 0;
            for(int i = 1; i < hostiles.Length; i++)
            {
                float curDist = Vector2.Distance(creature.transform.position, hostiles[i].transform.position);
                if(curDist < closest)
                {
                    closestIndex = i;
                    closest = curDist;
                }
            }
            return hostiles[closestIndex];
        }
        internal static Creature[] GetHostiles(Room room, Creature creature)
        {
            System.Collections.Generic.List<string> hostileTags = GetHostileTags(creature);
            System.Collections.Generic.List<Creature> entities = room.EntityList;
            System.Collections.Generic.List<Creature> hostiles = new System.Collections.Generic.List<Creature>();
            entities.Remove(creature);

            foreach(Creature entity in entities)
            {
                foreach(string tag in entity.Tags)
                {
                    if(hostileTags.Contains(tag)) 
                    {
                        hostiles.Add(entity);
                        break;
                    }
                }
            }
            return hostiles.ToArray();
        }
        internal static bool ContainsHostiles(Creature creature)
        {
            System.Collections.Generic.List<string> hostileTags = GetHostileTags(creature);
            foreach(Creature entity in creature.CurrentRoom.EntityList)
            {
                if(entity != creature)
                {
                    foreach(string tag in entity.Tags)
                    {
                        if(hostileTags.Contains(tag)) return true;
                    }
                }
            }
            return false;
        }
        private static System.Collections.Generic.List<string> GetHostileTags(Creature creature)
        {
            System.Collections.Generic.List<string> hostileTags = new System.Collections.Generic.List<string>();
            foreach(string tag in creature.Tags)
            {
                if(creature.LevelController.Tags.TryGetValue(tag, out string[] _hostileTags))
                {
                    hostileTags.AddRange(_hostileTags); 
                }

            }
            return hostileTags;
        }
    } 
}
