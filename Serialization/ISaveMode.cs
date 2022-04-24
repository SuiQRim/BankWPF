using BankObjects.ClientPrefab;

namespace LocalSerialization
{
    public interface ISaveMode
    {
        /// <summary>
        /// Сохранет выбранного клиента
        /// </summary>
        /// <param name="client"></param>
        public void SaveSelectedClient(Client client);

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string Format { get; }
    }
}
