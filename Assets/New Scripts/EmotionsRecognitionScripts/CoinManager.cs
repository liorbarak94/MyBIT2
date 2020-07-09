using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject animatedCoinPrefab;
    public Transform target;
    public int maxCoins;
    public Ease easeType;
    private Queue<GameObject> coinsQueue;

    // Animation Settings
    [Range(0.5f, 0.9f)] public float minAnimDuration;
    [Range(0.9f, 2f)] public float maxAnimDuration;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        coinsQueue = new Queue<GameObject>();
        targetPosition = target.position;
        PrepareCoins();
    }

    private void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.SetParent(transform);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    private void Animate(Vector3 collectedCoinPosition)
    {
        Debug.Log("here");
        if (coinsQueue.Count > 0)
        {
            GameObject coin = coinsQueue.Dequeue();
            coin.SetActive(true);

            // Move coin to the piggy
            coin.transform.position = collectedCoinPosition;

            // Animate coin to the piggy possition
            float duration = Random.Range(minAnimDuration, maxAnimDuration);
            coin.transform.DOMove(targetPosition, duration).SetEase(easeType).OnComplete(()=>
            {
                Debug.Log("here");
                coin.SetActive(false);
                coinsQueue.Enqueue(coin);
            });
        }
    }

    public void AddCoin(Vector3 collectedCoinPosition)
    {
        Debug.Log("collectedCoinPosition: " + collectedCoinPosition);

        Animate(collectedCoinPosition);
    }
}
