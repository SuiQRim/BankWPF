using System;

namespace BankObjects.DataBase.Messages
{
    public class AccountMessage
    {
        public AccountMessage(string Status, string Id, string Name, string cardID)
        {
            ActionType = Status;
            this.Id = Id;
            this.Name = Name;
            CardID = cardID;
            MessageDate = DateTime.Now;
        }

        public string ActionType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CardID { get; set; }

        public DateTime MessageDate { get; set; }

    }
}
