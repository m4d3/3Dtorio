using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Building : MonoBehaviour {

    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public Direction BuildingDirection = Direction.Up;

    private inputManager _manager;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		if(!_manager)
		    _manager = FindObjectOfType<inputManager>();

    }

    void OnMouseEnter()
    {
        if(_manager)
            _manager.OverBuilding = this;
    }

    void OnMouseExit()
    {
        if(_manager)
            _manager.OverBuilding = null;
    }

    protected Vector3 VectorFromDir()
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

    public virtual void Build()
    {
        
    }
}
