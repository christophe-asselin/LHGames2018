using Newtonsoft.Json;

namespace LHGames.Helper
{
    public class ActionContent
    {
        public string ActionName { get; set; }
        public string Content { get; set; }
        public ActionContent(string name)
        {
            ActionName = name;

        }
        public ActionContent(string name, Point content)
        {
            ActionName = name;
            Content = JsonConvert.SerializeObject(content);
        }

        public ActionContent(string name, UpgradeType content)
        {
            ActionName = name;
            Content = content.ToString();
        }

        public ActionContent(string name, PurchasableItem content)
        {
            ActionName = name;
            Content = content.ToString();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
