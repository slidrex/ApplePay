using UnityEngine;
public class DoorBehaviour : KeyHoldingHack
{
    [ReadOnly] public Room AttachedRoom;
    [Header("Door Behaviour")]
    [SerializeField] private Transform TeleportPoint;
    [SerializeField] private DoorDirection direction;
    public DoorBehaviour ConnectedDoor;
    public Vector2 Direction {get 
    {
        directionConverter.TryGetValue(direction, out Vector2 val);
        return val;
    }
    }
    public void SwapDirection(int swap) => direction = (DoorDirection)Mathf.Repeat((int)direction + swap, 4);
    protected override void OnAfterHack()
    {
        base.OnAfterHack();
        if(ConnectedDoor.isUnlocked == false) 
        {
            ConnectedDoor.OnUnlock();
            ConnectedDoor.AttachedRoom.DefineRoom();
        }
    }
    protected override void OnInteractAction()
    {
        base.OnInteractAction();
        if(InteractEntity != null) InteractEntity.transform.position = ConnectedDoor.TeleportPoint.position;
    }
    private System.Collections.Generic.Dictionary<DoorDirection, Vector2> directionConverter = new System.Collections.Generic.Dictionary<DoorDirection, Vector2>()
    {
        [DoorDirection.Up] = Vector2.up,
        [DoorDirection.Right] = Vector2.right,
        [DoorDirection.Down] = Vector2.down,
        [DoorDirection.Left] = Vector2.left,
    };
}
public enum DoorDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
