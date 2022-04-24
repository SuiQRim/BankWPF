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
            string path = "LocalSave";;
            Directory.CreateDirectory("LocalSave");
            path = Path.Combine(path, "ClientData");
            Directory.CreateDirectory("LocalSave");
            Directory.CreateDirectory(path + @"\json");
            Directory.CreateDirectory(path + @"\xml");
        }
    }
}
