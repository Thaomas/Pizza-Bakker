using Pizza_Client.Commands;
using Pizza_Client.Stores;
using Shared;
using System;
using System.Windows.Input;

namespace Pizza_Client.ViewModels
{
    class HomepageViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        private Array _allStatus;
        public Array AllStatus
        {
            get => _allStatus;
            set
            {
                _allStatus = value;
                OnPropertyChanged(nameof(AllStatus));
            }
        }

        private OrderStatus _status;
        public OrderStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private string _noteTitle;
        public string NoteTitle
        {
            get => _noteTitle;
            set
            {
                _noteTitle = value;
                OnPropertyChanged(nameof(NoteTitle));
            }
        }

        private string _noteText;
        public string NoteText
        {
            get => _noteText;
            set
            {
                _noteText = value;
                OnPropertyChanged(nameof(NoteText));
            }
        }

        public ICommand ChangeStatusCommand { get; }


        public HomepageViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            AllStatus = Enum.GetValues(typeof(OrderStatus));

            ChangeStatusCommand = new ChangeStatusCommand(_navigationStore);
        }
    }
}