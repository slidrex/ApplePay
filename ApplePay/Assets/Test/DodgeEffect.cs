using UnityEngine;

public class DodgeEffect : MonoBehaviour
{
    [SerializeField] private GameObject collisionParticles;

    private void Start()
    {
        Invoke("ActivateParticles", 0.1f);
    } 
    private void ActivateParticles() => PayWorld.Particles.InstantiateParticles(collisionParticles, 
        transform.position, Quaternion.identity, 1, transform);
}
