public static class WaveController
{
    private static float WaveEndDelayTime;
    public static Creature WaveCreature;
    public static IWavedepent WaveImplementation;
    public static void OnWaveBegin() { }
    public static void OnWaveEnd() { }
    public static void SetupWaveEntity(Creature wrappedEntity, IWavedepent waveImplementation, float waveEndDelayTime)
    {
        WaveCreature = wrappedEntity;
        WaveImplementation = waveImplementation;
        WaveEndDelayTime = waveEndDelayTime;
    }
    public static void InitWave()
    {
        OnWaveBegin();
        SetWaveStatus(WaveStatus.InWave);
    }
    public static void UpdateWaveStatus()
    {
        Room currentRoom = WaveCreature.CurrentRoom;
        foreach(Creature entity in currentRoom.EntityList)
        {
            if(entity.WaveRelation == EntityWaveType.BlockWave)
            {
                return;
            }
        }
        if(currentRoom.IsRedifinable() == true && currentRoom.IsExecutingMark() == false)
        {
            currentRoom.NextRoomStage();
        }
        else if(currentRoom.IsReleased())
        {
            StaticCoroutine.BeginCoroutine(DelayWaveEnd());
        }
    }
    public static void SetWaveStatus(WaveStatus waveStatus) => WaveImplementation.WaveStatus = waveStatus;
    public static System.Collections.IEnumerator DelayWaveEnd()
    {
        yield return new UnityEngine.WaitForSeconds(WaveEndDelayTime);
        WaveImplementation.WaveStatus = WaveStatus.NoWave;
        OnWaveEnd();
    }
}
public enum EntityWaveType
{
    ///<summary>
    ///Wave is not over if BlockWave Entities are present in the room.
    ///</summary>
    BlockWave,
    NotBlockWave
}
public enum WaveStatus
{
    NoWave,
    InWave
}