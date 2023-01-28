using UnityEngine;

public class RoomRenderAnchor : MonoBehaviour
{
    private Room[] rooms;
    private void Start()
    {
        InitRoomStack();
    }
    private void InitRoomStack()
    {
        rooms = FindObjectsOfType<Room>();
    }
    public void UnrenderStack()
    {
        foreach(Room room in rooms) room.gameObject.SetActive(false);
    }
    public void UnrenderRoom(Room room)
    {
        foreach(Room obj in rooms)
        {
            if(obj == room) obj.gameObject.SetActive(false);
        }
    }
    public void RenderRoom(Room room)
    {
        foreach(Room obj in rooms)
        {
            if(obj == room) obj.gameObject.SetActive(true);
        }
    }
}
