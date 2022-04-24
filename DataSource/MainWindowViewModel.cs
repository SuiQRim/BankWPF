using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using ViewModel;
using BankObjects.ClientPrefab;
using BankObjects.ClientPrefab.Agregates;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.CardPrefab;
using BankObjects.CardPrefab.DebitCard;
using BankObjects.CardPrefab.CreditCard;
using BankObjects.CardPrefab.Invest;
using DataSource.Child;
using System.Data.SqlClient;
using LocalSerialization;
using LocalSerialization.Mods;
using BankObjects.DataBase.Messages;
using BankObjects.DataBase;

namespace DataSource
{
    public class MainWindowViewModel : VMNotifyPropertyChanged
    {

        #region Methods

        public void Invest_VisibilityControl(Investment value)
        {

            if (value.IsAccumulation == false) IsCanOutput = true;
            else IsCanOutput = value.isReady;


            if (value.isActivated)
            {
                Invest_DebitComboBox_Header = "На карту...";
            }
            else
            {
                Invest_DebitComboBox_Header = "Пополнить с...";
                Invest_MenuActivate = true;
                IsCanOutput = false;
            }
            if (value.isActivated & value.IsAccumulation & !value.isReady)
            {
                Invest_CloseAllWindows();
            }
        }

        private Card? DeterminesWhoCardIs() {

            if (TabChanged == 1) return Selected_DebitCard;
            else if (TabChanged == 2) return Selected_CreditCard;
            return null;
        }
        private void ControlAllTransactionMenuItems_Vissibility(bool value) {

            IsTransaction_ThroughOurCards = value;
            IsTransaction_Shoping = value;
            IsTransaction_ByPhoneNumber = value;
            TransactionCard = null;
            TransactionClient = null;
            IsTransaction = value;
        }

       


        private void CardsUpDate()
        {

            OnPropertyChanged("Selected_Client");

            OnPropertyChanged("Selected_DebitCard");

            OnPropertyChanged("Selected_CreditCard");

            OnPropertyChanged("Selected_Investment");

            if (Selected_Investment != null)
            {
                Invest_VisibilityControl(Selected_Investment);
            }
            

        }
     
        public void Invest_CloseAllWindows() {

            Invest_IsWithDrawCash_Visibility = false;
            Invest_MenuActivate = false;
            IsCanOutput = false;
            Invest_ComboBoxTransferCard_Visibility = false;
        }

     

        private void ReversSelectedCard(int value)
        {
            Selected_DebitCard = null;
            Selected_CreditCard = null;
            Selected_Investment = null;

            if (value == 1 ^ value == 2)
            {
                Card_Comboboxes_Brush = Brushes.Green;
                Selected_TypeOfCard_Text = "Выберите карту...";
            }
            else if (value == 3)
            {
                Selected_TypeOfCard_Text = "Выберите вложение...";
            }
            Selected_WayOfTransaction_Header = "Перевод средств";

            IsInfo_Visible = false;
            IsCardHistoryMenu_Visiblle = false;
        }

        #endregion

  
        #region Fields

        #region Menu

        private bool _BrokeCard_Menu_Vissabillity;
        public bool BrokeVissability
        {
            get => _BrokeCard_Menu_Vissabillity;
            set => Set(ref _BrokeCard_Menu_Vissabillity, value);
        }


        private bool _CashBack_Vissibillity = true;
        public bool CashBack_Vissibillity
        {
            get => _CashBack_Vissibillity;
            set => Set(ref _CashBack_Vissibillity, value);
        }

        private bool _IsAutoRepayTheLoan;
        public bool IsAutoRepayTheLoan
        {
            get => _IsAutoRepayTheLoan;
            set => Set(ref _IsAutoRepayTheLoan, value);
        } 

        #endregion

        #region Transaction


        private bool _IsTransaction = false;
        public bool IsTransaction 
        {
            get => _IsTransaction;
            set {
                if (value == true)
                {
                    IsCardHistoryMenu_Visiblle = false;
                }

                Set(ref _IsTransaction, value);
            
            }
        }


