using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseBelt : Building {

    
    public float BeltSpeed = 2;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Build()
    {
        GetComponent<BoxCollider>().size = VectorFromDir() + (Vector3.one - VectorFromDir()) * 0.01f;
    }

    private void OnTriggerStay(Collider other)
    {
        BaseProduct product = other.GetComponent<BaseProduct>();

        if (!product || !this.enabled) return;
        
        if (product.OnBelt == null && !product.OnInserter)
        {
            product.OnBelt = this;
        } 
        MoveProduct(other.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<BaseProduct>() || !this.enabled) return;

        other.GetComponent<BaseProduct>().OnBelt = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseProduct>())
        {
            //MoveProduct(other.transform);
            other.GetComponent<BaseProduct>().OnBelt = null;
        }
    }

    public void MoveProduct(Transform product)
    {
        if (!Physics.Raycast(product.position, VectorFromDir(), BeltSpeed * Time.deltaTime * 6, 8)
            && product.GetComponent<BaseProduct>().OnBelt == this)
        {
            product.Translate(VectorFromDir() * BeltSpeed * Time.deltaTime);
        }
    }

    
}
