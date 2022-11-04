using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Pay.Camera
{
    public static class CameraShake
    {
        private static IEnumerator HandleFlatShake(float delay, float force, float shakeTime)
        {
            CinemachineBasicMultiChannelPerlin cam = MonoBehaviour.FindObjectOfType<CinemachineBasicMultiChannelPerlin>();

            yield return new WaitForSecondsRealtime(delay);
            
            cam.m_AmplitudeGain = force;
            cam.m_FrequencyGain = force;
            
            yield return new WaitForSecondsRealtime(shakeTime);

            DisableShake(cam);
        }
        private static IEnumerator HandleSmoothShake(float delay, float beginForce, float endForce,
            float addingForceOverTime, float decreasingForceOverTime)
        {
            CinemachineBasicMultiChannelPerlin cam = MonoBehaviour.FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
            

            yield return new WaitForSecondsRealtime(delay);
            

            for (float i = beginForce; i < endForce; i+= Time.deltaTime * addingForceOverTime)
            {
                cam.m_AmplitudeGain = i; cam.m_FrequencyGain = i;
                yield return new WaitForEndOfFrame();
            }
            for (float i = endForce; i > 0; i -= Time.deltaTime * decreasingForceOverTime)
            {
                cam.m_AmplitudeGain = i; cam.m_FrequencyGain = i;
                yield return new WaitForEndOfFrame();
            }
            DisableShake(cam);
        }

        private static IEnumerator HandleChaoticShake(float delay, float minForce, float maxForce,
            float forcesInterval, float forceCount)
        {
            CinemachineBasicMultiChannelPerlin cam = MonoBehaviour.FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
            
            yield return new WaitForSecondsRealtime(delay);
        
            for (int i = 0; i < forceCount; i++)
            {
                float rand = Random.Range(minForce, maxForce);
                cam.m_AmplitudeGain = rand; cam.m_FrequencyGain = rand;
                for (float j = 0; j < forcesInterval; j += Time.deltaTime)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            DisableShake(cam);
        }
        private static void DisableShake(CinemachineBasicMultiChannelPerlin camera)
        {
            camera.m_AmplitudeGain = 0;
            camera.m_FrequencyGain = 0;
        }
        public static void FlatShake(float delay, float force, float shakeTime) => StaticCoroutine.BeginCoroutine(HandleFlatShake(delay, force, shakeTime));
        public static void ChaoticShake(float delay, float minForce, float maxForce, float forcesInterval, float forceCount) => StaticCoroutine.BeginCoroutine(HandleChaoticShake(delay, minForce, maxForce, forcesInterval, forceCount));
        public static void SmoothShake(float delay, float beginForce, float endForce, float addingForceOverTime, float decreasingForceOverTime) => StaticCoroutine.BeginCoroutine(HandleSmoothShake(delay, beginForce, endForce, addingForceOverTime, decreasingForceOverTime));
    }
}