using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace LocalSerialization
{
    public static class DataDirectory
    {
        public static void CreateDirectory() 
        {
            Build();
        }

        /// <summary>
        /// Создает директорию со всеми отсутствующими папками
        /// P.s. я понимаю что это жудко ужасный метод
        /// </summary>
        private static void Build() 
        {
            string path = Diretory;
            Directory.CreateDirectory(path);

            path = Path.Combine(Diretory, ClientSaves);
            Directory.CreateDirectory(path);

            string original = Path.Combine(Diretory, ClientSaves, OriginalClientPath);
            string collection = Path.Combine(Diretory, ClientSaves, CollectionClientPath);


            Directory.CreateDirectory(original + @"\json");
            Directory.CreateDirectory(original + @"\xml");
            Directory.CreateDirectory(collection + @"\json");
            Directory.CreateDirectory(collection + @"\xml");
        }

        public const string Diretory = "LocalSave";
        public const string ClientSaves = "ClientData";
        public const string OriginalClientPath = "Original";
        public const string CollectionClientPath = "Collection";

    }
}
