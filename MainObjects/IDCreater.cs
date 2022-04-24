using System;

namespace BankObjects
{
    public static class IDCreater
    {
        private static Random rnd = new();

        /// <summary>
        /// Рандомно создаёт ключ
        /// </summary>
        /// <param name="count">Кол-во символов</param>
        /// <param name="charPrefab">Разрешённые символы</param>
        /// <returns></returns>
        public static string GetRandomChar(int count, string charPrefab)
        {
            string a = "";

            for (int i = 0; i < count; i++)
            {
                var index = rnd.Next(charPrefab.Length);
                a += charPrefab[index];

                if (i < count - 3 & (i + 1) % 4 == 0)
                {
                    a += "-";
                }
            }

            return a;
        }

        /// <summary>
        /// Рандомно создает ключ (для инвестиции
        /// </summary>
        /// <param name="charPrefab">Разрешённые символы</param>
        /// <returns></returns>
        public static string GetRandomChar(string charPrefab)
        {
            string a = "";

            for (int i = 0; i < 10; i++)
            {
                var index = rnd.Next(charPrefab.Length);
                a += charPrefab[index];

            }

            return a;
        }


        /// <summary>
        /// Извлекает 4 последних символов Id карты и возвращает скрытый Id
        /// </summary>
        /// <param name="CardId">Id карты</param>
        /// <returns></returns>
        public static string LastFourChar(string CardId)
        {

            string a = "XXXX-XXXX-XXXX-";

            for (int i = 0; i < 4; i++)
            {
                a += CardId[CardId.Length - 4 + i];
            }

            return a;
        }
    }
}
