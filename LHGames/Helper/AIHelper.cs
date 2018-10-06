using Newtonsoft.Json;

namespace LHGames.Helper
{
    public static class AIHelper
    {
        /// <summary>
        /// Creates a steal Action. You can only steal from Adjacent tiles
        /// (no diagonals).
        /// </summary>
        /// <param name="direction">The direction from which you want to steal.</param>
        /// <returns>The steal Action.</returns>
        public static string CreateStealAction(Point direction)
        {
            return CreateAction("StealAction", direction);
        }

        /// <summary>
        /// Creates a melee attack Action. You can only attack someone on an 
        /// Adjacent tile.
        /// </summary>
        /// <param name="direction">The directionof your target.</param>
        /// <returns>The attack Action.</returns>
        public static string CreateMeleeAttackAction(Point direction)
        {
            return CreateAction("MeleeAttackAction", direction);
        }

        /// <summary>
        /// Creates a Collect Action. You can only collect resources from 
        /// Adjacent tiles (no diagonals).
        /// </summary>
        /// <param name="direction">The direction of the resource you want
        /// to collect from.
        /// </param>
        /// <returns>The collect Action.</returns>
        public static string CreateCollectAction(Point direction)
        {
            return CreateAction("CollectAction", direction);
        }

        /// <summary>
        /// Creates a move action to the specified direction. You can only move
        /// to adjacent tiles (no diagonals).
        /// </summary>
        /// <param name="direction">The direction in which you want to move.</param>
        /// <returns>The move action.</returns>
        public static string CreateMoveAction(Point direction)
        {
            return CreateAction("MoveAction", direction);
        }

        /// <summary>
        /// Creates an upgrade action for the specified Upgrade. You muse be in
        /// your house to upgrade your player. The action will fail if you do
        /// not have enough resources or are not on your house or are already
        /// at max upgrade for this type.
        /// </summary>
        /// <param name="upgrade">The type of the upgrade.</param>
        /// <returns>The upgrade action.</returns>
        public static string CreateUpgradeAction(UpgradeType upgrade)
        {
            return JsonConvert.SerializeObject(new ActionContent("UpgradeAction", upgrade));
        }

        /// <summary>
        /// Creates a purchase action for the specified item. You need to be ON
        /// a shop tile for this action to succeed. If you are on any other
        /// type of tile, the action will fail. You can only carry 1 of each 
        /// item, except for health potions.
        /// </summary>
        /// <param name="item">The type of item to purchase.</param>
        /// <returns>The purchase Action.</returns>
        public static string CreatePurchaseAction(PurchasableItem item)
        {
            return JsonConvert.SerializeObject(new ActionContent("PurchaseAction", item));
        }

        /// <summary>
        /// Instanciates a heal action. The action will fail if you don't have
        /// any healing potions available.
        /// </summary>
        /// <returns>The heal action.</returns>
        public static string CreateHealAction()
        {
            return JsonConvert.SerializeObject(new ActionContent("HealAction"));
        }

        /// <summary>
        /// Creates an action that does nothing.
        /// </summary>
        /// <returns>An empty action.</returns>
        public static string CreateEmptyAction()
        {
            return "";
        }

        private static string CreateAction(string name, Point target)
        {
            return JsonConvert.SerializeObject(new ActionContent(name, target));
        }
    }
}
