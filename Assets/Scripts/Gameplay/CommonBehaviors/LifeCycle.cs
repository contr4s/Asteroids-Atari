using System.Collections;
using UnityEngine;

public class LifeCycle: MonoBehaviour
{
    [SerializeField] private float lifeTime = 2;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _coroutine = StartCoroutine(StartLifeCycle());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    private IEnumerator StartLifeCycle()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
