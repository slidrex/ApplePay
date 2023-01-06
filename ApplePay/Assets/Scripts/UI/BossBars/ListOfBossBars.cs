using UnityEngine;
using System.Collections.Generic;

public class ListOfBossBars : MonoBehaviour
{
    public static List<BossBar> bossBars = new List<BossBar>();
    public static bool IsHave = false;
    public static void AddBossBar(BossEntity boss)
    {
        boss.bossBarInstance = Instantiate(boss.GetBossBar(), boss.transform.position, Quaternion.identity, BossEntity.listOfBossBarsInstance.transform);
        boss.GetBossBar().SetEntity(boss);
        bossBars.Add(boss.bossBarInstance);
    }
    public static void DestroyBossBar(BossEntity boss)
    {
        bossBars.Remove(boss.bossBarInstance);
        if(bossBars.Count <= 0)
        {
            ListOfBossBars.IsHave = false;
            Destroy(BossEntity.listOfBossBarsInstance.gameObject);
            BossEntity.listOfBossBarsInstance = null;
        }
        if(boss.bossBarInstance.gameObject != null) Destroy(boss.bossBarInstance.gameObject);
    }
    public void OnDestroy()
    {
        if(IsHave == true)
        {
            ListOfBossBars.IsHave = false;
            Destroy(BossEntity.listOfBossBarsInstance.gameObject);
            BossEntity.listOfBossBarsInstance = null;
        }
    }
}
