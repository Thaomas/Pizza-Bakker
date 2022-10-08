﻿using Pizza_Client.Stores;
using Shared;

namespace Pizza_Client.ViewModels
{
    public class AddIngredientViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


        private string _ingredientName;
        public string IngredientName
        {
            get => _ingredientName;
            set
            {
                _ingredientName = value;
                OnPropertyChanged(nameof(IngredientName));
            }
        }

        private decimal _ingredientPrice;
        public decimal IngredientPrice
        {
            get => _ingredientPrice;
            set
            {
                _ingredientPrice = value;
                OnPropertyChanged(nameof(IngredientPrice));
            }
        }

        private string _ingredientAmount;
        public string IngredientAmount
        {
            get => _ingredientAmount;
            set
            {
                _ingredientAmount = value;
                OnPropertyChanged(nameof(IngredientAmount));
            }
        }


        public AddIngredientViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
