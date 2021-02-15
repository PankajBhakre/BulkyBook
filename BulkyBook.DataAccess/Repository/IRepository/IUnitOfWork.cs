using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository category { get; }
        ICoverTypeRepository coverType { get; }

        IProductRepository product { get; }
        ICompanyRepository company { get; }

        IApplicationUserRepository applicationUser { get; }
        ISP_Call SP_Call { get; }

        void Save();
    }
}
