using System.Collections;
using UnityEngine;

public class BulletImpactVfx : MonoBehaviour, IObjectItemPoolable
{
    [SerializeField]
    private ParticleSystem particles;

    private Coroutine autoReturnRoutine;

    public void OnDespawn()
    {
        if (autoReturnRoutine != null)
        {
            StopCoroutine(autoReturnRoutine);
            autoReturnRoutine = null;
        }

        if (particles != null)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void OnSpawn()
    {
        if (particles != null)
        {
            particles.Clear();
            particles.Play();
        }

        // Auto return after 1 second
        autoReturnRoutine = StartCoroutine(AutoReturnAfterSeconds(1f));
    }

    private IEnumerator AutoReturnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        PoolManager.Instance.Return(this);
    }
}
