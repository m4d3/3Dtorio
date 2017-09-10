using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProducer : Building
{
    private bool isProducing = true;
    private float spawnTimer = 0;
    private float productionTime = 1;
    private float productionSpeed = 1;
    private Vector3 spawnPosition;

    public GameObject Product;

    private GameObject lastProduct;

	// Use this for initialization
	void Start ()
	{
	    spawnPosition = transform.position + VectorFromDir() + Vector3.up * 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
	    if (isProducing)
	    {
	        if (spawnTimer >= productionTime)
	        {
	            SpawnProduct();
	            spawnTimer = 0;
	        }
	        else
	        {
	            spawnTimer += Time.deltaTime * productionSpeed;
	        }
	    }
	}

    void SpawnProduct()
    {
        if(lastProduct && lastProduct.transform.position == spawnPosition) return;

        lastProduct = Instantiate(Product, spawnPosition, Quaternion.identity);
    }
}
