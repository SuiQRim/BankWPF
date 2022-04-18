using System;

namespace BankObjects.EventMessages
{
    public class AccountMessage
    {
        public AccountMessage(string Status, string Id, string Name, string cardID)
        {
            this.ActionType = Status;
            this.Id = Id;
            this.Name = Name;
            this.CardID = cardID;
            MessageDate = DateTime.Now;
        }

        public string ActionType { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CardID { get; set; }

        public DateTime MessageDate { get; set; }

    }
}
