using BankObjects.ClientPrefab;
using System.IO;
using System.Text.Json;

namespace LocalSerialization.Mods
{
    internal class KeeperJson : Keeper
    {
        protected override string[] CreateFormat(Client client, string combinePath)
        {
            string json = JsonSerializer.Serialize(client);
            string fullPath = combinePath + ".json";

            string[] file = new string[2] { fullPath, json };

            return file;
        }
    }
}
