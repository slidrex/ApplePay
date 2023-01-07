public static class RoomDefiner
{
    public static void ActivateRoomMarks(this Room room)
    {
        for(int i = 0; i < room.MarkList.Count; i++)
        {
            room.ApplyMark(room.MarkList[i]);
        }
    }
    
    public static void ApplyMark(this Room room, ActionMark mark) => mark.ApplyMark(room);
}