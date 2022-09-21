using Newtonsoft.Json.Linq;
using REI.Stores;
using REI.Util;
using REI.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REI.Commands
{
    public class AddFileCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public AddFileCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            connectionHandler.SendData(LoginCallback, new JsonFile()
            {
                StatusCode = (int)StatusCodes.OK,
                OppCode = (int)OperationCodes.SAVE_NOTE,
                ID = connectionHandler.ID.Value,
                Data = new JsonData
                {
                    Title = ((HomepageViewModel)_navigationStore.CurrentViewModel).NoteTitle,
                    Content = ((HomepageViewModel)_navigationStore.CurrentViewModel).NoteText
                }
            });
        }

        private void LoginCallback(JObject obj)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadFileCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public ReadFileCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            connectionHandler.SendData(LoginCallback, new JsonFile()
            {
                StatusCode = (int)StatusCodes.OK,
                OppCode = (int)OperationCodes.GET_NOTE,
                ID = connectionHandler.ID.Value,
                Data = new JsonData
                {
                    Title = ((HomepageViewModel)_navigationStore.CurrentViewModel).NoteTitle
                }
            });
        }

        private void LoginCallback(JObject obj)
        {
            ((HomepageViewModel)_navigationStore.CurrentViewModel).NoteText = obj["Data"]["Content"].ToObject<string>();
        }
    }

    public class ChangeStatusCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public ChangeStatusCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            connectionHandler.SendData(LoginCallback, new JsonFile()
            {
                StatusCode = (int)StatusCodes.OK,
                OppCode = (int)OperationCodes.CHANGE_STATUS,
                ID = connectionHandler.ID.Value,
                Data = new JsonData
                {
                    EmployeeStatus = (int)((HomepageViewModel)_navigationStore.CurrentViewModel).Status
                }
            });
        }

        private void LoginCallback(JObject obj)
        {
            throw new NotImplementedException();
        }
    }

    public class DeleteNote : CommandBase {
        private readonly NavigationStore _navigationStore;

        public DeleteNote(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter) {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();

            //int tt = (int)((HomepageViewModel)_navigationStore.CurrentViewModel).Status;

            connectionHandler.SendData(LoginCallback, new JsonFile()
            {
                StatusCode = (int)StatusCodes.OK,
                OppCode = (int)OperationCodes.DELETE_NOTE,
                ID = connectionHandler.ID.Value,
                Data = new JsonData
                {
                    PrivacyLevel = PrivacyLevel.SELECTED_PEOPLE
                }
            }) ;
        }

        private void LoginCallback(JObject obj) {
            throw new NotImplementedException();

        }
    }


}
