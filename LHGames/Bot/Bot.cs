using System;
using System.Linq;
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

        public PathNoeud getParent()
        {
            return parent;
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
        List<PathNoeud> finalPath = new List<PathNoeud>();
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            //portion pathfinding pour les Mines debut
            PathNoeud playerPosition = new PathNoeud(PlayerInfo.Position.X, PlayerInfo.Position.Y, 0, 0, null);
            List<PathNoeud> minePosition = new List<PathNoeud>();
            List<ResourceTile> minePosition_ = new List<ResourceTile>();
            minePosition_ = GetVisibleResourceTiles(map);
            foreach(ResourceTile rt in minePosition_)
            {
                minePosition.Add(new PathNoeud(rt.Position.X, rt.Position.Y, 0, 0, null));
            }
            List<PathNoeud> paths = new List<PathNoeud>();
            //List<PathNoeud> finalPath = new List<PathNoeud>();
            if (finalPath.Count() == 0) {
                if (MustReturnToHouse())
                {
                    int x = PlayerInfo.HouseLocation.X;
                    int y = PlayerInfo.HouseLocation.Y;
                    PathNoeud housePath = new PathNoeud(x, y, 0, 0, null);
                    PathNoeud path = trouverPathMine(playerPosition, housePath, map, PlayerInfo);
                    paths.Add(path);
                }
                else
                {
                    foreach (PathNoeud n in minePosition)
                    {
                        PathNoeud path = trouverPathMine(playerPosition, n, map, PlayerInfo);
                        if (path != null)
                        {
                            paths.Add(path);
                        }
                    }
                }
                PathNoeud currentPath = paths[0];
                foreach (PathNoeud n in paths)
                {
                    if (currentPath.getGCost() > n.getGCost())
                    {
                        currentPath = n;
                    }
                }
                //fin portion pathfinding
                finalPath = new List<PathNoeud>();
                while (currentPath != null)
                {
                    finalPath.Add(currentPath);
                    currentPath = currentPath.getParent();
                }
            }

            // miner si a coter dune mine
            if (PlayerInfo.CarriedResources != PlayerInfo.CarryingCapacity && mineAutour(map, PlayerInfo))
            {
                int x = 0;
                int y = 0;
                for (int i = -1; i <= 1; i += 2)
                {
                    if (TileContent.Resource == map.GetTileAt(PlayerInfo.Position.X + i, PlayerInfo.Position.Y))
                    {
                        x = PlayerInfo.Position.X + i;
                        y = PlayerInfo.Position.Y;
                    }
                }
                for (int i = -1; i <= 1; i += 2)
                {
                    if (TileContent.Resource == map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + i))
                    {
                        x = PlayerInfo.Position.X;
                        y = PlayerInfo.Position.Y + i;
                    }
                }
                return AIHelper.CreateCollectAction(new Point(x - PlayerInfo.Position.X, y - PlayerInfo.Position.Y));
            }

            // se deplacer
            if (finalPath.Count > 0)
            {
                PathNoeud prochainMove = finalPath[finalPath.Count - 1];
                finalPath.Remove(prochainMove);
                return AIHelper.CreateMoveAction(new Point(prochainMove.getX()-PlayerInfo.Position.X, prochainMove.getY()-PlayerInfo.Position.Y));
            }
            


            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(_currentDirection, 0));
            
            return null;
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }

        internal bool MustReturnToHouse()
        {
            bool mustReturn = false;
            if (PlayerInfo.CarriedResources + 500 > PlayerInfo.CarryingCapacity)
                mustReturn = true;

            return mustReturn;
        }
        static Boolean mineAutour(Map map, IPlayer PlayerInfo)
        {
            int x = 0;
            int y = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                if (TileContent.Resource == map.GetTileAt(PlayerInfo.Position.X + i, PlayerInfo.Position.Y))
                {
                    x = PlayerInfo.Position.X + i;
                    y = PlayerInfo.Position.Y;
                    return true;
                }
            }
            for (int i = -1; i <= 1; i += 2)
            {
                if (TileContent.Resource == map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + i))
                {
                    x = PlayerInfo.Position.X;
                    y = PlayerInfo.Position.Y + i;
                    return true;
                }
            }
            return false;
        }
        static PathNoeud trouverPathMine(PathNoeud start, PathNoeud end, Map map, IPlayer playerInfo)
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
                openSet.Remove(next);
                closedSet.Add(next);

                if (verifierNoeudPareil(next, end))
                {
                    return next;
                }

                // trouve les voisins
                List<PathNoeud> neighbours = getNeighbours(next, end, map, playerInfo);
                foreach (PathNoeud n in neighbours) {
                    bool contienDeja = false;
                    foreach(PathNoeud n2 in openSet)
                    {
                        if (verifierNoeudPareil(n, n2))
                        {
                            contienDeja = true;
                        }
                    }
                    foreach (PathNoeud n2 in closedSet)
                    {
                        if (verifierNoeudPareil(n, n2))
                        {
                            contienDeja = true;
                        }
                    }
                    bool isWalkable = true;
                    if (TileContent.Wall == map.GetTileAt(n.getX(), n.getY()) || TileContent.Lava == map.GetTileAt(n.getX(), n.getY()))
                    {
                        isWalkable = false;
                    }

                    if (!contienDeja && isWalkable)
                    {
                        openSet.Add(n);
                    }
                }
            }
            return null;
        }

        static List<PathNoeud> getNeighbours(PathNoeud n, PathNoeud end, Map map, IPlayer playerInfo)
        {
            List<PathNoeud> neighbours = new List<PathNoeud>();

            for (int i = -1; i <= 1; i += 2)
            {
                int posX = n.getX();
                int posY = n.getY();
                if (map.VisibleDistance > Math.Abs(posX - playerInfo.Position.X) && map.VisibleDistance > Math.Abs(posY - playerInfo.Position.Y))
                {
                    PathNoeud tampon = new PathNoeud(posX + i, posY, n.getGCost() + 1, calculerHCost(n, end), n);
                    neighbours.Add(tampon);
                }
            }
            for (int i = -1; i <= 1; i += 2)
            {
                int posX = n.getX();
                int posY = n.getY();
                if (map.VisibleDistance > Math.Abs(posX - playerInfo.Position.X) && map.VisibleDistance > Math.Abs(posY - playerInfo.Position.Y))
                {
                    PathNoeud tampon = new PathNoeud(posX, posY + i, n.getGCost() + 1, calculerHCost(n, end), n);
                    neighbours.Add(tampon);
                }
            }
            return neighbours;
        }

        static int calculerHCost(PathNoeud n, PathNoeud end)
        {
            int x = Math.Abs(n.getX() - end.getX());
            int y = Math.Abs(n.getY() - end.getY());
            return x + y;
        }

        internal List<ResourceTile> GetVisibleResourceTiles(Map map)
        {
            List<Tile> visibleTiles = map.GetVisibleTiles().ToList<Tile>();
            List<ResourceTile> visibleResourceTiles = new List<ResourceTile>();

            foreach (ResourceTile t in visibleTiles.OfType<ResourceTile>())
            {
                visibleResourceTiles.Add(t);
            }

            return visibleResourceTiles;
        }

        static bool verifierNoeudPareil(PathNoeud n1, PathNoeud n2)
        {
            if (n1.getX() == n2.getX() && n1.getY() == n2.getY())
            {
                return true;
            }
            return false;
        }

    }
}

class TestClass
{
    public string Test { get; set; }
}