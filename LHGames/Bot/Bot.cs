using System;
using System.Collections.Generic;
using LHGames.Helper;

namespace LHGames.Bot
{
    class PathNoeud
    {
        int x;
        int y;
        int gCost;
        int hCost;
        int fCost;
        PathNoeud parent;

        public PathNoeud(int x_, int y_, int gCost_, int hCost_, PathNoeud parent_)
        {
            x = x_;
            y = y_;
            gCost = gCost_;
            hCost = hCost_;
            parent = parent_;
            setFCost();
        }

        void setFCost()
        {
            fCost = hCost + gCost;
        }

        public int getFCost()
        {
            return fCost;
        }

        public int getHCost()
        {
            return hCost;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getGCost()
        {
            return gCost;
        }

    }

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
            PathNoeud playerPosition = new PathNoeud(PlayerInfo.Position.X, PlayerInfo.Position.Y, 0, 0, null);
            PathNoeud minePosition = new PathNoeud()
            List<PathNoeud> path =  trouverPathMine(playerPosition, minePosition);

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

        static List<PathNoeud> trouverPathMine(PathNoeud start, PathNoeud end)
        {
            List<PathNoeud> openSet = new List<PathNoeud>();
            List<PathNoeud> closedSet = new List<PathNoeud>();
            List<PathNoeud> path = new List<PathNoeud>();
            openSet.Add(start);
            while (openSet.Count > 0)
            {
                // choisi le prochain noeud a evaluer
                PathNoeud next = openSet[0];
                foreach(PathNoeud n in openSet)
                {
                    if (next.getFCost() > n.getFCost() || (next.getFCost() == n.getFCost() && next.getHCost() < n.getHCost()))
                    {
                        next = n;
                    }
                }
                // trouve les voisins
                List<PathNoeud> neighbours = getNeighbours(n, end);

            }

            return null;
        }

        static List<PathNoeud> getNeighbours(PathNoeud n, PathNoeud end)
        {
            List<PathNoeud> neighbours = new List<PathNoeud>();

            for (int i = -1; i <= 1; i += 2)
            {
                int posX = n.getX();
                int posY = n.getY();
                PathNoeud tampon = new PathNoeud(posX, posY, n.getGCost() + 1, )

                neighbours.Add()
            }


            return neighbours;
        }

        static int calculerHCost(PathNoeud n, PathNoeud end)
        {
            int hCost = 0;
            int n.getX();

            return hCost;
        }

    }
}

class TestClass
{
    public string Test { get; set; }
}