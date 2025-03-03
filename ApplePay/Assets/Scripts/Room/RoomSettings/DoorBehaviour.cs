using UnityEngine;

public class DoorBehaviour : KeyHoldingHack
{
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Room AttachedRoom { get; set; }
    [Header("Door Behaviour")]
    [SerializeField] private Transform TeleportPoint;
    [SerializeField] private DoorDirection direction;
    public DoorBehaviour ConnectedDoor { get; private set; }
    public Vector2 Direction {get 
    {
        directionConverter.TryGetValue(direction, out Vector2 val);
        return val;
    }
    }
    private void Awake()
    {
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public Sprite GetSprite()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        return spriteRenderer.sprite;
    }
    public void SetConnectedDoor(DoorBehaviour door, Sprite connectedDoorSprite)
    {
        ConnectedDoor = door;
        ConnectedDoor.spriteRenderer.sprite = connectedDoorSprite;
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
    protected override void OnInteractBegin(InteractManager interactEntity)
    {
        base.OnInteractBegin(interactEntity);
        interactEntity.anim.SetBool("isUnhacking", false);
        interactEntity.anim.SetTrigger("isHacking");
    }
    protected override void OnInteractInterruption(InteractManager interactEntity)
    {
        base.OnInteractInterruption(interactEntity);
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
