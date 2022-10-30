using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Customer_Client.Logic
{
    public class UserInfo
    {
        public Guid customerID { get; set; }
        public string UserName { get; set; }

        private static string _saveLocation = @"SaveData\CustomerData.json";
        private static UserInfo _instance;
        public static UserInfo Instance { get => _instance; }

        public UserInfo(Guid customerID, string userName)
        {
            this.customerID = customerID;
            this.UserName = userName;
            _instance = this;
        }

        public static bool LoadUserInfo()
        {
            _instance = null;
            try
            {
                _instance = IO.ReadObjectFromFile<UserInfo>(_saveLocation);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            Trace.WriteLine($"Load: {_instance?.customerID != Guid.Empty}");
            return _instance?.customerID != Guid.Empty;
        }

        public static void Save()
        {
            IO.WriteFile(_saveLocation, JsonConvert.SerializeObject(_instance));
        }
        public static void Delete()
        {
            IO.DeleteFile(_saveLocation);
        }
    }
}
