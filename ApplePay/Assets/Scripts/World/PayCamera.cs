using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Pay.Camera
{
    public static class CameraShake
    {
        public static IEnumerator FlatShake(float delay, float force, float shakeTime)
        {
            CinemachineBasicMultiChannelPerlin cam = MonoBehaviour.FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
            float curTime = delay + shakeTime;

            while(curTime > 0)
            {
                curTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if(curTime > delay)
                {
                    cam.m_AmplitudeGain = force;
                    cam.m_FrequencyGain = force;
                }
            }
            DisableShake(cam);
        }
        public static IEnumerator SmoothShake(float delay, float beginForce, float endForce,
            float addingForceOverTime, float decreasingForceOverTime)
        {
            CinemachineBasicMultiChannelPerlin cam = MonoBehaviour.FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
            float curTime = 0;

            while(curTime < delay)
            {
                curTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

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

        public static IEnumerator ChaoticShake(float delay, float minForce, float maxForce,
            float forcesInterval, float forceCount)
        {
            CinemachineBasicMultiChannelPerlin cam = MonoBehaviour.FindObjectOfType<CinemachineBasicMultiChannelPerlin>();
            float curTime = 0;
            while(curTime < delay)
            {
                curTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        
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
        public static void DisableShake(CinemachineBasicMultiChannelPerlin camera)
        {
            camera.m_AmplitudeGain = 0;
            camera.m_FrequencyGain = 0;
        }
    }
}