        private bool _IsTransactionValid = false;
        public bool IsTransactionValid 
        {
            get => _IsTransactionValid;
            set {
                
                if (!IsTransaction_Shoping && value && (TransactionCard == null ^ TransactionValue == ""))return;
                if (IsTransaction_ByPhoneNumber & TransactionCard == null) return;

                Set(ref _IsTransactionValid, value); 
            }
        }

        private bool _IsTransaction_ThroughOurCards = false;
        public bool IsTransaction_ThroughOurCards
        {
            get => _IsTransaction_ThroughOurCards;
            set {
                if (value)
                {
                    TransactionClient = Selected_Client;

                    IsTransaction_Shoping = false;
                    IsTransaction_ByPhoneNumber = false;
                }
               
                Set(ref _IsTransaction_ThroughOurCards, value);
            }
        }

        private bool _IsTransaction_ByPhoneNumber;
        public bool IsTransaction_ByPhoneNumber
        {
            get => _IsTransaction_ByPhoneNumber;
            set
            {
                if (value)
                {
                    IsTransaction_Shoping = false;
                    IsTransaction_ThroughOurCards = false;
                }
                
                Set(ref _IsTransaction_ByPhoneNumber, value);
            }
        }

        private bool _IsTransaction_Shoping = false;
        public bool IsTransaction_Shoping
        {
            get => _IsTransaction_Shoping;
            set
            {
                if (value)
                {
                    TransactionClient = Shop;
                    TransactionCard = Shop.MyDebitCards[0];

                    IsTransaction_ThroughOurCards = false;
                    IsTransaction_ByPhoneNumber = false;
                }
                
                Set(ref _IsTransaction_Shoping, value);
            }
        }

        private string? _Selected_WayOfTransaction;
        public string? Selected_WayOfTransaction
        {
            get => _Selected_WayOfTransaction;
            set
            {
                
                IsTransaction = true;


                switch (value)
                {
                    case "Между своими счетами":
                        IsTransaction_ThroughOurCards = true;
                        break;

                    case "По номеру телефона (нет)":
                        IsTransaction_ByPhoneNumber = true;
                        break;

                    case "Совершить покупку":
                        IsTransaction_Shoping = true;
                        break;
                    default:
                        ControlAllTransactionMenuItems_Vissibility(false);
                        IsTransaction = false;
                        break;
                }
                Set(ref _Selected_WayOfTransaction, value);
            }
        }


        private string _Transaction_CashBackType_Header = "Тип покупки...";
        public string Transaction_CashBackType_Header
        {
            get => _Transaction_CashBackType_Header;
            set => Set(ref _Transaction_CashBackType_Header, value);
        }


        private string _Selected_WayOfTransaction_Header = "Перевод средств...";
        public string Selected_WayOfTransaction_Header
        {
            get => _Selected_WayOfTransaction_Header;
            set => Set(ref _Selected_WayOfTransaction_Header, value);
        }

        private double _TransactionValue_WithComission;
        public double TransactionValue_WithComission
        {
            get => _TransactionValue_WithComission;
            set {
                if (TransactionValue != "" && TransactionClient != null && TransactionClient.Status.GetType() == typeof(V_I_P))
                {
                    value = Convert.ToDouble(TransactionValue);
                    
                }
                Set(ref _TransactionValue_WithComission, value); }
        }

        private double _TransactionPrecent;
        public double TransactionPrecent
        {
            get => _TransactionPrecent;
            set {
                if (TransactionValue != "")
                {
                    double cash = Convert.ToDouble(TransactionValue);

                    TransactionValue_WithComission = cash - cash * (value / 100);

                }
                Set(ref _TransactionPrecent, value); 
            }
                    
            
        }

        private EventData? _Selected_EventForBuy = null;
        public EventData? Selected_EventForBuy
        {
            get => _Selected_EventForBuy;
            set =>Set(ref _Selected_EventForBuy, value);

        }

        private Client? _TransactionClient;
        public Client? TransactionClient
        {
            get => _TransactionClient;
            set {
               
                Set(ref _TransactionClient, value);
                
                if (value != null)
                {

                    TransactionPrecent = Selected_Client!.Status.Commission;
                    
                }
            }
        }


        private BankObjects.CardPrefab.Card? _TransactionCard;
        public BankObjects.CardPrefab.Card? TransactionCard
        {
            get => _TransactionCard;
            set
            { 
                Set(ref _TransactionCard, value);
                if (value != null) IsTransactionValid = true;
                else IsTransactionValid = false;
            }
        }


