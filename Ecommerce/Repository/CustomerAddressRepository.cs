using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class CustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public CustomerAddressRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public List<CustomerAddress> GetCustomerAddresses(int customerID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerID)
            };

            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetCustomerAddresses, reader =>
            {
                List<CustomerAddress> addresses = new();
                while (reader.Read())
                {
                    addresses.Add(new CustomerAddress
                    {
                        AddressId = reader.GetInt32("AddressId"),
                        CustomerId = reader.GetInt32("CustomerId"),
                        Address = reader.GetString("Address"),
                        Name = reader.GetString("Name"),
                        PhoneNumber = reader.GetString("PhoneNumber")
                    });
                }
                return addresses;
            }, parameters);
        }

        public CustomerAddress GetCustomerAddress(int addressID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@AddressID", addressID)
            };

            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetCustomerAddress, reader =>
            {
                if (reader.Read())
                {
                    return new CustomerAddress
                    {
                        AddressId = reader.GetInt32("AddressId"),
                        CustomerId = reader.GetInt32("CustomerId"),
                        Address = reader.GetString("Address"),
                        Name = reader.GetString("Name"),
                        PhoneNumber = reader.GetString("PhoneNumber")
                    };
                }
                return null;
            }, parameters);
        }

        public bool CreateCustomerAddress(CustomerAddress customerAddress)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerAddress.CustomerId),
                new SqlParameter("@Address", customerAddress.Address),
                new SqlParameter("@Name", customerAddress.Name),
                new SqlParameter("@PhoneNumber", customerAddress.PhoneNumber)
            };

            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.CreateCustomerAddress, parameters);
        }

        public bool UpdateCustomerAddress(int addressID, CustomerAddress customerAddress)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@AddressID", addressID),
                new SqlParameter("@Address", customerAddress.Address),
                new SqlParameter("@Name", customerAddress.Name),
                new SqlParameter("@PhoneNumber", customerAddress.PhoneNumber)
            };

            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.UpdateCustomerAddress, parameters);
        }

        public bool DeleteCustomerAddress(int addressID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@AddressID", addressID)
            };

            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.DeleteCustomerAddress, parameters);
        }
    }
}