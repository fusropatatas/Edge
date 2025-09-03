using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BlockSpawner spawner;
    [SerializeField] private GameObject camera;

    void Start()
    {
        spawner = FindObjectOfType<BlockSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))   // If player presses Space, stops the object and drops it
        {
            if(spawner.blockPrefabs[spawner.currentBlock] != null)
            {
                spawner.blockPrefabs[spawner.currentBlock].DropBlock();
                spawner.SpawnerUpdate();
                camera.transform.position += new Vector3(0f, 0.2f, 0f);

                spawner.spawnLeft = spawner.spawnLeft ? false : true;
            }

            spawner.SpawnBlock();
        }
    }
}
