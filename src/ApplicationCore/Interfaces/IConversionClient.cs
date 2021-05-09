using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IConversionClient<T> where T : IWebServicesEntity
    {
        Task<IEnumerable<T>> GetAll();
    }
}
