using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    [SerializeField] public BlockMovement lastBlock;
    [SerializeField] public BlockMovement hangingBlock;
    
    [SerializeField] public BlockSpawner spawner;

    [SerializeField] public float blockSpeed = 2.5f;
    [SerializeField] public bool moving = false;

    void Start()
    {
        spawner = FindObjectOfType<BlockSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            MoveBlock();
        }
    }

    public void MoveBlock()
    {
        // Debug.Log(this.name);
        if(spawner.spawnLeft)
        {
            gameObject.transform.position += new Vector3(0f, 0f, transform.forward.z * blockSpeed) * Time.deltaTime;
            
            // Debug.Log("moving from the left");
        }
        else
        {
            gameObject.transform.position += new Vector3(transform.forward.x * blockSpeed, 0f, 0f) * Time.deltaTime;
            
            // Debug.Log("moving from the right");
        }

    }

    public void DropBlock()
    {
        Debug.Log("Dropping Block");
        blockSpeed = 0f;    // Stop the block's movement

        float hangingLength;
        float direction;

        // Get the length of the hanging part of the new block
        if(spawner.spawnLeft)
        {
            hangingLength = transform.position.z - lastBlock.transform.position.z;
            direction = hangingLength > 0 ? 1f : -1f;
        }
        else
        {
            hangingLength = transform.position.x - lastBlock.transform.position.x;
            direction = hangingLength > 0 ? 1f : -1f;
        }

        SliceCube(hangingLength, direction);
    }

    public void SliceCube(float hangingLength, float direction)
    {
        if(spawner.spawnLeft) // When block spawns from the top left, this is how to slice it
        {
            float remainingZLength = transform.localScale.z - Mathf.Abs(hangingLength);
            float slicedZLength = transform.localScale.z - remainingZLength;

            float remainingZPosition = lastBlock.transform.position.z + (hangingLength / 2f);

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, remainingZLength);
            transform.position = new Vector3(transform.position.x, transform.position.y, remainingZPosition);

            float blockEdge = transform.position.z + (direction * remainingZLength / 2f);
            float slicedZPosition =  blockEdge + (slicedZLength / 2f * direction);

            hangingBlock.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, slicedZLength);
            hangingBlock.transform.position = new Vector3(transform.position.x, transform.position.y, slicedZPosition);

            hangingBlock.gameObject.AddComponent<Rigidbody>();
        }
        else // When block spawns from the top right, this is how to slice it
        {
            float remainingXLength = transform.localScale.x - Mathf.Abs(hangingLength);
            float slicedXLength = transform.localScale.x - remainingXLength;

            float remainingXPosition = lastBlock.transform.position.x + (hangingLength / 2f);

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, remainingXLength);
            transform.position = new Vector3(remainingXPosition, transform.position.y, transform.position.z);

            float blockEdge = transform.position.x + (direction * remainingXLength / 2f);
            float slicedXPosition =  blockEdge + (slicedXLength / 2f * direction);

            hangingBlock.transform.localScale = new Vector3(slicedXLength, transform.localScale.y, transform.localScale.z);
            hangingBlock.transform.position = new Vector3(slicedXPosition, transform.position.y, transform.position.z);

            hangingBlock.gameObject.AddComponent<Rigidbody>();
        }
    }
}
