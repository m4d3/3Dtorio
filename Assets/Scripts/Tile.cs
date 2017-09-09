using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private inputManager _manager;

    // Use this for initialization
    void Start()
    {
        _manager = FindObjectOfType<inputManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        _manager.tilePosition = transform.position;
    }


}
