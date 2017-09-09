using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    private bool isBuilding = false;
    private GameObject currentBuilding;
    public Vector3 tilePosition;
    private Building buildingScript;
    public Building OverBuilding;

    public GameObject[] Buildings;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000, 9))
        {
            if (hitInfo.transform.GetComponent<Tile>())
            {
                tilePosition = hitInfo.transform.position;
                Debug.Log("Hit");
            }
        }

        if (currentBuilding)
        {
            currentBuilding.transform.position = tilePosition + Vector3.up * 0.1f;

            if (Input.GetKeyDown(KeyCode.T))
            {
                currentBuilding.transform.Rotate(Vector3.up, 90);

                if (currentBuilding.GetComponent<Building>())
                {
                    Building building = currentBuilding.GetComponent<Building>();

                    if (building.BuildingDirection.Equals(3))
                        building.BuildingDirection = 0;
                    else
                        building.BuildingDirection += 1;

                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                currentBuilding = null;
                buildingScript.enabled = true;
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Building");
                currentBuilding = Instantiate(Buildings[0], tilePosition, Quaternion.identity);
                buildingScript = currentBuilding.GetComponent<BaseBelt>();
                buildingScript.enabled = false;

            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Building");
                currentBuilding = Instantiate(Buildings[1], tilePosition, Quaternion.identity);
                buildingScript = currentBuilding.GetComponent<BaseInserter>();
                buildingScript.enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.Z) && OverBuilding)
            {
                Destroy(OverBuilding);
            }
        }
    }
}
