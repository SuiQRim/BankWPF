using BankObjects.ClientPrefab;
using System.Collections.Generic;
namespace LocalSerialization
{
    public interface ISaveMode
    {
        /// <summary>
        /// Сохранет выбранного клиента
        /// </summary>
        /// <param name="client"></param>
        public void SaveSelectedClient(Client client);

        public void SaveAllClients(List<Client> clientCollection);

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string Format { get; }
    }
}
