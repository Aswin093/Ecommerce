using Ecommerce.Model;
using System.Collections.Generic;

namespace Ecommerce.Interface
{
    public interface ICustomerAddressRepository
    {
        List<CustomerAddress> GetCustomerAddresses(int customerID);
        CustomerAddress GetCustomerAddress(int addressID);
        bool CreateCustomerAddress(CustomerAddress customerAddress);
        bool UpdateCustomerAddress(int addressID, CustomerAddress customerAddress);
        bool DeleteCustomerAddress(int addressID);
    }
}