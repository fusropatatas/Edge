using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPool;
    [SerializeField] public BlockMovement[] blockPrefabs;
    private int poolSize;

    [SerializeField] private GameObject leftSpawn;
    [SerializeField] private GameObject rightSpawn;

    [SerializeField] public int currentBlock;
    [SerializeField] public int lastBlock;
    [SerializeField] public int hangingBlock;

    public bool spawnLeft = true;

    private void Start()
    {
        blockPrefabs = blockPool.gameObject.GetComponentsInChildren<BlockMovement>();
        poolSize = blockPrefabs.Length;

        // int index = 0;
        // foreach(Transform trans in blockTransforms)
        // {
        //     index++;
        //     blockPrefabs.SetValue(trans.BlockMovement, index-1);
        // }

        lastBlock = 0;
        currentBlock = 1;
        hangingBlock = 2;

        blockPrefabs[currentBlock].moving = true;
        blockPrefabs[lastBlock].moving = false;
        blockPrefabs[hangingBlock].moving = false;

        blockPrefabs[currentBlock].lastBlock = blockPrefabs[lastBlock];
        blockPrefabs[currentBlock].hangingBlock = blockPrefabs[hangingBlock];

        blockPrefabs[lastBlock].transform.position = new Vector3(0f,0f,0f);
        blockPrefabs[currentBlock].transform.position = leftSpawn.transform.position;
    }

    public void SpawnBlock()
    {
        int nextBlock = (currentBlock + 2) % poolSize;

        blockPrefabs[nextBlock].transform.localScale = new Vector3(
            blockPrefabs[currentBlock].transform.localScale.z,
            blockPrefabs[currentBlock].transform.localScale.y,
            blockPrefabs[currentBlock].transform.localScale.x);

        int prevCurrent = currentBlock;                 // remember who was current
        lastBlock = prevCurrent;                        // prevCurrent becomes "last"
        currentBlock = nextBlock;                       // advance to the wrapped "next" block index
        hangingBlock = (hangingBlock + 2) % poolSize;   // advance hanging safely

        if(spawnLeft) // Spawn the new block from the top left spawn point
        {
            blockPrefabs[currentBlock].transform.position = leftSpawn.transform.position + new Vector3(0f, 0.2f, 0f);
            rightSpawn.transform.position = new Vector3(blockPrefabs[lastBlock].transform.position.x + 3.0f, blockPrefabs[lastBlock].transform.position.y + 0.2f, blockPrefabs[lastBlock].transform.position.z);
            blockPrefabs[currentBlock].transform.rotation = Quaternion.Euler(0, 180, 0);

            // Debug.Log("Spawning Left");
        }
        else // Spawn the new block from the top right spawn point
        {
            blockPrefabs[currentBlock].transform.position = rightSpawn.transform.position + new Vector3(0f, 0.2f, 0f);
            blockPrefabs[currentBlock].transform.rotation = Quaternion.Euler(0, 270, 0); // Reorients the forward z-axis (blue line) to move the object correctly

            // Debug.Log("Spawning Right");
        }

        blockPrefabs[currentBlock].moving = true;
        blockPrefabs[lastBlock].moving = false;
        blockPrefabs[hangingBlock].moving = false;
        
        blockPrefabs[currentBlock].lastBlock = blockPrefabs[lastBlock];
        blockPrefabs[currentBlock].hangingBlock = blockPrefabs[hangingBlock];
    }

    public void SpawnerUpdate()
    {
        rightSpawn.transform.position = new Vector3(blockPrefabs[currentBlock].transform.position.x + 3.0f, blockPrefabs[currentBlock].transform.position.y, blockPrefabs[currentBlock].transform.position.z);
        leftSpawn.transform.position = new Vector3(blockPrefabs[currentBlock].transform.position.x, blockPrefabs[currentBlock].transform.position.y, blockPrefabs[currentBlock].transform.position.z + 3.0f);
        // Debug.Log("Updating Spawners");
    }
}
