using System;
using BankObjects.ClientPrefab;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LocalSerialization.Mods
{
    internal abstract class Keeper : ISaveMode
    {
        protected static int Id;
        private const string Road = @"LocalSave\";
        private const string AllDataSavePath = @"Save_BankData\";
        private const string ClientSavePath = @"Save_BankData\";

        /// <summary>
        /// Работа с текстом чтобы все сохранялось в верном формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string CorrectText(string text) => text.Replace(" ", string.Empty);

        private static string CompositeClientSaveFileName(string clientName)
        {
            clientName = CorrectText(clientName);
            string combinePath = Path.Combine(Road,ClientSavePath, clientName);
            return combinePath;
        } 

        public void SaveAllData()
        {
        }

        public async void SaveSelectedClient(Client client)
        {
            string[] file = CreateFormat(client, CompositeClientSaveFileName(client.Name));

            using (StreamWriter sw = new StreamWriter(file[0]))
            {
                sw.WriteAsync(file[1]);
            };
        }
        
        protected abstract string [] CreateFormat(Client client, string combinePath);

    }
}
