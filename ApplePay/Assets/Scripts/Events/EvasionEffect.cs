using UnityEngine;

public class EvasionEffect : MonoBehaviour
{
    [SerializeField] private GameObject collisionParticles;

    private void Awake()
    {
        Invoke("ActivateParticles", 0.05f);
    } 
    private void ActivateParticles() => PayWorld.Particles.InstantiateParticles(collisionParticles, 
        transform.position, Quaternion.identity, 1, transform);
}
