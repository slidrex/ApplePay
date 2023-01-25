public class CharmObject : UnityEngine.ScriptableObject 
{
    public virtual void OnInstantiate() {}
        public enum CharmType
        {
            Base,
            Switchable
        }
    public CharmType charmType 
    {
        get
        {
            CharmObject charm = this as Charm;
            if(charm == null)
            {
                return CharmType.Switchable;
            }
            return CharmType.Base;
        }
    }
    public Charm GetActiveCharm()
    {
        if(charmType == CharmType.Base)
        {
            return (Charm)this;
        }
        if(charmType == CharmType.Switchable)
        {
            MixedCharm charm = (MixedCharm)this;
            return charm.Charms[charm.ActiveIndex];
        }
        return null;
    }
}