        private string _TransactionValue = "";
        public string TransactionValue
        {
            get => _TransactionValue ?? "";
            set
            {
                if (value == "0" ^ !value.All(char.IsDigit) ^value.Length > 11 ) return;

                if (value == "")
                {
                    Set(ref _TransactionValue, value);
                    IsTransactionValid = false;
                    TransactionValue_WithComission = 0;
                    return;
                }

                Set(ref _TransactionValue, value);
                IsTransactionValid = true;
                TransactionValue_WithComission =
                    (Convert.ToDouble(TransactionValue) - (Convert.ToDouble(TransactionValue) *
                    (TransactionPrecent / 100)));


            }
        }

        #endregion


        #region Client


        private ObservableCollection<Client> _ClienList = new();
        public ObservableCollection<Client> ClienList
        {
            get => _ClienList;
            set => Set(ref _ClienList, value);
        }


        private Client? _Selected_Client;
        public Client? Selected_Client
        {
            get => _Selected_Client;
            set
            {
                if (value == null) IsClientSelected = false;

                else IsClientSelected = true;

                Set(ref _Selected_Client, value);
                TransactionPrecent = value!.Status.Commission;
                Selected_WayOfTransaction = null;
                IsCardSelected = false;

                ReversSelectedCard(TabChanged);
            }
        }

        private bool _IsClientSelected;
        public bool IsClientSelected
        {
            get => _IsClientSelected;
            set => Set(ref _IsClientSelected, value);
        }
       


 

        
        #endregion

        #region Debit

        private Debit? _Selected_DebitCard;
        public Debit? Selected_DebitCard
        {
            get => _Selected_DebitCard;
            set
            {

                if (value != null)
                {
                    IsCardSelected = true;
                    Selected_TypeOfCard_Text = value.CardId!;
                    Card_Comboboxes_Brush = Brushes.Black;
                }

                Set(ref _Selected_DebitCard, value);

            }
        }

        private bool _IsDebitSelected;
        public bool IsCardSelected
        {
            get => _IsDebitSelected;
            set => Set(ref _IsDebitSelected, value);
        }

        public ObservableCollection<string> TransferDebit { get; set; } = new() { 
            "Между своими счетами",
            "По номеру телефона (нет)",
            "Совершить покупку"
        };

        #endregion

        #region Credit


        #region New

        public NewCredit NewCredit { get; set; } = new();

        private bool _NewCredit_Menu_Visibility;
        public bool NewCredit_Menu_Visibility
        {
            get => _NewCredit_Menu_Visibility;
            set => Set(ref _NewCredit_Menu_Visibility, value);
        }

        #endregion

        #region Main

        private Credit? _Selected_CreditCard;
        public Credit? Selected_CreditCard
        {
            get => _Selected_CreditCard;
            set
            {

                if (value != null)
                {
                    IsCreditSelected = true;
                    Card_Comboboxes_Brush = Brushes.Black;
                }

                Set(ref _Selected_CreditCard, value);

            }
        }
        
        private bool _IsCreditSelected;
        public bool IsCreditSelected
        {
            get => _IsCreditSelected;
            set => Set(ref _IsCreditSelected, value);
        }

        public ObservableCollection<string> TransferCredit { get; set; } = new()
        {
            "Совершить покупку"
        };
        #endregion


        #endregion

        #region Investment

        #region New

        public NewInvestment NewInvest { get; set; } = new();


        private bool _NewInvest_Menu_Visibillity;
        public bool NewInvest_Menu_Visibillity
        {
            get => _NewInvest_Menu_Visibillity;
            set 
            {
                CashBack_Vissibillity = !value;
                Set(ref _NewInvest_Menu_Visibillity, value); 
            }
        }

        #endregion

        #region Main

        private Investment? _Selected_Investment;
        public Investment? Selected_Investment
        {
            get => _Selected_Investment;
            set
            {
                Invest_IsWithDrawCash_Visibility = false;

                if (value != null)
                {
                    IsInvestmentSelected = true;
                    Card_Comboboxes_Brush = Brushes.Black;
                    Invest_VisibilityControl(value);
                }

                Set(ref _Selected_Investment, value);


            }
        }

