using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabirynthSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    public List<LabirynthCell> spawnQueue { get; private set; } = new List<LabirynthCell>();
    public List<LabirynthCell> updateQueue { get; private set; } = new List<LabirynthCell>();

    public IntVector2 gridSize {get; protected set; }

    GameObject[,] grid;

    // Start is called before the first frame update
    virtual public void Start()
    {
        grid = new GameObject[gridSize.x, gridSize.y];
        Debug.Log("tworzenie grid");
    }

    // Update is called once per frame
    virtual public void Update()
    {

        if (spawnQueue.Count > 0)
        {
            LabirynthCell cellToSpawn = spawnQueue[0];

            spawnQueue.RemoveAt(0);

           // Debug.Log("spawning: " + cellToSpawn.cellType.ToString());
            GameObject cell = (GameObject)Instantiate(prefab, new Vector3(cellToSpawn.position.x, cellToSpawn.position.y, 0), Quaternion.Euler(0, 0, 0));
            cell.GetComponentInChildren<TestingCellObjectScript>().SetEnable(true);
            grid[cellToSpawn.position.x, cellToSpawn.position.y] = cell;
        }

        if(updateQueue.Count > 0)
        {
            LabirynthCell cellToUpdate = updateQueue[0];
            if (!grid[cellToUpdate.position.x, cellToUpdate.position.y]) return;
            updateQueue.RemoveAt(0);

            Debug.Log("updating: " + cellToUpdate.cellType.ToString() + " into: " + cellToUpdate.cellType.ToString());
            Debug.Log(grid[cellToUpdate.position.x, cellToUpdate.position.y].GetComponentInChildren<TestingCellObjectScript>());
            grid[cellToUpdate.position.x, cellToUpdate.position.y].GetComponentInChildren<TestingCellObjectScript>().SetEnable(false);
        }
    }

    public void SpawnLabirynthObject(MasterLabirynthCell cell)
    {
        spawnQueue.Add(cell);
        Debug.Log("adding to spawn queue");
    }

    public void UpdateLabirynthObject(MasterLabirynthCell cell)
    {
        updateQueue.Add(cell);
        Debug.Log("adding to update queue");
    }
}
