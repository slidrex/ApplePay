using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    public List<DropItem> DropItems = new List<DropItem>();
    public void DropLoot()
    {
        for(int i = 0; i < DropItems.Count; i++)
        {
            float rand = Random.Range(0, 100f);
            if(DropItems[i].DropChance >= rand) {
                var obj = Instantiate(DropItems[i].Item, transform.position, Quaternion.identity);
            }
        }
    }
    [System.Serializable]
    public struct DropItem
    {
        public GameObject Item;
        [Range(0f, 100f)] public float DropChance;
    }
}
