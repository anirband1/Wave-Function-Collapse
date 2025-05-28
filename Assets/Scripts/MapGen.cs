using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] int MAP_SIZE = 5;
    [SerializeField] int tileSize = 2;
    [Space]
    [SerializeField] GameObject straight;
    [SerializeField] GameObject turn_90;
    [SerializeField] GameObject way_3;
    [SerializeField] GameObject way_4;


    // NS,     // straight default 0
    // EW,     // straight         1
    // SE,     // 90               2 
    // EN,     // 90 default       3
    // NW,     // 90               4
    // WS,     // 90               5
    // ESW,    // 3way             6
    // SWN,    // 3way             7
    // WNE,    // 3way default     8
    // NES,    // 3way             9
    // NSEW,   // 4way             10

    // NIL,    // air              11

    // WENS
    // EWSN

    // posx - {1, 4, 5, 6, 8, 9, 10}
    // negx - {1, 2, 3, 6, 7, 8, 10}
    // posz - {0, 2, 5, 6, 7, 9, 10}
    // negz - {0, 3, 4, 7, 8, 9, 10}

    // ~posx - {0, 2, 3, 7, 11}
    // ~negx - {0, 4, 5, 9, 11}
    // ~posz - {1, 3, 4, 8, 11}
    // ~negz - {1, 2, 5, 6, 11}



    GameObject[] roadTilesGO = new GameObject[12];
    Quaternion[] roadRotation = new Quaternion[12];

    RoadTile[] roadTiles = new RoadTile[12];

    void Start()
    {
        InitializeTiles();

        List<int>[,] grid = WFC.StartWFC(MAP_SIZE, roadTiles);

        for (int i = 0; i < MAP_SIZE; i++)
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                int index = grid[i, j][0];
                if (index != 11)
                {
                    GameObject go = Instantiate(roadTilesGO[index], new Vector3(i * tileSize, 0, j * tileSize), Quaternion.Euler(-90, 90 * roadTiles[index].rotY, 0));
                    go.transform.localScale *= tileSize;
                }
            }
        }
    }

    void InitializeTiles()
    {
        #region random
        roadTilesGO[0] = straight;
        roadTilesGO[1] = straight;
        roadTilesGO[2] = turn_90;
        roadTilesGO[3] = turn_90;
        roadTilesGO[4] = turn_90;
        roadTilesGO[5] = turn_90;
        roadTilesGO[6] = way_3;
        roadTilesGO[7] = way_3;
        roadTilesGO[8] = way_3;
        roadTilesGO[9] = way_3;
        roadTilesGO[10] = way_4;

        roadRotation[0] = Quaternion.Euler(-90, 0, 0);
        roadRotation[1] = Quaternion.Euler(-90, 90, 0);
        roadRotation[2] = Quaternion.Euler(-90, 0, 0);
        roadRotation[3] = Quaternion.Euler(-90, 90, 0);
        roadRotation[4] = Quaternion.Euler(-90, 180, 0);
        roadRotation[5] = Quaternion.Euler(-90, -90, 0);
        roadRotation[6] = Quaternion.Euler(-90, 0, 0);
        roadRotation[7] = Quaternion.Euler(-90, 90, 0);
        roadRotation[8] = Quaternion.Euler(-90, 180, 0);
        roadRotation[9] = Quaternion.Euler(-90, -90, 0);
        roadRotation[10] = Quaternion.Euler(-90, 0, 0);
        #endregion

        roadTiles[0] = new RoadTile(0, new int[4][] {
            new int[] {0, 2, 3, 7, 11},
            new int[] {0, 4, 5, 9, 11},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[1] = new RoadTile(1, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {1, 3, 4, 8, 11},
            new int[] {1, 2, 5, 6, 11}
        });
        roadTiles[2] = new RoadTile(1, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {0, 4, 5, 9, 11},
            new int[] {1, 3, 4, 8, 11},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[3] = new RoadTile(0, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {0, 4, 5, 9, 11},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {1, 2, 5, 6, 11}
        });
        roadTiles[4] = new RoadTile(3, new int[4][] {
            new int[] {0, 2, 3, 7, 11},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {1, 2, 5, 6, 11}
        });
        roadTiles[5] = new RoadTile(2, new int[4][] {
            new int[] {0, 2, 3, 7, 11},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {1, 3, 4, 8, 11},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[6] = new RoadTile(2, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {1, 3, 4, 8, 11},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[7] = new RoadTile(1, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {0, 4, 5, 9, 11},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[8] = new RoadTile(0, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {1, 2, 5, 6, 11}
        });
        roadTiles[9] = new RoadTile(3, new int[4][] {
            new int[] {0, 2, 3, 7, 11},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[10] = new RoadTile(0, new int[4][] {
            new int[] {1, 4, 5, 6, 8, 9, 10},
            new int[] {1, 2, 3, 6, 7, 8, 10},
            new int[] {0, 2, 5, 6, 7, 9, 10},
            new int[] {0, 3, 4, 7, 8, 9, 10}
        });
        roadTiles[11] = new RoadTile(0, new int[4][] {
            new int[] {0, 2, 3, 7, 11},
            new int[] {0, 4, 5, 9, 11},
            new int[] {1, 3, 4, 8, 11},
            new int[] {1, 2, 5, 6, 11}
        });

    }
}
