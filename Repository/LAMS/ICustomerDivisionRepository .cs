﻿using HaloBiz.Model.LAMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Repository
{
    public interface ICustomerDivisionRepository
    {
        Task<CustomerDivision> SaveCustomerDivision(CustomerDivision entity);

        Task<CustomerDivision> FindCustomerDivisionById(long Id);
        Task<CustomerDivision> FindCustomerDivisionByName(string name);

        Task<IEnumerable<CustomerDivision>> FindAllCustomerDivision();

        Task<CustomerDivision> UpdateCustomerDivision(CustomerDivision entity);
        Task<bool> DeleteCustomerDivision(CustomerDivision entity);
    }
}