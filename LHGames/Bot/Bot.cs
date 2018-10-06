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
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            //portion pathfinding pour les Mines debut
            PathNoeud playerPosition = new PathNoeud(PlayerInfo.Position.X, PlayerInfo.Position.Y, 0, 0, null);
            List<PathNoeud> minePosition = new List<PathNoeud>();
            List<ResourceTile> minePosition_ = new List<ResourceTile>();
            minePosition_ = GetVisibleResourceTiles(map);
            List<PathNoeud> paths = new List<PathNoeud>();
            Stack<PathNoeud> finalPath = new Stack<PathNoeud>();
            if (finalPath == null) {
                foreach (PathNoeud n in minePosition)
                {
                    PathNoeud path = trouverPathMine(playerPosition, n, map);
                    paths.Add(path);
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
                finalPath = new Stack<PathNoeud>();
                while (currentPath != null)
                {
                    finalPath.Push(currentPath);
                    currentPath = currentPath.getParent();
                }
            }


            if(finalPath.Count > 0)
            {
                PathNoeud prochainMove = finalPath.Pop();
                return AIHelper.CreateMoveAction(new Point(prochainMove.getX(), prochainMove.getY()));
            }

            // TODO: Implement your AI here.
            /*
            if (map.GetTileAt(PlayerInfo.Position.X + _currentDirection, PlayerInfo.Position.Y) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }
            
            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(_currentDirection, 0));
            */
            return null;
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }

        static PathNoeud trouverPathMine(PathNoeud start, PathNoeud end, Map map)
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

                if (verifierNoeudPareil(next, end))
                {
                    return next;
                }

                // trouve les voisins
                List<PathNoeud> neighbours = getNeighbours(next, end);
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
                    if (TileContent.Wall == map.GetTileAt(n.getX(), n.getY()) || TileContent.Empty == map.GetTileAt(n.getX(), n.getY()) || TileContent.Lava == map.GetTileAt(n.getX(), n.getY()) || TileContent.Resource == map.GetTileAt(n.getX(), n.getY()))
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

        static List<PathNoeud> getNeighbours(PathNoeud n, PathNoeud end)
        {
            List<PathNoeud> neighbours = new List<PathNoeud>();

            for (int i = -1; i <= 1; i += 2)
            {
                int posX = n.getX();
                int posY = n.getY();
                PathNoeud tampon = new PathNoeud(posX + i, posY, n.getGCost() + 1, calculerHCost(n, end), n);
                neighbours.Add(tampon);
            }
            for (int i = -1; i <= 1; i += 2)
            {
                int posX = n.getX();
                int posY = n.getY();
                PathNoeud tampon = new PathNoeud(posX, posY + i, n.getGCost() + 1, calculerHCost(n, end), n);
                neighbours.Add(tampon);
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