        private bool _IsInvestmentSelected;
        public bool IsInvestmentSelected
        {
            get => _IsInvestmentSelected;
            set => Set(ref _IsInvestmentSelected, value);
        }



        private bool _Invest_MenuActivate;
        public bool Invest_MenuActivate
        {
            get => _Invest_MenuActivate;
            set
            {
                if (value == true)
                {
                    Invest_ComboBoxTransferCard_Visibility = true;
                }

                Set(ref _Invest_MenuActivate, value);
            }
        }


        private string _Invest_DebitComboBox_Header = "";
        public string Invest_DebitComboBox_Header
        {
            get => _Invest_DebitComboBox_Header;
            set => Set(ref _Invest_DebitComboBox_Header, value);
        }


        private bool _IsCanOutput;
        public bool IsCanOutput
        {
            get => _IsCanOutput;
            set => Set(ref _IsCanOutput, value);
        }

       
        private bool _Invest_ComboBoxTransferCard_Visibility;
        public bool Invest_ComboBoxTransferCard_Visibility
        {
            get => _Invest_ComboBoxTransferCard_Visibility;
            set
            {
                Set(ref _Invest_ComboBoxTransferCard_Visibility, value);
            }
        }


        private bool _Invest_IsWithDrawCash_Visibillity;
        public bool Invest_IsWithDrawCash_Visibility
        {
            get => _Invest_IsWithDrawCash_Visibillity;
            set {

                Invest_ComboBoxTransferCard_Visibility = value;
                Set(ref _Invest_IsWithDrawCash_Visibillity, value); 
            }
        }


    

        #endregion

        #endregion




        #region More

        private Client Shop;


        private int _TabChanged;
        public int TabChanged
        {
            get => _TabChanged;

            set
            {
                IsCreditSelected = false;
                IsCardSelected = false;
                IsInvestmentSelected = false;
                Selected_WayOfTransaction = null;

                ControlAllTransactionMenuItems_Vissibility(false);
                Invest_CloseAllWindows();

                TransactionValue = "";

                Set(ref _TabChanged, value);
                
                ReversSelectedCard(value);
            }
        }



        private Brush? _Card_Comboboxes_Brush;
        public Brush? Card_Comboboxes_Brush
        {
            get => _Card_Comboboxes_Brush;
            set => Set(ref _Card_Comboboxes_Brush, value);
        }


        private string _Selected_TypeOfCard_Text = "Выберите карту...";
        public string Selected_TypeOfCard_Text
        {
            get => _Selected_TypeOfCard_Text;
            set => Set(ref _Selected_TypeOfCard_Text, value);
        }

        private string _CardHistoryMenu_Text = "";
        public string CardHistoryMenu_Text
        {
            get => _CardHistoryMenu_Text;
            set => Set(ref _CardHistoryMenu_Text, value);
        }


        private string _Info_Text = "";
        public string Info_Text
        {
            get => _Info_Text;
            set => Set(ref _Info_Text, value);
        }


        private bool _IsCardHistoryMenu_Visible = true; 
        public bool IsCardHistoryMenu_Visiblle
        {
            get => _IsCardHistoryMenu_Visible;
            set {
                if (value)
                {
                    CardHistoryMenu_Text = "Скрыть историю";

                    ControlAllTransactionMenuItems_Vissibility(false);

                    if (IsInfo_Visible)
                    {
                        IsInfo_Visible = false;
                    }
                    
                }
                else
                {
                    CardHistoryMenu_Text = "Показать историю";
                }
                Set(ref _IsCardHistoryMenu_Visible, value);
            }
        }

        private bool _IsInfo_Visible;
        public bool IsInfo_Visible
        {
            get => _IsInfo_Visible;
            set
            {
                if (value)
                {
                    Info_Text = "Окей";
                    if (IsCardHistoryMenu_Visiblle)
                    {
                        IsCardHistoryMenu_Visiblle = false;
                    }
                }
                else
                {
                    Info_Text = "Подробно";
                    
                }

                if (Selected_Investment != null)
                {
                    if (value) Invest_CloseAllWindows();

                    else Invest_VisibilityControl(Selected_Investment);
                }
                Set(ref _IsInfo_Visible, value);
            }

        }

