using UnityEngine;
public class WaveController : MonoBehaviour
{
    [SerializeField] private MarkDatabase markDatabase;
    internal struct BindedWaveStatus
    {
        internal WaveStatus status;
        internal float bindTime;
    }
    [SerializeField] private Creature WrappedCreature;
    private IWavedepent wrappedWaveComponent;
    private Room wrappedRoom;
    [ReadOnly, SerializeField] private WaveStatus wrappedEntityWaveStatus;
    [SerializeField] private BindedWaveStatus bindedStatus;
    private RoomMark[] wrappedRoomStages;
    private void Start()
    {
        AssignWrappedEntity(WrappedCreature);
        GlobalEventManager.OnActionActivated += WaveUpdater;
        if(WrappedCreature == null)
            throw new System.NullReferenceException("Wrapped Creature hasn't been assigned.");
    }
    private void Update()
    {
        if(bindedStatus.bindTime > 0) 
        {
            BindedWaveStatus curStatus = bindedStatus;
            curStatus.bindTime -= Time.deltaTime;
        
            if(curStatus.bindTime < 0) curStatus = new BindedWaveStatus();
            bindedStatus = curStatus;
        }
    }
    private void OnDestroy() => GlobalEventManager.OnActionActivated -= WaveUpdater;
    private void OnRoomSwitched()
    {
        byte stageCount = (byte)Random.Range(WrappedCreature.CurrentRoom.MinStageCount, WrappedCreature.CurrentRoom.MaxStageCount);
        wrappedRoomStages = new RoomMark[stageCount];
        StageMarkAssign(wrappedRoomStages);
        foreach(RoomMark roomMark in WrappedCreature.CurrentRoom.MarkList)
        {
            if(roomMark.MarkType == MarkDatabase.MarkType.Mob)
            {
                MobMark curMark = (MobMark)roomMark;
                if(curMark.SpawnDelay > bindedStatus.bindTime)
                {
                    bindedStatus.status = WaveStatus.InWave;
                    bindedStatus.bindTime = curMark.SpawnDelay + curMark.MaxSpawnInterval * (curMark.MinMobCount + curMark.MaxMobCount)/2;
                }
            }
        }
        
    }
    private void StageMarkAssign(RoomMark[] markArray)
    {
        for(int i = 0; i < markArray.Length; i++)
        {
            while(markArray[i] == null)
            {
                int rand = Random.Range(0, 100);
                MarkDatabase.MarkSlot currentRoomMark = markDatabase.MarkList[Random.Range(0, markDatabase.MarkList.Length)];
                if(currentRoomMark.SpawnChance >= rand && currentRoomMark.Mark.MarkType == MarkDatabase.MarkType.Mob)
                {
                    markArray[i] = currentRoomMark.Mark;
                    break;
                }
            }
        }
    }
    private void OnWaveBegin() => wrappedWaveComponent.SetWaveStatus(WaveStatus.InWave);
    public void WaveUpdater()
    {
        wrappedEntityWaveStatus = wrappedWaveComponent.WaveStatus;
        if(bindedStatus.bindTime > 0)
        {
            wrappedWaveComponent.SetWaveStatus(bindedStatus.status);
            return;
        }
        if(wrappedRoom != null && WrappedCreature.CurrentRoom != null && WrappedCreature.CurrentRoom != wrappedRoom)
        {
            OnRoomSwitched();
            foreach(Creature roomCreature in wrappedRoom.EntityList)
            {
                foreach(Creature hostileCreature in WrappedCreature.Hostiles)
                {
                    if(!roomCreature.isDead && hostileCreature.GetType() == roomCreature.GetType())
                    {
                        if(wrappedWaveComponent.WaveStatus == WaveStatus.NoWave) OnWaveBegin();
                        return;
                    }
                }
            }
        }
        wrappedRoom = WrappedCreature.CurrentRoom;
        
        if(wrappedWaveComponent.WaveStatus == WaveStatus.InWave) OnWaveEnd();
    }
    private void OnWaveEnd()
    {
        bool released;
        TryStageRelease(out released);
        if(released) return;
        wrappedWaveComponent.SetWaveStatus(WaveStatus.NoWave);
    }
    private void TryStageRelease(out bool released)
    {
        for(int i = 0; i < wrappedRoomStages.Length; i++)
        {
            if(wrappedRoomStages[i] == null) continue;

            FindObjectOfType<RoomDefiner>().ApplyMark(wrappedRoomStages[i], WrappedCreature.CurrentRoom);
            wrappedRoomStages[i] = null;
            released = true;
            return;
        }
        released = false;
    }
    public void AssignWrappedEntity(Creature wrappedCreature)
    {
        if(wrappedCreature.GetComponent<IWavedepent>() == null)
            throw new System.NullReferenceException("Wrapped entity doesn't contain \"IWavedepent\" interface.");
        else
        {
            wrappedWaveComponent = wrappedCreature.GetComponent<IWavedepent>();
            WrappedCreature = wrappedCreature;
        }
    }
}
public enum WaveStatus
{
    NoWave,
    InWave
}