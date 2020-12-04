using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    protected bool _isActivated = false;
    protected ObjectBasePool _instancePool;
    protected Coroutine _delayDespawn = null;

    public virtual bool IsActivated
    {
        get
        {
            return _isActivated;
        }
    }

    public virtual bool IsDeactivated
    {
        get
        {
            return _isActivated == false;
        }
    }

    protected virtual void Awake()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    public virtual void OnSpawn(ObjectBasePool instancePool)
    {
        _instancePool = instancePool;
        _isActivated = true;
    }

    public virtual void Despawn(float delaySecond)
    {
        if (_delayDespawn != null)
        {
            StopCoroutine(_delayDespawn);
            _delayDespawn = null;
        }

        _delayDespawn = StartCoroutine(_DelayDespawn(delaySecond));
    }

    protected virtual IEnumerator _DelayDespawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (IsDeactivated)
        {
            yield break;
        }

        _delayDespawn = null;
        Despawn();
    }

    public virtual void Despawn()
    {
        if (IsDeactivated)
        {
            return;
        }


        if (_delayDespawn != null)
        {
            StopCoroutine(_delayDespawn);
            _delayDespawn = null;
        }

        if (_instancePool != null)
        {
            _instancePool.Despawn(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void OnDespawn()
    {
        _isActivated = false;
    }
}
