﻿using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ICustomerRepository
    {
        Task<Customer> SaveCustomer(Customer entity);

        Task<Customer> FindCustomerById(long Id);
        Task<Customer> FindCustomerByName(string name);

        Task<IEnumerable<Customer>> FindAllCustomer();

        Task<Customer> UpdateCustomer(Customer entity);
        Task<bool> DeleteCustomer(Customer entity);
    }
}
