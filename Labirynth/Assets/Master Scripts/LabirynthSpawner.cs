using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    virtual public bool Begin()
    {
        if (started) return false;
        started = true;
        grid = new GameObject[gridSize.x, gridSize.y];
        Debug.Log("tworzenie grid");
        return true;
    }

    private float spawnCooldown = 0;
    private bool spawnDone = false;

    public bool breakAfterSpawn = false;


    private float updateCooldown = 0;
    private bool updateDone = false;

    public bool breakAfterUpdate = false;

    protected bool started { get; private set; } = false;

    // Update is called once per frame
    virtual public void Update()
    {
        

        if (spawnQueue.Count > 0)
        {
            LabirynthCell cellToSpawn = spawnQueue[0];

            spawnQueue.RemoveAt(0);

           // Debug.Log("spawning: " + cellToSpawn.cellType.ToString());
            GameObject cell = (GameObject)Instantiate(prefab, new Vector3(cellToSpawn.position.x, cellToSpawn.position.y, 0), Quaternion.Euler(0, 0, 0));
            //cell.GetComponentInChildren<CellObject>().SetCellType(CellObject.CELL_TYPE.WALL);

            
            grid[cellToSpawn.position.x, cellToSpawn.position.y] = cell;

            spawnCooldown = 0.2f;
            spawnDone = true;
            
        }


        
        if(spawnDone)
        {
            

            spawnCooldown -= Time.deltaTime;
            if(spawnCooldown <= 0)
            {
                spawnDone = false;
                breakAfterSpawn = true;
            }
        }

        

       
        

        if(updateQueue.Count > 0)
        {
            LabirynthCell cellToUpdate = updateQueue[0];
            if (!grid[cellToUpdate.position.x, cellToUpdate.position.y]) return;
            updateQueue.RemoveAt(0);

            Debug.Log("updating: " + cellToUpdate.cellType.ToString() + " into: " + cellToUpdate.cellType.ToString());
            
            //grid[cellToUpdate.position.x, cellToUpdate.position.y].GetComponentInChildren<CellObject>().SetCellType(CellObject.CELL_TYPE.PATH);
            grid[cellToUpdate.position.x, cellToUpdate.position.y].SetActive(false);

            updateCooldown = 0.2f;
            updateDone = true;
        }

        if (updateDone)
        {
            updateCooldown -= Time.deltaTime;
            if (updateCooldown <= 0)
            {
                updateDone = false;
                breakAfterUpdate = true;
            }
        }

        
    }

    public void SpawnLabirynthObject(MasterLabirynthCell cell)
    {
        spawnQueue.Add(cell);
        //Debug.Log("adding to spawn queue");
    }

    public void UpdateLabirynthObject(MasterLabirynthCell cell)
    {
        updateQueue.Add(cell);
        //Debug.Log("adding to update queue");
    }

    
}
