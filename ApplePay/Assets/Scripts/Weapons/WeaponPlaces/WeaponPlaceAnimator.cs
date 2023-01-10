using UnityEngine;
public class WeaponPlaceAnimator
{
    private bool InAnimation;
    private Transform TransformObject;
    private InstantiateWeapon activeWeaponInfo;
    private float animationTime;
    private float passedTime;
    private float animationSpeed;
    private float animationRotationSpeed;
    internal void StartAnimation(InstantiateWeapon item, Transform movableTransform)
    {
        AnimationSetup(item, movableTransform);
        passedTime = 0f;
        InAnimation = true;
    }
    private void AnimationSetup(InstantiateWeapon item, Transform movableTransform)
    {
        activeWeaponInfo = item;
        TransformObject = movableTransform;
        animationSpeed = item.animationInfo.linearVelocity;
        animationTime = activeWeaponInfo.animationInfo.animationTime;
        animationRotationSpeed = activeWeaponInfo.animationInfo.angularVelocity;
        if(activeWeaponInfo.animationPreset.RandomAngularVelocityDirection) animationRotationSpeed *= Mathf.Round(Random.Range(0, 1f)) == 1 ? 1: -1;
        item.animationInfo.inAnimation = true;
    }
    private void AnimationUpdate()
    {
        TransformObject.position += TransformObject.up * activeWeaponInfo.animationPreset.VelocityPattern.Evaluate(passedTime/animationTime) * Time.deltaTime * animationSpeed;
        TransformObject.Rotate(Vector3.forward * activeWeaponInfo.animationPreset.AngularVelocityPattern.Evaluate(passedTime/animationTime) * Time.deltaTime * animationRotationSpeed * 180);
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
        activeWeaponInfo.weaponInfo.SetCooldown();
        InAnimation = false;
        passedTime = 0;
        activeWeaponInfo.animationInfo.inAnimation = false;
    }
    public float GetRemainAnimationTime() => animationTime - passedTime;
}
