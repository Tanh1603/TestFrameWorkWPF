using Microsoft.EntityFrameworkCore.Internal;
using NaviatePage.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.Models
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task<T> Create(T entity);

        Task<T> Update(int id, T entity);

        Task<bool> Delete(int id);

        //Task<(IEnumerable<T> items, int totalCount)> GetPaged(int pageNumber, int pageSize);

        //Task<IEnumerable<T>> SearchCustomer(string searchTerm);

        //Task<int> GetTotalCout();
    }
}