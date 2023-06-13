using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;

namespace diyabetik.dao.Abstract
{   
    // Ortak işlemler için interface
    public interface IDAO<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);
        Task<List<T>> GetAllByUid(string id);
        Task Delete(string id, string uid);
        Task<T> Add(T t);
        Task<T> Update(T t);
    }
}