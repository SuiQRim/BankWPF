using System;
using BankObjects.ClientPrefab;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalSerialization
{
    internal interface ISaveMode
    {
        public void SaveSelectedClient(Client client);

        public void SaveAllData();
    }
}
