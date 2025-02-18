using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    const int maxRate = 9999;
    public float targetFrameRate = 60f;
    private float currentFrameTime;
    void Awake()
    {
        //Application.targetFrameRate = 10;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = maxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }
    private IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1f / targetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)sleepTime * 1000);
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}
