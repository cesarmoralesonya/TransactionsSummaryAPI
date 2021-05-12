﻿using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface IConversionClient<T> where T : IWebServiceModel
    {
        Task<IEnumerable<T>> GetAll();
    }
}
