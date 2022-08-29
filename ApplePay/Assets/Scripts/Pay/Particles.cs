using UnityEngine;

namespace PayWorld
{
    public static class Particles
    {
        public static void InstantiateParticles(GameObject particles, Vector2 position, Quaternion rotation,
            float timeToDestroyInSeconds)
        {
            if(particles == null) return;
            GameObject temp = MonoBehaviour.Instantiate(particles, position, rotation);
            MonoBehaviour.Destroy(temp, timeToDestroyInSeconds);
        }

        public static void InstantiateParticles(GameObject particles, Vector2 position, Quaternion rotation,
            float timeToDestroyInSeconds, Transform parent)
        {
            if(particles == null) return;
            GameObject temp = MonoBehaviour.Instantiate(particles, position, rotation, parent);
            MonoBehaviour.Destroy(temp, timeToDestroyInSeconds);
        }
    }
}
