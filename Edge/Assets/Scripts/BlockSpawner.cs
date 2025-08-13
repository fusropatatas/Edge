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

    private bool spawnLeft = true;

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
        int nextBlock = currentBlock > 18 ? 1 : currentBlock + 3;
        blockPrefabs[currentBlock + 3].transform.localScale = blockPrefabs[currentBlock].transform.localScale;

        lastBlock = currentBlock + 2 > poolSize - 1 ? 0 : currentBlock;
        currentBlock = currentBlock + 2 > poolSize - 1 ? 1 : currentBlock + 2;
        hangingBlock = hangingBlock + 2 > poolSize - 2 ? 2 : hangingBlock + 2;
        // spawnLeft = spawnLeft ? false : true;

        if(spawnLeft)
        {
            blockPrefabs[currentBlock].transform.position = leftSpawn.transform.position + new Vector3(0f, 0.2f, 0f);
            leftSpawn.transform.position += new Vector3(0f, 0.2f, 0f);
            rightSpawn.transform.position += new Vector3(0f, 0.2f, 0f);
            blockPrefabs[currentBlock].transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            blockPrefabs[currentBlock].transform.position = rightSpawn.transform.position + new Vector3(0f, 0.2f, 0f);
            leftSpawn.transform.position += new Vector3(0f, 0.2f, 0f);
            rightSpawn.transform.position += new Vector3(0f, 0.2f, 0f);
            blockPrefabs[currentBlock].transform.rotation = Quaternion.Euler(0, 270, 0);
        }

        blockPrefabs[currentBlock].moving = true;
        blockPrefabs[lastBlock].moving = false;
        blockPrefabs[hangingBlock].moving = false;
        
        blockPrefabs[currentBlock].lastBlock = blockPrefabs[lastBlock];
        blockPrefabs[currentBlock].hangingBlock = blockPrefabs[hangingBlock];
    }
}
