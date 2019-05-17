using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG;
using STP;
public class TileRoughRender : MonoBehaviour
{
    [SerializeField]
    private Vector2 tile_offset;
    private Vector2Int map_offset;

    [SerializeField]
    private TileSTPSet tileSet;

    [SerializeField]
    private GameObject tilePrefab;

    private BSP bspMap;

    string[,] mapData;

    private void Awake()
    {
        bspMap = this.GetComponent<BSP>();

        if (bspMap != null)
            bspMap.OnMapBuild += RenderTile;
    }

    public void RenderTile(Vector2Int dungeonSize, List<BSPMapComponent> bspComponents) {

        map_offset = new Vector2Int(Mathf.RoundToInt( -dungeonSize.x), Mathf.RoundToInt(-dungeonSize.y));

        var dungeonFullSize = new Vector2Int(dungeonSize.x * 2, dungeonSize.y * 2);
        //1 = wall , 0 = empty
        mapData = new string[dungeonFullSize.x, dungeonFullSize.y];

        FloodFillMap(dungeonFullSize);

        for (int i = 0; i < bspComponents.Count; i++) {

            for (float x = bspComponents[i].spaceRect.xMin; x < bspComponents[i].spaceRect.xMax; x++)
            {
                for (float y = bspComponents[i].spaceRect.yMin; y < bspComponents[i].spaceRect.yMax; y++)
                {
                    int xIndex = Mathf.RoundToInt(dungeonSize.x + x);
                    int yIndex = Mathf.RoundToInt(dungeonSize.y + y);
                    mapData[xIndex, yIndex] = "0";

                    if (bspComponents[i].GetType() == typeof(BSPRoom))
                    {
                        BSPRoom room = (BSPRoom)bspComponents[i];
                        for (int d = 0; d < room.doorPosition.Count; d++)
                        {
                            Vector2Int offsetPos = room.doorPosition[d] + new Vector2Int(24, 24);
                            //Debug.Log(offsetPos.x +", "+ offsetPos.y);
                            mapData[offsetPos.x, offsetPos.y] = "1";
                        }
                    }
                }
            }
        }

        Debug.Log(dungeonFullSize);
        RenderToWorldCanvas(map_offset, dungeonFullSize, mapData);
    }

    private void RenderToWorldCanvas(Vector2Int offset, Vector2Int dungeonSize, string[,] mapData) {
        for (int x = 0; x < dungeonSize.x; x++)
        {
            for (int y = 0; y < dungeonSize.y; y++)
            {
                GameObject p = Instantiate(tilePrefab);
                p.transform.position = new Vector2(x, y) + offset + tile_offset;
                SpriteRenderer sRenderer = p.GetComponent<SpriteRenderer>();
                var tileSTP = tileSet.GetTile(mapData[x, y]);

                if (tileSTP != null) {
                    sRenderer.sprite = tileSTP.GetSprite();
                }
            }
        }
    }

    private void FloodFillMap(Vector2Int dungeonSize) {
        for (int x = 0; x < dungeonSize.x; x++) {
            for (int y = 0; y < dungeonSize.y; y++) {
                mapData[x, y] = "1";
            }
        }
    }


    

    private void OnDestroy()
    {
        if (bspMap != null)
            bspMap.OnMapBuild -= RenderTile;
    }
}