        private ObservableCollection<EventData> _Events = new ObservableCollection<EventData>();

        public ObservableCollection<EventData> Events
        {
            get => _Events;
            set => Set(ref _Events, value);
        }


        #endregion


        #endregion


        #region Commands

  
        #region General

        public ICommand Info_VisibilityControl { get; }
        private void OnInfo_VisibilityControl(object p)
        {
            IsInfo_Visible = !IsInfo_Visible;
        }

        public ICommand History_VisibilityControl { get; }
        private void OnHistory_VisibilityControl(object p)
        {
            IsCardHistoryMenu_Visiblle = !IsCardHistoryMenu_Visiblle;
        }

        #endregion

        #region UserMenu

        public ICommand AddClient { get; }
        private void OnAddClient(object p)
        {
            Client newClient = new(BankEvents);
            ClienList.Add(newClient);
            AccountCreated?.Invoke(newClient);
        }

       
        public ICommand TimeSkip { get; }
        private void OnTimeSkip(object p)
        {
            foreach (var client in ClienList)
            {
                if (IsAutoRepayTheLoan)
                {
                    foreach (Credit credit in client.MyCreditCards)
                    {
                        if (!credit.IsReady)
                        {
                            credit.Debt = credit.MonthDebt * -1;
                        } 
                    }
                }

                client.UpDate();
            }

            Invest_CloseAllWindows();
            CardsUpDate();
        }
         
        public ICommand IncreasBalance { get; }
        private void OnIncreasBalance(object p)
        {
            BankEvents.IncreasBalance((DeterminesWhoCardIs() as Debit)!, 100000);
        }


        private SaveController _saveController = new();
        public ICommand SaveClient { get; }
        private void OnSaveClient(object p)
        {
            _saveController.Mode = new KeeperJson();
            _saveController.Mode.SaveSelectedClient(Selected_Client!);

            _saveController.Mode = new KeeperXML();
            _saveController.Mode.SaveSelectedClient(Selected_Client!);
        }

        #endregion

        #region Transaction

        public ICommand TransactionAccept { get; }
        private void OnTransactionAccept(object p)
        {
            double sum = Convert.ToDouble(TransactionValue);
            double commission = TransactionValue_WithComission;
            double cashBack = 0;

            //Для каждого вида транзакций применяются свои условия,
            //а результат в виде сообщения о транзакции один на все случаи.
            //
            //При переводе
            //Другому клиенту - ничего не применяется
            //между своими счетами - снимается коммиссия
            //магазину - снимается коммиссия и применяется кешбкек
            if (IsTransaction_ByPhoneNumber == true)
            {
                IsTransaction_ByPhoneNumber = false;
            }

            else if (IsTransaction_ThroughOurCards == true)
            {
                commission = sum;
                IsTransaction_ThroughOurCards = false;
            }

            else if (IsTransaction_Shoping == true)
            {
                commission = sum;
                if (EventData.EventUsing(Selected_Client!.MyEvents, Selected_EventForBuy!) == true)
                {
                    cashBack = sum * (Selected_EventForBuy!.Discount / 100);
                }

                IsTransaction_Shoping = false;
                Transaction_CashBackType_Header = "Тип покупки...";
                Selected_EventForBuy = null;
            }



            //Создается экземпляр информации о транзакции
            TransactionMessage transactionMessage = new( "Отправлено", sum,
                    Selected_Client!, DeterminesWhoCardIs()!, TransactionClient!,
                    TransactionCard!, cashBack, commission);

            //Вызывается метод транзакции у отправителя
            //Selected_Client!.TransatcionSender(transactionMessage);
            DeterminesWhoCardIs()!.TransactionSend(transactionMessage);

            ControlAllTransactionMenuItems_Vissibility(false);
            TransactionValue = "";
            Selected_WayOfTransaction = null;
            Selected_WayOfTransaction_Header = "Перевод средств...";
            IsTransaction = false;
            IsTransactionValid = false;

        }

        public ICommand CloseDebitTransactionMenu { get; }
        private void OnCloseDebitTransactionMenu(object p){

            Selected_WayOfTransaction_Header = "Перевод средств";
            TransactionCard = null;  
            Selected_WayOfTransaction = null;
            TransactionClient = null;
            Selected_EventForBuy = null;
            TransactionValue = "";
            ControlAllTransactionMenuItems_Vissibility(false);
        }


