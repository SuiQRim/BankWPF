﻿using BankObjects.ClientPrefab;
using System.IO;

namespace LocalSerialization.Mods
{
    public abstract class Keeper : ISaveMode
    {
        public Keeper(string format) { this._fileFormat = format;}
       

        // Предоставленные папки
        private const string _localDirectory = @"LocalSave";
        private const string _clientSaveDirectory = @"ClientData";


        public string _fileFormat;
        public string Format { get => _fileFormat; }

        /// <summary>
        /// Возвращает текст в верном формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string CorrectText(string text) => text.Replace(" ", string.Empty);

        /// <summary>
        /// Собирает путь к файлу
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        private string CombinePathForClientFile(string clientName)
        {
            clientName = CorrectText(clientName);
            string combinePath = Path.Combine(_localDirectory, _clientSaveDirectory, Format, clientName + $".{Format}");
            return combinePath;
        }

        /// <summary>
        /// Сохраняет выбранного клиента
        /// </summary>
        /// <param name="client"></param>
        public void SaveSelectedClient(Client client)
        {
            string[] file = CreateFormat(new ClientSet(client), CombinePathForClientFile(client.Name));

            try
            {
                WriteFile(file);
            }
            catch (DirectoryNotFoundException)
            {
                DataDirectory.CreateDirectory();
                WriteFile(file);
            }

        }

        /// <summary>
        /// Записывает текст с файл
        /// </summary>
        /// <param name="file"></param>
        private static void WriteFile(string [] file) 
        {
            using (StreamWriter sw = new(file[0]))
            {
                sw.WriteAsync(file[1]);
            };
        }

        /// <summary>
        /// Особая логика серелизации объекта в текст
        /// </summary>
        /// <param name="client">Клиент</param>
        /// <param name="combinePath">Путь без расширения</param>
        /// <returns></returns>
        protected abstract string [] CreateFormat(ClientSet client, string combinePath);

    }
}
