using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] public BlockMovement block;

    [SerializeField] public float blockSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            Debug.Log("SpacePressed");
            StopBlock();
        }
        else
        {
            gameObject.transform.position += new Vector3(0f, 0f, transform.forward.z * blockSpeed) * Time.deltaTime;
        }
    }

    public void StopBlock()
    {
        blockSpeed = 0f;
    }
}
