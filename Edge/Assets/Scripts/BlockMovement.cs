using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    [SerializeField] public BlockMovement lastBlock;
    [SerializeField] public BlockMovement hangingBlock;

    [SerializeField] public float blockSpeed = 1.5f;
    [SerializeField] public bool moving = false;

    void Start()
    {
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
        gameObject.transform.position += new Vector3(0f, 0f, transform.forward.z * blockSpeed) * Time.deltaTime;
    }

    public void DropBlock()
    {
        Debug.Log("Dropping Block");
        blockSpeed = 0f;    // Stop the block's movement

        // Get the length of the hanging part of the new block
        float hangingZLength = transform.position.z - lastBlock.transform.position.z;
        float direction = hangingZLength > 0 ? 1f : -1f;

        SliceCube(hangingZLength, direction);
    }

    public void SliceCube(float hangingZLength, float direction)
    {
        float remainingZLength = transform.localScale.z - Mathf.Abs(hangingZLength);
        float slicedZLength = transform.localScale.z - remainingZLength;

        float remainingZPosition = lastBlock.transform.position.z + (hangingZLength / 2f);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, remainingZLength);
        transform.position = new Vector3(transform.position.x, transform.position.y, remainingZPosition);

        float blockEdge = transform.position.z + (direction * remainingZLength / 2f);
        float slicedZPosition =  blockEdge + (slicedZLength / 2f * direction);

        hangingBlock.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, slicedZLength);
        hangingBlock.transform.position = new Vector3(transform.position.x, transform.position.y, slicedZPosition);

        hangingBlock.gameObject.AddComponent<Rigidbody>();
        // Debug.Log(hangingZLength);
    }
}
