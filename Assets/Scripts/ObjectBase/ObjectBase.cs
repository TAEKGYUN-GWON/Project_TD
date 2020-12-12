using System.Collections;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    protected bool _isActivated = false;
    protected ObjectBasePool InstancePool;
    protected Coroutine DelayDespawn = null;

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
        InstancePool = instancePool;
        _isActivated = true;
    }

    public virtual void Despawn(float delaySecond)
    {
        if (DelayDespawn != null)
        {
            StopCoroutine(DelayDespawn);
            DelayDespawn = null;
        }

        DelayDespawn = StartCoroutine(_DelayDespawn(delaySecond));
    }

    protected virtual IEnumerator _DelayDespawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (IsDeactivated)
        {
            yield break;
        }

        DelayDespawn = null;
        Despawn();
    }

    public virtual void Despawn()
    {
        if (IsDeactivated)
        {
            return;
        }


        if (DelayDespawn != null)
        {
            StopCoroutine(DelayDespawn);
            DelayDespawn = null;
        }

        if (InstancePool != null)
        {
            InstancePool.Despawn(this);
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
