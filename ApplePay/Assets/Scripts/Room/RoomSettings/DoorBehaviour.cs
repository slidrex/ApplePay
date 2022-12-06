using UnityEngine;

public class DoorBehaviour : KeyHoldingHack
{
    public Room AttachedRoom { get; set; }
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
    protected override void OnAfterHack(InteractManager interactEntity)
    {
        base.OnAfterHack(interactEntity);
        if(ConnectedDoor.isUnlocked == false) 
        {
            ConnectedDoor.OnUnlock();
            ConnectedDoor.AttachedRoom.DefineRoom();
        }
    }
    public override bool BeforeInteractBegin(InteractManager interactEntity)
    {
        return WaveStatusCheck(interactEntity.entity) == WaveStatus.NoWave ? true : false;
    }
    protected override void OnInteractAction(InteractManager interactEntity)
    {
        base.OnInteractAction(interactEntity);
        if(interactEntity != null) 
        {
            interactEntity.transform.position = ConnectedDoor.TeleportPoint.position;
            interactEntity.entity.LevelController.UpdateRoomEntityList();
        }
    }
    private WaveStatus WaveStatusCheck(Creature entity)
    {
        IWavedepent wavedepentComponent = entity.GetComponent<IWavedepent>();
        if(wavedepentComponent == null) return WaveStatus.NoWave;
        return wavedepentComponent.WaveStatus;
    }
    public override void OnInteractBegin(InteractManager interactEntity)
    {
        base.OnInteractBegin(interactEntity);
        interactEntity.anim.SetBool("isUnhacking", false);
        interactEntity.anim.SetTrigger("isHacking");
    }
    protected override void OnInteractEnd(InteractManager interactEntity)
    {
        base.OnInteractEnd(interactEntity);
        interactEntity.anim.SetBool("isUnhacking", true);
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
