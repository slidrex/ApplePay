using UnityEngine;
public class WeaponPlaceAnimator
{
    private bool InAnimation;
    private Transform TransformObject;
    private WeaponItem activeWeaponInfo;
    private float animationTime;
    private float passedTime;
    private float animationSpeed;
    private float animationRotationSpeed;
    internal void StartAnimation(WeaponItem item, Transform movableTransform)
    {
        AnimationSetup(item, movableTransform);
        passedTime = 0f;
        InAnimation = true;
    }
    private void AnimationSetup(WeaponItem item, Transform movableTransform)
    {
        activeWeaponInfo = item;
        TransformObject = movableTransform;
        animationSpeed = item.WeaponInfo.GetVelocity();
        animationTime = activeWeaponInfo.WeaponInfo.GetAnimationTime();
        animationRotationSpeed = activeWeaponInfo.WeaponInfo.GetAngularVelocity();
        if(activeWeaponInfo.Weapon.AttackAnimationSettings.RandomAngularVelocityDirection) animationRotationSpeed *= Mathf.Round(Random.Range(0, 1f)) == 1 ? 1: -1;
        item.WeaponInfo.AnimationInfo.inAnimation = true;
    }
    private void AnimationUpdate()
    {
        TransformObject.position += TransformObject.up * activeWeaponInfo.Weapon.AttackAnimationSettings.VelocityPattern.Evaluate(passedTime/animationTime) * Time.deltaTime * animationSpeed;
        TransformObject.Rotate(Vector3.forward * activeWeaponInfo.Weapon.AttackAnimationSettings.AngularVelocityPattern.Evaluate(passedTime/animationTime) * Time.deltaTime * animationRotationSpeed * 180);
        passedTime += Time.deltaTime;
        if(passedTime >= animationTime) OnAnimationOver();
    }
    internal void Update()
    {
        if(InAnimation) AnimationUpdate();
    }
    private void OnAnimationOver()
    {
        MonoBehaviour.Destroy(TransformObject.gameObject);
        InAnimation = false;
        activeWeaponInfo.WeaponInfo.AnimationInfo.inAnimation = false;
    }
    public float GetRemainAnimationTime() => animationTime - passedTime;
}
