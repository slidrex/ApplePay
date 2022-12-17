using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EntityChangeHealthCallaback
{
    ///<summary>Calls before entity takes damage.</summary>
    void BeforeDamageCallback(ref Creature handler, ref Damage[] damages);
}
