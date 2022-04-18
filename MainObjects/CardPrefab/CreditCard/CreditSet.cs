using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankObjects.CardPrefab.CreditCard
{
    public class CreditSet : CardSet
    {
        public CreditSet() { }
        public CreditSet(Credit credit) : base (credit)
        {
            this.StartBalance = credit.StartBalance;
            this.Debt = credit.Debt;
            this.MonthDebt = credit.MonthDebt;
            this.MonthCount = credit.MonthCount;
            this.MonthNow = credit.MonthNow;
            this.Precent = credit.Precent;
            this.IsReady = credit.IsReady;
            this.IsFreez = credit.IsFreez;
        }

        public double StartBalance { get; set; }

        public double Debt { get; set; }

        public double MonthDebt { get; set; }

        public int MonthCount { get; set; }

        public int MonthNow { get; set; }

        public double Precent { get; set; }    

        public bool IsReady { get; set; }

        public bool IsFreez { get; set; }
    }
}
