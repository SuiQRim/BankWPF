using BankObjects.ClientPrefab;
using System.Text.Json;

namespace LocalSerialization.Mods
{
    public class KeeperJson : Keeper
    {
        public KeeperJson() : base("json") { }
        protected override string[] CreateFormat(ClientSet client, string path)
        {
            string json = JsonSerializer.Serialize(client);
            string[] file = new string[2] {path, json };
            return file;
        }
    }
}
