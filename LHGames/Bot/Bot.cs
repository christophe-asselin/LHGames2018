using System;
using System.Collections.Generic;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }
        private int _currentDirection = 1;

        private int _numAmelioration = 0;
        List<string> Amelioration = new List<string> {
            "1S", "1C", "2S", "2C",
            "1D", "1H", "3S", "2D", "3C",
            "2H", "4C", "3D", "4S", "3H",
            "4D", "4H", "1A", "2A", "3A", "5C",
            "5S", "5D", "5H", "4A", "5A"
        };

        List<int> Cout_Amelioration = new List<int> {
            10000, 15000, 25000, 50000, 100000 
        };

        internal bool CanBuy_Amelioration(IPlayer playerInfo) {
            string AmeliorationEnCours = Amelioration[_numAmelioration];
            if(playerInfo.Score >= Cout_Amelioration[AmeliorationEnCours[0]]) {
                return true;
            }
            return false;
        }

        // Doit d'abord vérifier si peut acheter
        internal string WhatToBuy_Amelioration(IPlayer playerInfo) {

            string AmeliorationEnCours = Amelioration[_numAmelioration];
                
            if(AmeliorationEnCours[1] == 'C')
                return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            if(AmeliorationEnCours[1] == 'S')
                return AIHelper.CreateUpgradeAction(UpgradeType.CollectingSpeed);
            if(AmeliorationEnCours[1] == 'D')
                return AIHelper.CreateUpgradeAction(UpgradeType.Defence);
            if(AmeliorationEnCours[1] == 'A')
                return AIHelper.CreateUpgradeAction(UpgradeType.AttackPower);
            if(AmeliorationEnCours[1] == 'H')
                return AIHelper.CreateUpgradeAction(UpgradeType.MaximumHealth);

            _numAmelioration++;
            return "";
        }

        internal Bot() { }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>

        
        // Conditions de l'appel de la fonction :
        // 1 - Si le joueur est à côté d'une mine
        internal string Mining(IPlayer playerInfo, Point direction) {
            
            // CreateCollectAction(Point direction)
            if(playerInfo.CarryingCapacity.Equals(playerInfo.TotalResources)) {
                return AIHelper.CreateEmptyAction();
            }
            return AIHelper.CreateCollectAction(direction);
        }

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
            
            if(PlayerInfo.Position.X == PlayerInfo.HouseLocation.X)
                if(PlayerInfo.Position.Y == PlayerInfo.HouseLocation.Y)
                    if(CanBuy_Amelioration(PlayerInfo))
                        return WhatToBuy_Amelioration(PlayerInfo);

            // TODO: Implement your AI here.
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }

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
    }
}

class TestClass
{
    public string Test { get; set; }
}