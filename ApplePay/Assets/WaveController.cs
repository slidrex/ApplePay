using UnityEngine;
public static class WaveController
{
    private static float WaveEndDelayTime;
    public static Creature WaveCreature;
    public static IWavedepent WaveImplementation;
    public static GameObject StageClearEffect {get; private set;}
    private static DoorBehaviour[] doors;
    public static void OnWaveBegin()
    {
        doors = WaveCreature.CurrentRoom.Doors;
        for (int i = 0; i < doors.Length; i++)
        {
            if(doors[i] != null && doors[i].InWaveEffect != null)
            {
                doors[i].Animator.SetTrigger("WaveBegun");
                doors[i].InWaveEffect.Play();
            }
        }
    }
    public static void OnWaveEnd()
    {
        doors = WaveCreature.CurrentRoom.Doors;
        for (int i = 0; i < doors.Length; i++)
        {
            if(doors[i] != null && doors[i].InWaveEffect != null)
            {
                doors[i].Animator.SetTrigger("WaveEnd");
                doors[i].InWaveEffect.Stop();
            }
        }
    }
    public static void SetupWaveEntity(Creature wrappedEntity, IWavedepent waveImplementation, float waveEndDelayTime)
    {
        WaveCreature = wrappedEntity;
        WaveImplementation = waveImplementation;
        WaveEndDelayTime = waveEndDelayTime;
    }
    public static void InitWave()
    {
        OnWaveBegin();
        StageClearEffect = Resources.Load<GameObject>("StageClearEffect");
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
            Pay.UI.UIHolder holder = WaveCreature.GetComponent<IUIHolder>().GetHolder();
            currentRoom.NextRoomStage();
            if(StageClearEffect != null) PayWorld.Particles.InstantiateParticles(StageClearEffect, holder.HUDCanvas.transform.position, 
                UnityEngine.Quaternion.identity, 0.25f, holder.HUDCanvas.transform);
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