using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPool;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private float _shootingSpeed = 1f;
    private Queue<GameObject> pool = new Queue<GameObject>();
    private Coroutine _shootCoroutine;

    private void Awake()
    {
        for (int i = 0; i < 300; i++)
        {
            var obj = Instantiate(objectPool);
            pool.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    public void RetunToPool(GameObject item)
    {
        item.SetActive(false);
        pool.Enqueue(item);
    }
    public void GetFromPool()
    {
        if (_shootCoroutine == null)
        {
            _shootCoroutine = StartCoroutine(Shoot());
        }
    }
    private IEnumerator Shoot()
    {
        GameObject newBullet;
        if (pool.Count > 1)
        {
            for (int i = 0; i < spawnPosition.Length; i++)
            {
                newBullet = pool.Dequeue();
                newBullet.transform.position = spawnPosition[i].position;
                newBullet.SetActive(true);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.parent.forward * 3f + new Vector3(0, 0.4f, 0), ForceMode.Impulse);
            }
        }
        else
        {
            for (int i = 0; i < spawnPosition.Length; i++)
            {
                var instantiatedBullet = Instantiate(objectPool);
                instantiatedBullet.transform.position = spawnPosition[i].position;
                pool.Enqueue(instantiatedBullet);
            }
        }
        yield return new WaitForSeconds(_shootingSpeed);
        _shootCoroutine = null;
        yield break;
    }
}
