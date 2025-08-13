using UnityEngine;

public class NewBlockMovement : MonoBehaviour
{
    public enum BlockState
    {
        Pooling,
        Spawning,
        Moving,
        Dropping,
        Despawning,

    }

    public BlockState CurrentBlockState {get; private set;}

    public void SetBlockState(BlockState blockState)
    {
        if(CurrentBlockState == blockState) return;

        switch(blockState)
        {
            case BlockState.Pooling:
                // HandlePooling();
                break;

            case BlockState.Spawning:
                // HandleSpawning();
                break;

            case BlockState.Moving:
                // HandleMoving();
                break;
            
            case BlockState.Dropping:
                // HandleDropping();
                break;

            case BlockState.Despawning:
                // HandleDespawning();
                break;

            CurrentBlockState = blockState;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
