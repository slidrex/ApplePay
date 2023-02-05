using UnityEngine;
using Cinemachine;

public class ConfinerSetter : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner confiner;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private GameObject fade;
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
        if(holder is PlayerEntity player)
        {
            Transform canvas = player.GetHolder().HUDCanvas.transform;
            GameObject obj = Instantiate(fade, canvas.position, Quaternion.identity, canvas);
            Destroy(obj, 0.25f);
        }
    }
    private void OnDestroy() =>
        holder.RoomChangeCallback -= OnConfinerChange;
}
