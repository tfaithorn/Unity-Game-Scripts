using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a pool of items that are instantiated in 1 batch, and then re-used.
/// This is more efficient than calling Instantiate constantly
/// </summary>
public class PoolingHelper : MonoBehaviour
{
    private List<GameObject> itemPool;
    private List<PoolItem> itemsInUse;
    public bool reuseWhenEmpty = true;
    public int poolLimit;
    private GameObject originalItem;
    private GameObject poolParent;

    private class PoolItem {
        public GameObject poolItem;
        public bool allowReuse;

        public PoolItem(GameObject poolItem)
        {
            this.poolItem = poolItem;
            this.allowReuse = true;
        }
    }

    public void Initialize(GameObject poolItem, int poolLimit, string gbName = null)
    {
        this.originalItem = poolItem;
        this.poolLimit = poolLimit;
        this.itemPool = new List<GameObject>();
        this.itemsInUse = new List<PoolItem>();

        if (gbName != null)
        {
            this.poolParent = new GameObject(gbName);
        }
        else {
            this.poolParent = new GameObject();
        }

        PopulatePool();
    }

    private void PopulatePool()
    {
        for (int i = 0; i < poolLimit; i++)
        {
            GameObject newItem = Instantiate(originalItem, Vector3.zero, Quaternion.identity);
            newItem.transform.SetParent(poolParent.transform,false);
            newItem.SetActive(false);
            itemPool.Add(newItem);
        }
    }

    public GameObject RequestItem()
    {
        GameObject item;

        //re-use item if out
        if (itemPool.Count == 0)
        {
            if (reuseWhenEmpty && itemsInUse.Exists(x => x.allowReuse == true))
            {
                for (int i = 0; i < itemsInUse.Count; i++)
                {
                    if (itemsInUse[i].allowReuse)
                    {
                        item = itemsInUse[i].poolItem;
                        itemsInUse.Remove(itemsInUse.Find(x => x.poolItem == item));
                        itemsInUse.Add(new PoolItem(item));
                        return item;
                    }
                }
            }
            else {
                //instantiate if out of items
                item = Instantiate(originalItem, Vector3.zero, Quaternion.identity);
                itemsInUse.Add(new PoolItem(item));
                return item;

            }
        }

        //take item from pool
        item = itemPool[0];
        itemsInUse.Add(new PoolItem(item));
        itemPool.RemoveAt(0);

        return item;

    }

    public void ReturnItem(GameObject item)
    {
        if (!itemPool.Contains(item))
        {
            item.SetActive(false);
            itemsInUse.Remove(itemsInUse.Find(x => x.poolItem == item));
            itemPool.Add(item);
            item.transform.SetParent(poolParent.transform, false);
        }
    }

    public void ReturnToPoolAfter(GameObject item, float duration)
    {
        itemsInUse.Find(x => x.poolItem == item).allowReuse = false;
        StartCoroutine(returnTimer(item,duration));
    }

    private IEnumerator returnTimer(GameObject item, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnItem(item);
    }

}
