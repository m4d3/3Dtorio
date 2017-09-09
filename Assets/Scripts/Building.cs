using System.Collections;
using System.Collections.Generic;
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
}
