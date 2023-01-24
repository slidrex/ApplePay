using UnityEngine;

public abstract class WeaponObject : MonoBehaviour
{    
    protected Creature Owner;
    public GameObject[] InstantiatedObjects;
    public void LinkAttacker(Creature attacker) => Owner = attacker;
    protected abstract GameObject[] OnActivate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target);
    public void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target)
    {
        InstantiatedObjects = OnActivate(attacker, originPosition, attackPosition, target);
    }
    protected virtual void Update() { }
}