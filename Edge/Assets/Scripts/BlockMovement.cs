using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    [SerializeField] public BlockMovement lastBlock;
    [SerializeField] public BlockMovement hangingBlock;
    
    [SerializeField] public BlockSpawner spawner;

    [SerializeField] public float blockSpeed;
    [SerializeField] public bool moving = false;
    Rigidbody rb;

    void Start()
    {
        spawner = FindObjectOfType<BlockSpawner>();
        blockSpeed = 3f;
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
        moving = false;    // Stop the block's movement

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

// Legacy slicing code before refactor
//     public void SliceCube(float hangingLength, float direction)
//     {
//         if(spawner.spawnLeft) // When block spawns from the top left, this is how to slice it
//         {
//             float remainingZLength = transform.localScale.z - Mathf.Abs(hangingLength);
//             float slicedZLength = transform.localScale.z - remainingZLength;

//             float remainingZPosition = lastBlock.transform.position.z + (hangingLength / 2f);

//             transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, remainingZLength);
//             transform.position = new Vector3(transform.position.x, transform.position.y, remainingZPosition);

//             float blockEdge = transform.position.z + (direction * remainingZLength / 2f);
//             float slicedZPosition =  blockEdge + (slicedZLength / 2f * direction);

//             hangingBlock.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, slicedZLength);
//             hangingBlock.transform.position = new Vector3(transform.position.x, transform.position.y, slicedZPosition);

//             hangingBlock.gameObject.AddComponent<Rigidbody>();
//         }
//         else // When block spawns from the top right, this is how to slice it
//         {
//             float remainingXLength = transform.localScale.x - Mathf.Abs(hangingLength);
//             float slicedXLength = transform.localScale.x - remainingXLength;

//             float remainingXPosition = lastBlock.transform.position.x + (hangingLength / 2f);

//             transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, remainingXLength);
//             transform.position = new Vector3(remainingXPosition, transform.position.y, transform.position.z);

//             float blockEdge = transform.position.x + (direction * remainingXLength / 2f);
//             float slicedXPosition =  blockEdge + (slicedXLength / 2f * direction);

//             hangingBlock.transform.localScale = new Vector3(slicedXLength, transform.localScale.y, transform.localScale.z);
//             hangingBlock.transform.position = new Vector3(slicedXPosition, transform.position.y, transform.position.z);

//             hangingBlock.gameObject.AddComponent<Rigidbody>();
//         }
//     }
// }

    public void SliceCube(float hangingLength, float direction)
    {
        // World slice axis: Z when spawning from the left, X when spawning from the right
        Vector3 axisW = spawner.spawnLeft ? Vector3.forward : Vector3.right;

        // Figure out which LOCAL axis (x or z) corresponds to the world slice axis.
        // If the block was rotated 90° for X-runs, local Z will line up with world X.
        int axisIndex; // 0 = local X, 2 = local Z
        float xDot = Mathf.Abs(Vector3.Dot(transform.right, axisW));
        float zDot = Mathf.Abs(Vector3.Dot(transform.forward, axisW));
        axisIndex = (xDot >= zDot) ? 0 : 2;

        Vector3 originalScale = transform.localScale;
        float originalLength = (axisIndex == 0) ? originalScale.x : originalScale.z;

        float remainingLength = originalLength - Mathf.Abs(hangingLength);
        float slicedLength    = originalLength - remainingLength;

        // Full miss: drop the whole thing
        if (remainingLength <= 0f)
        {
            hangingBlock.transform.localScale = originalScale;
            hangingBlock.transform.position   = transform.position;
            hangingBlock.transform.rotation   = transform.rotation;
            if (hangingBlock.GetComponent<Rigidbody>() == null)
                rb = hangingBlock.gameObject.AddComponent<Rigidbody>();
            return;
        }

        // Center of the overlapping (remaining) piece in WORLD along the slice axis
        Vector3 remainingCenter = lastBlock.transform.position + axisW * (hangingLength / 2f);
        remainingCenter.y = transform.position.y;

        // Lock the perpendicular horizontal axis so there’s no lateral drift
        if (Mathf.Abs(axisW.z) > 0.5f)  // slicing along world Z
            remainingCenter.x = lastBlock.transform.position.x;
        else                              // slicing along world X
            remainingCenter.z = lastBlock.transform.position.z;

        // Apply new scale to the main piece along the resolved LOCAL axis
        Vector3 newMainScale = originalScale;
        if (axisIndex == 0) newMainScale.x = remainingLength; else newMainScale.z = remainingLength;
        transform.localScale = newMainScale;
        transform.position   = remainingCenter;

        // Compute hanging piece center in WORLD
        Vector3 blockEdge    = transform.position + axisW * (direction * remainingLength / 2f);
        Vector3 slicedCenter = blockEdge + axisW * (direction * slicedLength / 2f);
        slicedCenter.y = transform.position.y;
        if (Mathf.Abs(axisW.z) > 0.5f) slicedCenter.x = transform.position.x; else slicedCenter.z = transform.position.z;

        // Configure the hanging piece — CRITICAL: rotate to match the moving block
        // and scale on the SAME LOCAL axis we used for the main piece.
        hangingBlock.transform.rotation = transform.rotation;

        Vector3 newHangScale = originalScale;
        if (axisIndex == 0) newHangScale.x = slicedLength; else newHangScale.z = slicedLength;
        hangingBlock.transform.localScale = newHangScale;

        hangingBlock.transform.position = slicedCenter;
        if (hangingBlock.GetComponent<Rigidbody>() == null)
            rb = hangingBlock.gameObject.AddComponent<Rigidbody>();
    }


}