﻿using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            category = new CategoryRepository(_db);
            coverType = new CoverTypeRepository(_db);
            product = new ProductRepository(_db);
            company = new CompanyRepository(_db);
            applicationUser = new ApplicationUserRepository(_db);
            SP_Call = new SP_Call(_db);
        }

        public ICategoryRepository category { get; private set; }
        public ICoverTypeRepository coverType { get; private set; }
        public IProductRepository product { get; private set; }
        public ICompanyRepository company { get; private set; }
        public IApplicationUserRepository applicationUser { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
