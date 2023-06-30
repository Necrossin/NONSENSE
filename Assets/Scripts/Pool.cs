using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    struct PoolObjectInfo
    {
        public int iCapacity;
        public GameObject PoolCategory;
    }

    public static Pool Instance;

    Object[] AssetsToLoad;

    Dictionary<GameObject, Queue<GameObject>> lstPools = new Dictionary<GameObject, Queue<GameObject>>();
    Dictionary<GameObject, List<GameObject>> lstActivePools = new Dictionary<GameObject, List<GameObject>>();
    Dictionary<GameObject, GameObject> lstActivePoolsAll = new Dictionary<GameObject, GameObject>();
    Dictionary<GameObject, PoolObjectInfo> lstPoolsInfo = new Dictionary<GameObject, PoolObjectInfo>();

    private void Awake()
    {
        Instance = this;

        CreatePoolQueues();
    }

    void CreatePoolQueues()
    {
        AssetsToLoad = Resources.LoadAll("", typeof(PoolObjectTemplate));

        foreach (var t in AssetsToLoad)
        {
            var PoolEntry = (PoolObjectTemplate)t;
            var PoolPrefab = PoolEntry.Prefab;

            // Create new dictionary entry if there is none

            if (!lstPoolsInfo.ContainsKey(PoolPrefab))
            {
                var Category = BlankCategory(t.name + " Pool");

                var EntryInfo = new PoolObjectInfo();
                EntryInfo.iCapacity = PoolEntry.Capacity;
                EntryInfo.PoolCategory = Category;

                lstPoolsInfo[PoolPrefab] = EntryInfo;
            }

            if (!lstPools.ContainsKey(PoolPrefab))
            {
                lstPools[PoolPrefab] = new Queue<GameObject>();

                for (int i = 0; i < lstPoolsInfo[PoolPrefab].iCapacity; i++)
                {
                    var Entry = Instantiate(PoolPrefab, Vector3.zero, Quaternion.identity);
                    if (Entry != null)
                    {
                        Entry.SetActive(false);
                        Entry.transform.SetParent(lstPoolsInfo[PoolPrefab].PoolCategory.transform);

                        lstPools[PoolPrefab].Enqueue(Entry);
                    }
                }
            }

            if (!lstActivePools.ContainsKey(PoolPrefab))
                lstActivePools[PoolPrefab] = new List<GameObject>();
        }
    }

    public GameObject InstantiateFromPool( GameObject Prefab, Vector3 vPos, Quaternion qRot, bool bManualEnable = false )
    {
        if (!lstPools.ContainsKey(Prefab))
        {
            Debug.LogWarning("GameObject " + Prefab.name + " does not exist in the pool!");
            return null;
        }
            

        if (!lstPoolsInfo.ContainsKey(Prefab))
            return null;

        //If object pool is empty - reuse the oldest active one
        if (lstPools[Prefab].Count < 1)
        {
            var LastActiveObject = lstActivePools[Prefab][0];
            if (LastActiveObject != null)
                ReturnToPool(LastActiveObject);
            else
                return null;
        }


        var Entry = lstPools[Prefab].Dequeue();
        if (Entry != null)
        {
            Entry.transform.SetParent(null);

            Entry.transform.position = vPos;
            Entry.transform.rotation = qRot;

            if (!bManualEnable)
                Entry.SetActive(true);

            lstActivePoolsAll.Add(Entry, Prefab);
            lstActivePools[Prefab].Add(Entry);

            return Entry;
        }

        return null;
    }

    public void ReturnToPool( GameObject Obj )
    {
        var Prefab = GetPoolPrefabFromObject(Obj);
        if (Prefab == null)
            return;

        var objList = lstPools[Prefab];
        var objListInfo = lstPoolsInfo[Prefab];

        if (objList == null)
            return;

        Obj.SetActive(false);

        Obj.transform.position = Vector3.zero;
        Obj.transform.rotation = Quaternion.identity;

        Obj.transform.localPosition = Vector3.zero;
        Obj.transform.localRotation = Quaternion.identity;

        Obj.transform.SetParent(objListInfo.PoolCategory.transform, true);

        lstPools[Prefab].Enqueue(Obj);
        lstActivePoolsAll.Remove(Obj);
        lstActivePools[Prefab].Remove(Obj);
    }

    public GameObject GetPoolPrefabFromObject( GameObject Obj )
    {
        if (lstActivePoolsAll.ContainsKey(Obj))
            return lstActivePoolsAll[Obj];

        return null;
    }

    GameObject BlankCategory(string name = "")
    {
        var Empty = new GameObject(name);

        Empty.transform.parent = gameObject.transform;
        Empty.transform.localPosition = Vector3.zero;
        Empty.transform.rotation = Quaternion.identity;

        Empty.layer = gameObject.layer;

        return Empty;
    }
}
