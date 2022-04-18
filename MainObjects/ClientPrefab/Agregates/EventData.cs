using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace BankObjects.ClientPrefab.Agregates
{
    public class EventData
    {
        public EventData(string type, double discount) {

            Type = type;

            Discount = discount;
        }
        public string Type { get; set; }

        public double Discount { get; set; }

        private static Random rnd = new();

        /// <summary>
        /// Список всех кешбек Акций
        /// </summary> 
        private static readonly ObservableCollection<EventData> Events = new ObservableCollection<EventData>
        {
            new("Спорт",10),
            new("Кгини",10),
            new("Мебель",7),
            new("Игрушки",7),
            new("Комп. железо",5),
            new("Трансп. Проезд",5),
            new("Для шитья",7),
            new("Телевизоры",5),
            new("Ремонт дома",7),
            new("Гигиена",10),
            new("Антистрессы",7),
            new("Кружки",7),
            new("Аниме",7),
            new("Кино",7),
            new("Подписки",7),
            new("Безопасность",7),
            new("Оружие",5),
            new("Питание",7),
            new("Посуда",7),
            new("Обувь",7),
            new("Одежда",7),
            new("Здоровье",7),
            new("Путишествие",7),
            new("Обучение",7),
            new("Растения",7),
            new("Контроллеры",7),
            new("Смартфоны",7)
        };

        /// <summary>
        /// Возвращает список кешбек акций
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<EventData> GetEvents() => Events;

        /// <summary>
        /// Возвращает N кешбек акций
        /// </summary>
        /// <param name="N">Кол-во кешбек акций</param>
        /// <returns></returns>
        public static ObservableCollection<EventData> GetEvents(int N)
        {

            ObservableCollection<EventData> events = new();

            List<EventData> eventNumbers = new();

            EventData newEvent;

            int randomEvent;

            for (int i = 0; i < N;)
            {
                randomEvent = rnd.Next(Events.Count);

                newEvent = Events[randomEvent];

                if (Repetitions(eventNumbers, newEvent) == false) continue;

                i++;

                events.Add(newEvent);
                eventNumbers.Add(newEvent);
            }


            return events;
        }

        /// <summary>
        /// Исклучает вариант повторения кешбек акций в списке
        /// </summary>
        /// <param name="eventNumbers">Список кешбек акций</param>
        /// <param name="newEvent">Новая акция</param>
        /// <returns>Повторения отсутсвуют?</returns>
        private static bool Repetitions(List<EventData> eventNumbers, EventData newEvent)
        {

            foreach (EventData item in eventNumbers)
            {
                if (item.Type == newEvent.Type)
                {
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// Проверяет активирован ли нужный кешбек
        /// </summary>
        /// <param name="clientEvents">Активированные кешбек акции клиента</param>
        /// <param name="selectedEvent">Выбранная кешбек акция</param>
        /// <returns></returns>
        public static bool EventUsing(ObservableCollection<EventData> clientEvents, EventData selectedEvent)
        {

            bool CanUseEvent = false;

            if (selectedEvent != null)
            {
                foreach (EventData item in clientEvents)
                {
                    if (item.Type == selectedEvent.Type)
                    {
                        CanUseEvent = true;
                        break;
                    }
                }
            }
            return CanUseEvent;
        }
    }
}
