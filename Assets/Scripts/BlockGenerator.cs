using UnityEngine;

public enum BlockGenerationMethod
{
    None,
    Dynamic,
    ObjectPooling
}

public class BlockGenerator : MonoBehaviour
{

    [SerializeField] private BlockGenerationMethod _blockGenerationMethod;

    public void GenerateBlock(TileObjectType type)
    {

    }

}