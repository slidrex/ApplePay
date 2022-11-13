public abstract class ItemAccessDatabase<K, V> : Database
{
    private bool Init = false;
    protected System.Collections.Generic.Dictionary<K, V> Items  {get; set;}
    protected abstract DatabaseValue[] items { get; set; }
    public void InitDatabase()
    {
        Items = new System.Collections.Generic.Dictionary<K, V>(items.Length);
        for(int i = 0; i < items.Length; i++)
        {
            Items.Add(items[i].Key, items[i].Value);
        }
    }
    public V GetItem(K key) 
    {
        if(Init == false) InitDatabase();
        V val;
        Items.TryGetValue(key, out val);
        return val;
    }
    [System.Serializable]
    protected struct DatabaseValue
    {
        public K Key;
        public V Value;
    }
}
