using System;
using Team;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Items
{
    public class WorkplaceController : MonoBehaviour
    {
        public int Price = 4000;
        public float Multiplier = 1.2f;
        public Text Bought;
        public Text Available;
        public Text PriceText;
        public Button Button;
        public IntEvent MoneyChangedEvent;
        public IntEvent FloorsChangedEvent;

        private Bank bank;
        private TeamManager team;
        private UnityAction<int> moneyChangedAction;
        private UnityAction<int> floorsChangedAction;

        private void Awake()
        {
            bank = ContentHub.Instance.bank;
            team = TeamManager.Instance;

            moneyChangedAction += BalanceChanged;
            MoneyChangedEvent.AddListener(moneyChangedAction);
            BalanceChanged(bank.Balance);

            floorsChangedAction += FloorsChanged;
            FloorsChangedEvent.AddListener(floorsChangedAction);
            FloorsChanged(0);
            
            Button.onClick.AddListener(ButtonClicked);

            Bought.text = team.GetWorkplaces().ToString();
            PriceText.text = CalculatePrice().ToString();
            
            if (team.GetWorkplaces() >= team.GetAvailableWorkplaces())
            {
                Button.interactable = false;
            }
        }

        private void ButtonClicked()
        {
            var price = CalculatePrice();
            if (team.GetWorkplaces() < team.GetAvailableWorkplaces() && bank.Pay(price))
            {
                team.AddWorkplace();
                PriceText.text = CalculatePrice().ToString();
            }

            if (team.GetWorkplaces() >= team.GetAvailableWorkplaces())
            {
                Button.interactable = false;
            }
            
            Bought.text = team.GetWorkplaces().ToString();
        }

        private void BalanceChanged(int balance)
        {
            if (balance > CalculatePrice() && team.GetWorkplaces() < team.GetAvailableWorkplaces())
            {
                Button.interactable = true;
            }
            else
            {
                Button.interactable = false;
            }
        }

        private void FloorsChanged(int floors)
        {
            Available.text = team.GetAvailableWorkplaces().ToString();
            BalanceChanged(bank.Balance);
        }

        private int CalculatePrice()
        {
            return (int) (Price * Math.Pow(Multiplier, team.GetWorkplaces() - 1));
        }
    }
}