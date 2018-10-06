using System;
using System.Collections.Generic;
using System.Linq;

namespace LHGames.Helper
{
    /// <summary>
    /// This class represents the GameMap.
    /// DO NOT MODIFY FUNCTIONS FROM THIS CLASS.
    /// </summary>
    internal class Map
    {
        private Tile[,] Tiles { get; set; }
        private int XMin { get; set; }
        private int YMin { get; set; }
        private int XMax { get; set; }
        private int YMax { get; set; }

        /// <summary>
        /// How far your Bot can see.
        /// </summary>
        public int VisibleDistance { get; set; }

        /// <summary>
        /// If you can break walls (trees)
        /// </summary>
        public bool WallsAreBreakable { get; set; }
        
        internal Map(string customSerializedMap, int xMin, int yMin, bool wallsAreBreakable)
        {
            XMin = xMin;
            YMin = yMin;
            WallsAreBreakable = wallsAreBreakable;
            DeserializeMap(customSerializedMap);
            InitMapSize();
        }

        /// <summary>
        /// Returns the TileType at this location. If you try to look outside 
        /// of your visible region, it will always return TileType.Tile (Empty
        /// tile).
        /// 
        /// Negative values are valid since the map wraps around when you reach
        /// the end.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The content of the tile.</returns>
        internal TileContent GetTileAt(int x, int y)
        {
            if (x < XMin || x > XMax || y < YMin || y > YMax)
            {
                return TileContent.Empty;
            }
            return Tiles[x - XMin, y - YMin].TileType;
        }

        /// <summary>
        /// Returns an IEnumerable of all tiles that are visible to your bot.
        /// </summary>
        /// <returns>All visible tiles.</returns>
        internal IEnumerable<Tile> GetVisibleTiles()
        {
            return Tiles.Cast<Tile>();
        }

        /// <summary>
        /// Deserialize the map received from the game server. 
        /// DO NOT MODIFY THIS.
        /// </summary>
        /// <param name="customSerializedMap">The received map.</param>
        private void DeserializeMap(string customSerializedMap)
        {
            customSerializedMap = customSerializedMap.Substring(1, customSerializedMap.Length - 2);
            var rows = customSerializedMap.Split('[');
            var column = rows[1].Split('{');
            Tiles = new Tile[rows.Length - 1, column.Length - 1];
            for (int i = 0; i < rows.Length - 1; i++)
            {
                column = rows[i + 1].Split('{');
                for (int j = 0; j < column.Length - 1; j++)
                {
                    var tileType = (byte)TileContent.Empty;
                    if (column[j + 1][0] != '}')
                    {
                        var infos = column[j + 1].Split('}');
                        infos = infos[0].Split(',');
                        if (infos.Length > 1)
                        {
                            tileType = byte.Parse(infos[0]);
                            var amountLeft = int.Parse(infos[1]);
                            var density = double.Parse(infos[2]);
                            Tiles[i, j] = new ResourceTile(tileType, i + XMin, j + YMin, amountLeft, density);
                        }
                        else
                        {
                            tileType = byte.Parse(infos[0]);
                        }
                    }
                    if (tileType != (byte)TileContent.Resource)
                    {
                        Tiles[i, j] = new Tile(tileType, i + XMin, j + YMin);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the XMax, YMax and VisibleDistance.
        /// </summary>
        private void InitMapSize()
        {
            if (Tiles == null)
            {
                throw new InvalidOperationException("Tiles cannot be null.");
            }

            XMax = XMin + Tiles.GetLength(0);
            YMax = YMin + Tiles.GetLength(1);
            VisibleDistance = (XMax - XMin - 1) / 2;
        }
    }
}
