using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankObjects.ClientPrefab;

namespace LocalSerialization
{
    internal class Save
    {
        public ISaveMode Mode { get; set; }

        public void Serilize(Client client) 
        {
            Mode.SaveSelectedClient(client);
        }

    }
}
