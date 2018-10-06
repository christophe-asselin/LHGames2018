using System;
using System.Collections.Generic;
using LHGames.Helper;
using System.Linq;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;

        internal Bot() { }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>
        internal void BeforeTurn(IPlayer playerInfo)
        {
            PlayerInfo = playerInfo;
           
        }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        /// <param name="map">The gamemap.</param>
        /// <param name="visiblePlayers">Players that are visible to your bot.</param>
        /// <returns>The action you wish to execute.</returns>
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            // TODO: Implement your AI here.
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }

            List<Tile> visibleHouseTiles = new List<Tile>();
            visibleHouseTiles = GetVisibleHouses(map);

            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(_currentDirection, 0));
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }

        internal List<ResourceTile> GetVisibleResourceTiles(Map map)
        {
            List<Tile> visibleTiles = map.GetVisibleTiles().ToList<Tile>();
            List<ResourceTile> visibleResourceTiles = new List<ResourceTile>();

            foreach (ResourceTile t in visibleTiles.OfType<ResourceTile>())
            {
                visibleResourceTiles.Add(t);
                Console.WriteLine(t.Position.X + ", " + t.Position.Y);
            }

            return visibleResourceTiles;
        }

        internal List<Tile> GetVisibleHouses(Map map)
        {
            List<Tile> visibleTiles = map.GetVisibleTiles().ToList<Tile>();
            List<Tile> visibleHouseTiles = new List<Tile>();

            foreach (Tile t in visibleTiles)
            {
                if (t.TileType == TileContent.House && t.Position != PlayerInfo.HouseLocation)
                {
                    visibleHouseTiles.Add(t);
                    Console.WriteLine(t.Position.X + ", " + t.Position.Y);
                }
            }

            return visibleHouseTiles;
        }

        internal int BuyNPotions()
        {
            int nPotions = GetNPotions(PlayerInfo);
            return (10 - nPotions);
        }

        internal bool MustReturnToHouse()
        {
            bool mustReturn = false;
            if (PlayerInfo.CarriedResources + 500 >= PlayerInfo.CarryingCapacity)
                mustReturn = true;

            return mustReturn;
        }

        internal IPlayer AttackNearby(IEnumerable<IPlayer> visiblePlayers)
        {
            foreach(IPlayer player in visiblePlayers)
            {
                if (PlayerInfo.AttackPower > player.Defence && (PlayerInfo.Health + PlayerInfo.Defence + (5 * GetNPotions(PlayerInfo)) - player.AttackPower) > (player.Health + player.Defence + (5 * GetNPotions(player)) - PlayerInfo.AttackPower))
                    return player;
            }

            return null;
        }

        internal int GetNPotions(IPlayer player)
        {
            int nPotions = 0;
            foreach (PurchasableItem item in player.CarriedItems)
            {
                if (item == PurchasableItem.HealthPotion)
                    nPotions++;
            }
            return nPotions;
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}