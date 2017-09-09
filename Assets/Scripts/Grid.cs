using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int Rows = 5;
    public int Columns = 5;
    public GameObject Tile;

    private void Awake()
    {

    }

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j ++)
            {
                GameObject go = Instantiate(Tile, new Vector3(i, 0, j), Quaternion.identity, this.transform);
            }
        }
    }
}



