using System;
using Team;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Items
{
    /// <summary>
    /// This component is responsible for managing the floor shop.
    /// </summary>
    public class FloorController : MonoBehaviour
    {
        public int Price = 100000;
        public float Multiplier = 1.2f;
        public Text Bought;
        public Text Available;
        public Text PriceText;
        public Button Button;
        public IntEvent MoneyChangedEvent;

        private Bank bank;
        private TeamManager team;
        private UnityAction<int> moneyChangedAction;

        private void Awake()
        {
            bank = ContentHub.Instance.bank;
            team = TeamManager.Instance;

            moneyChangedAction += BalanceChanged;
            MoneyChangedEvent.AddListener(moneyChangedAction);
            BalanceChanged(bank.Balance);
            
            Button.onClick.AddListener(ButtonClicked);

            Bought.text = team.GetFloors().ToString();
            Available.text = team.MaxFloors.ToString();
            PriceText.text = CalculatePrice().ToString();
            
            if (team.GetFloors() >= team.MaxFloors)
            {
                Button.interactable = false;
            }
        }

        private void ButtonClicked()
        {
            var price = CalculatePrice();
            if (team.GetFloors() < team.MaxFloors && bank.Pay(price))
            {
                team.AddFloor();
                PriceText.text = CalculatePrice().ToString();
            }

            if (team.GetFloors() >= team.MaxFloors)
            {
                Button.interactable = false;
            }
            
            Bought.text = team.GetFloors().ToString();
        }

        private void BalanceChanged(int balance)
        {
            if (balance > CalculatePrice() && team.GetFloors() < team.MaxFloors)
            {
                Button.interactable = true;
            }
            else
            {
                Button.interactable = false;
            }
        }

        private int CalculatePrice()
        {
            return (int) (Price * Math.Pow(Multiplier, team.GetFloors() - 1));
        }
    }
}