        #endregion

        #region Chapter Menu

        #region Add Debit

        public ICommand AddDebit { get; }
        private void OnAddDebit(object p)
        {
            NewInvest_Menu_Visibillity = false;
            NewCredit_Menu_Visibility = false;
            Debit newdeDit = new(Selected_Client!.Name, Selected_Client.AccountID, BankEvents);
            
            Selected_Client!.AddDebitCard( newdeDit);

        }



        #endregion

        #region Add Credit

        public ICommand AddCredit { get; }
        private void OnAddCredit(object p)
        {
            if (NewCredit_Menu_Visibility)
            {
                NewCredit_Menu_Visibility = false;
                return;
            }

            NewInvest_Menu_Visibillity = false;
            BrokeVissability = false;

            NewCredit.NewCreditCorrector(NewCredit.Value, 10000, 500000, Selected_Client!.Competence.CreditPercent,10000);
            NewCredit.MonthCount = 3;

            NewCredit_Menu_Visibility = true;
        }

        public ICommand Accept_AddCredit { get; }
        private void OnAccept_AddCredit(object p)
        {
            Credit newCredit = new(NewCredit.Value, NewCredit.MonthDebt,
                NewCredit.MonthCount, NewCredit.Precent,
                Selected_Client!.Name, Selected_Client.AccountID, BankEvents);

            
            Selected_Client!.AddCreditCard( newCredit);

            NewCredit_Menu_Visibility = false;
        }

        #endregion

        #region  Broke Card
        public ICommand AcceptBrokeVissability { get; }
        private void OnAcceptBrokeVissability(object p)
        {
            if (BrokeVissability)
            {
                BrokeVissability = false;
                return;
            }

            NewCredit_Menu_Visibility = false;
            NewInvest_Menu_Visibillity = false;

            BrokeVissability = true;
        }

        public ICommand BrokeCard { get; }
        private void OnBrokeCard(object p)
        {
            MessageBoxResult result = DebitController.DestroyDebit(Selected_DebitCard!.Balance, Selected_DebitCard.CashBack);

            if (result == MessageBoxResult.No)
            {
                BrokeVissability = false;
                return;
            }

            CardClosed?.Invoke("дебетовая карта", Selected_Client!, Selected_DebitCard);
            Selected_Client!.MyDebitCards.Remove(Selected_DebitCard!);
            BrokeVissability = false;
        }

        #endregion

        #region Add Investment
        public ICommand AddInvestment { get; }
        private void OnAddInvestment(object p)
        {
            if (NewInvest_Menu_Visibillity)
            {
                NewInvest_Menu_Visibillity = false;
                return;
            }

            NewCredit_Menu_Visibility = false;
            BrokeVissability = false;

            NewInvest.Precent = Selected_Client!.Competence.InvestPrecent;
            NewInvest.Value = 30000;
            NewInvest.MounthCount = 3;
            

            NewInvest_Menu_Visibillity = true;
        }


        public ICommand AcceptAddInvestment { get; }
        private void OnAcceptAddInvestment(object p)
        {
            Investment invest = new(NewInvest.Value, NewInvest.Precent,
                NewInvest.ClearProfit, NewInvest.MounthCount, NewInvest.IsAccumulation,
                Selected_Client!.Name, Selected_Client.AccountID, BankEvents);

           

            Selected_Client!.AddInvestment(invest);

            MessageBox.Show("Новый вклад оформлен! \nЧтобы он активировался, вам нужно пополнить его в меню \"Вклады\" в теченнии месяца.",
                "Поздравляем!", MessageBoxButton.OK, MessageBoxImage.Information);

            NewInvest_Menu_Visibillity = false;
        }
  
        #endregion

        #endregion

        #region Chapter Investment

        public ICommand WithDrawCash { get; }
        private void OnWithDrawCash(object p)
        {
            Invest_IsWithDrawCash_Visibility = !Invest_IsWithDrawCash_Visibility;
        }

