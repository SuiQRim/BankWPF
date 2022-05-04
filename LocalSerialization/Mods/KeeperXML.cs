using BankObjects.ClientPrefab;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace LocalSerialization.Mods
{
    public class KeeperXML : Keeper
    {
        public KeeperXML() : base("xml") { }

        protected override string[] CreateFormat(ClientSet client, string path)
        {
            string xml;

            XmlSerializer serializer = new (typeof(ClientSet));
            using(StringWriter sw = new ())
            {
                serializer.Serialize(sw, client);
                xml = sw.ToString();
            }

            string[] file = new string[2] { path, xml};
            return file;
        }
        protected override string[] CreateFormat(List<ClientSet> clientList, string combinePath)
        {
            string xml;

            XmlSerializer serializer = new(typeof(List<ClientSet>));
            using (StringWriter sw = new())
            {
                serializer.Serialize(sw, clientList);
                xml = sw.ToString();
            }

            string[] file = new string[2] { combinePath, xml };
            return file;
        }
    }
}
