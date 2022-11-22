public struct FloatRef
{
    public System.Func<float> Get;
    public System.Action<float> Set;
    public FloatRef(System.Func<float> getter, System.Action<float> setter)
    {
        Get = getter;
        Set = setter;
    }
}
