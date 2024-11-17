
using UnityEngine;

public class GeneratorDynamic : TileObjectGenerator
{
    public override void Init()
    {

    }

    public override TileObject GetTileObject(TileObjectType type)
    {
        TileObject tileObject = Instantiate(_tileObjPrefabMap[type].GetComponent<TileObject>());
        tileObject.transform.localScale = new Vector3(GridManager.s_Instance.TileWidth * 0.8f, GridManager.s_Instance.TileHeight * 0.8f, 1);
        // SpriteRenderer spriteRenderer;
        // if(TryGetComponent(out spriteRenderer))
        // {
        //     Sprite sprite = spriteRenderer.sprite;

        //     if(sprite != null)
        //     {
        //         Vector2 spriteSize = sprite.bounds.size;

        //         Vector3 newScale = tileObject.transform.localScale;
        //         newScale.x = GridManager.s_Instance.TileWidth / spriteSize.x;
        //         newScale.y = GridManager.s_Instance.TileHeight / spriteSize.y;
                
        //         // Apply the new scale to the object
        //         tileObject.transform.localScale = newScale;
        //     }
        // }

        return tileObject;
    }

    public override void ReturnTileObject(TileObject tileObject)
    {
        Destroy(tileObject.gameObject);
    }
}