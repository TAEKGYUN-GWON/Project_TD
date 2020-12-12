using UnityEngine;

public class ObjectBasePool : ObjectPool<ObjectBase>
{
    public static ObjectBasePool CreateInstancePool(Transform parent, ObjectBase prefab, int defaultPoolSize, bool increasementable, int increasementSize)
    {
        var pool = new ObjectBasePool();
        pool.InitPool(parent, prefab, defaultPoolSize, increasementable, increasementSize);

        return pool;
    }

    public override ObjectBase Spawn()
    {
        var instance = base.Spawn();
        instance.OnSpawn(this);
        return instance;
    }

    public T Spawn<T>() where T : ObjectBase
    {
        var instance = Spawn();
        instance.OnSpawn(this);
        return instance as T;
    }
}