        public ICommand InvestmentFinish { get; }
        private void OnInvestmentFinish(object p)
        {
            Invest_IsWithDrawCash_Visibility = false;

            TransactionMessage transactionMessage = new("Отправлено",0,Selected_Client!, Selected_Investment!, 
                Selected_Client!, TransactionCard!, 0 ,0);

            Selected_Investment!.TransactionSend(transactionMessage);

            if (Selected_Investment.isReady)
            {
                 Selected_Client!.MyInvestments.Remove(Selected_Investment);
            }
           

        }

        public ICommand Activate_Investment { get; }
        private void OnActivate_Investment(object p)
        {

            //Создается экземпляр информации о транзакции
            TransactionMessage transactionMessage = new("Отправлено", Selected_Investment!.StartBalance,
                    Selected_Client!, TransactionCard!, Selected_Client!,
                    Selected_Investment!, 0 ,Selected_Investment!.StartBalance);

            //Вызывается метод транзакции у отправителя
            TransactionCard!.TransactionSend(transactionMessage);

            ControlAllTransactionMenuItems_Vissibility(false);
            Selected_Investment.ActivateInvest();

            Invest_VisibilityControl(Selected_Investment);
            Invest_ComboBoxTransferCard_Visibility = false;
            Invest_MenuActivate = false;
            
        }

        #endregion


        #endregion

        #region Command Enable
        private bool CanAnyWay(object p) => true;
        private bool CanIfValid(object p) => IsTransactionValid;

        private bool CanIfDebitSelected(object p) => _IsDebitSelected;
        private bool CanIfClientSelected(object p) => IsClientSelected;

        #endregion


        #region Events

        private BankEvents _BankEvents = new(BankSystemSQLServer_Path);
        public BankEvents BankEvents
        {
            get => _BankEvents;
            set => Set(ref _BankEvents, value);
        }


        private event Action<Client>? AccountCreated;

        private event Action<string,Client, Card>? CardClosed;

        #endregion


        private static readonly SqlConnectionStringBuilder BankSystemSQLServer_Path = new()
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            InitialCatalog = "BankSystemEFDataBase",
            IntegratedSecurity = true,
            MultipleActiveResultSets = true
        };

        public MainWindowViewModel() {


            CardClosed += BankEvents.CardDestroy;
            AccountCreated += BankEvents.NewClient;

            ClienList = BankEvents.GetClientFromDataBase();


            Shop = new( "Магазин", new Entity(), BankEvents);
            Shop.MyDebitCards.Add(new Debit("Shop", Shop.AccountID, BankEvents));
            
            AddInvestment = new LamdaCommand(OnAddInvestment, CanIfClientSelected);
            AddDebit = new LamdaCommand(OnAddDebit, CanIfClientSelected);
            AddCredit = new LamdaCommand(OnAddCredit, CanIfClientSelected);
            AcceptBrokeVissability = new LamdaCommand(OnAcceptBrokeVissability, CanIfClientSelected);

            BrokeCard = new LamdaCommand(OnBrokeCard, CanAnyWay);
            InvestmentFinish = new LamdaCommand(OnInvestmentFinish, CanAnyWay);
            WithDrawCash = new LamdaCommand(OnWithDrawCash, CanAnyWay);
            TimeSkip = new LamdaCommand(OnTimeSkip, CanAnyWay);
            TransactionAccept = new LamdaCommand(OnTransactionAccept, CanIfValid);
            AddClient = new LamdaCommand(OnAddClient, CanAnyWay);
            AcceptAddInvestment = new LamdaCommand(OnAcceptAddInvestment, CanAnyWay);
            Accept_AddCredit = new LamdaCommand(OnAccept_AddCredit, CanAnyWay);
            Activate_Investment = new LamdaCommand(OnActivate_Investment, CanAnyWay);
            CloseDebitTransactionMenu = new LamdaCommand(OnCloseDebitTransactionMenu, CanAnyWay);
            Info_VisibilityControl = new LamdaCommand(OnInfo_VisibilityControl, CanAnyWay);
            History_VisibilityControl = new LamdaCommand(OnHistory_VisibilityControl, CanAnyWay);
            IncreasBalance = new LamdaCommand(OnIncreasBalance, CanIfDebitSelected);
            SaveClient = new LamdaCommand(OnSaveClient, CanIfClientSelected);

            Events = EventData.GetEvents();
            Events.Add(new EventData("Другое",0));


        }

    }
}
