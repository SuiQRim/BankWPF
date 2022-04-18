using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankObjects.CardPrefab.Invest
{
    public class InvestmentSet : CardSet
    {

        public InvestmentSet() { }

        public InvestmentSet(Investment invest) : base (invest)
        {
            this.StartBalance = invest.StartBalance;
            this.ClearProfit = invest.ClearProfit; 
            this.AccumulationBalance =  invest.AccumulationBalance;
            this.MonthNow = invest.MonthNow; 
            this.MonthCount = invest.MonthCount;
            this.Precent = invest.Precent;
            this.ProfitPrecent = invest.ProfitPrecent;
            this.IsReady = invest.isReady;
            this.IsActivated = invest.isActivated;
            this.IsAccumulation = invest.IsAccumulation;
        }
        public double StartBalance { get; set; }

        public double ClearProfit { get; set; }

        public double AccumulationBalance { get; set; }

        public int MonthNow { get; set; }

        public int MonthCount { get; set; }

        public double Precent { get; set; }

        public double ProfitPrecent { get; set; }   

        public bool IsReady { get; set; }

        public bool IsActivated { get; set; }  

        public bool IsAccumulation { get; set; }
    }
}
