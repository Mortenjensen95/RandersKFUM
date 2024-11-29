using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Repository
{
    //Interface (skabelon) 
    public interface IRepository<t> where t : class
    {
        IEnumerable<t> GetAll();
        t GetById(int id);
        void Add(t entity);
        void Update(t entity);
        void Delete(int id);
    }
}
