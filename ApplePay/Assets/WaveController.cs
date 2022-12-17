using UnityEngine;
public class WaveController : MonoBehaviour
{
    //[SerializeField] private MarkDatabase markDatabase;
    internal struct BindedWaveStatus
    {
        internal WaveStatus status;
        internal float bindTime;
    }
    [SerializeField] private Creature WrappedCreature;
    private IWavedepent wrappedWaveComponent;
    private Room wrappedRoom;
    private WaveStatus wrappedEntityWaveStatus;
    [SerializeField] private BindedWaveStatus bindedStatus;
    private ActionMark[] wrappedRoomStages;
    private void Start()
    {
        AssignWrappedEntity(WrappedCreature);
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
    private void OnWaveBegin() => wrappedWaveComponent.SetWaveStatus(WaveStatus.InWave);
    public void UpdateWaveStatus()
    {
        
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

            wrappedRoom.ApplyMark(wrappedRoomStages[i]);
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