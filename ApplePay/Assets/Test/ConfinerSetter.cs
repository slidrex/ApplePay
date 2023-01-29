using UnityEngine;
using Cinemachine;

public class ConfinerSetter : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner confiner;
    [SerializeField] private CinemachineVirtualCamera cam;
    private Creature holder;

    private void Start()
    {
        holder = cam.Follow.gameObject.GetComponent<Creature>();
        if(holder != null) holder.RoomChangeCallback += OnConfinerChange;
        confiner.m_BoundingShape2D = holder.CurrentRoom.CameraConfiner;
    }
    private void OnConfinerChange(Room newRoom, Room oldRoom)
    {
        
        confiner.m_BoundingShape2D = newRoom.CameraConfiner;
    }
    private void OnDestroy()
    {
        holder.RoomChangeCallback -= OnConfinerChange;
    }
}
