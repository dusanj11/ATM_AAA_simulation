using System.Runtime.Serialization;

namespace Modeli
{
    [DataContract]
    public class User
    {
        private string username;
        private string pin;
        private double balance;
        private bool isAuthenticated;

        public User()
        {
            this.username = null;
            this.pin = null;
            this.balance = 0;
            this.isAuthenticated = false;
        }

        public User(string username, string pin, double balance)
        {
            this.username = username;
            this.pin = pin;
            this.balance = balance;
            this.isAuthenticated = false;
        }

        [DataMember]
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        [DataMember]
        public string Pin
        {
            get
            {
                return pin;
            }
            set
            {
                pin = value;
            }
        }

        [DataMember]
        public double Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        [DataMember]
        public bool IsAuthenticated
        {
            get
            {
                return isAuthenticated;
            }
            set
            {
                isAuthenticated = value;
            }
        }
    }
}