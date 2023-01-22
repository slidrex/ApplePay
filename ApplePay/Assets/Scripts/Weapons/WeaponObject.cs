using UnityEngine;

public abstract class WeaponObject : MonoBehaviour
{    
    protected Creature Owner;
    public void LinkAttacker(Creature attacker) => Owner = attacker;
    public virtual void Activate(Creature attacker, Vector2 originPosition, Vector2 attackPosition, Transform target, out Projectile projectile) => projectile = null;
    protected virtual void Update() { }
}