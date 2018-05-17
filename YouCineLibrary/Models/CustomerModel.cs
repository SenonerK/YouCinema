using System;

namespace YouCineLibrary.Models
{
    public class CustomerModel
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Credit { get; set; }

        public bool Update()
        {
            return Config.Connection.UpdateCustomer(ID, Email, FirstName, LastName, Credit);
        }
    }
}
