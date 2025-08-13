using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private BlockSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<BlockSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
