using UnityEngine;

public static class RoomExtension
{
    public static bool ConnectRooms(Room first, Room second, Vector2 offset)
    {
        for(int i = 0; i < first.Doors.Length; i++)
        {
            DoorBehaviour firstDoor = first.Doors[i];
                if(firstDoor.Direction == offset)
                {
                    for(int j = 0; j < second.Doors.Length; j++)
                    {
                        DoorBehaviour secondDoor = second.Doors[j];
                        if(secondDoor.Direction == -offset)
                        {
                            firstDoor.SetConnectedDoor(secondDoor, secondDoor.GetSprite());
                            secondDoor.SetConnectedDoor(firstDoor, secondDoor.GetSprite());
                        }
                    }
                    return true;
                }
        }
        return false;
    }
    public static bool AreRoomsConnected(Room first, Room second)
    {
        foreach(DoorBehaviour door in first.Doors)
        {
            if(door != null && door.ConnectedDoor != null && door.ConnectedDoor.AttachedRoom == second) return true;
        }
        return false;
    }
}
