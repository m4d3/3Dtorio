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
	    GetComponent<BoxCollider>().size = VectorFromDir() + (Vector3.one - VectorFromDir()) * 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
		
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

    Vector3 VectorFromDir()
    {
        Vector3 dir = Vector3.up;

        switch (BuildingDirection)
        {
            case Direction.Up:
                dir = this.transform.forward;
                break;
            case Direction.Right:
                dir = this.transform.right;
                break;
            case Direction.Down:
                dir = this.transform.forward * -1;
                break;
            case Direction.Left:
                dir = this.transform.right * -1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return dir;
    }
}
