﻿using System;

namespace MuslimFashion.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customer { get; }
        IMenuRepository Menu { get; }
        IProductRepository product { get; }

        ISubMenuRepository SubMenu { get; }
        ISizeRepository Size { get; }
        int SaveChanges();
    }
}