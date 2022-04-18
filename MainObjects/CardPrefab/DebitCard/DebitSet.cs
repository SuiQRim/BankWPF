using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankObjects.CardPrefab.DebitCard
{
    public class DebitSet : CardSet
    {
        public DebitSet()
        {

        }
        public DebitSet(Debit debit): base (debit)
        {
            this.CashBack = debit.CashBack;
        }

        public double CashBack { get ; set; }
    }
}
