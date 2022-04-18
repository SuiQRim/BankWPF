using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankObjects.CardPrefab
{
    public class CardSet
    {
        public CardSet()
        {

        }
        public CardSet(Card card)
        {
            this.CardId = card.CardId;
            this.HolderId = card.HolderID;
            this.HolderName = card.HolderName;
            this.Balance = card.Balance;
            this.DateTotCreate = card.CardCreateData;
        }
        public int Id { get; set; }

        public string CardId { get; set; }
        public string HolderId { get; set; }
        public string HolderName { get; set; }

        public double Balance { get; set; }
        public string DateTotCreate { get; set; }
    }
}
