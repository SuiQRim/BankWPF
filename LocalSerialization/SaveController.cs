using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using BankObjects.ClientPrefab;

namespace LocalSerialization
{
    public class SaveController
    {
        public ISaveMode? Mode { get; set; }

        public void Serilize(Client client, List<Client> clientCollection) 
        {
            Mode.SaveSelectedClient(client);
            Mode.SaveAllClients(clientCollection);
        }

    }
}
