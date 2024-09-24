using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Data.Entity;
using Repository.Helper.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class UnitOfWork : IDisposable
    {
        private KoiFarmDbContext _context;
        private GenericRepository<Product> _product;
        private GenericRepository<ProductItem> _productitem;
        private GenericRepository<Role> _role;
        private GenericRepository<User> _user;
        private GenericRepository<Blog> _blog;
        private GenericRepository<UserRefreshToken> _userrefreshtoken;
        private GenericRepository<Cart> _cart;
        private GenericRepository<CartItem> _cartItem;


        private bool disposed = false;

        public UnitOfWork(KoiFarmDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Blog> BlogRepository
        {
            get
            {
                if (_blog == null)
                {
                    _blog = new GenericRepository<Blog>(_context);
                }
                return _blog;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (_product == null)
                {
                    _product = new GenericRepository<Product>(_context);
                }
                return _product;
            }
        }

        public GenericRepository<ProductItem> ProductItemRepository
        {
            get
            {
                if (_productitem == null)
                {
                    _productitem = new GenericRepository<ProductItem>(_context);
                }
                return _productitem;
            }
        }

        public GenericRepository<Role> RoleRepository
        {
            get
            {
                if (_role == null)
                {
                    _role = new GenericRepository<Role>(_context);
                }
                return _role;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (_user == null)
                {
                    _user = new GenericRepository<User>(_context);
                }
                return _user;
            }
        }

        public GenericRepository<UserRefreshToken> UserRefreshTokenRepository
        {
            get
            {
                if (_userrefreshtoken == null)
                {
                    _userrefreshtoken = new GenericRepository<UserRefreshToken>(_context);
                }
                return _userrefreshtoken;
            }
        }

        public GenericRepository<Cart> CartRepository
        {
            get
            {
                if (_cart == null)
                {
                    _cart = new GenericRepository<Cart>(_context);
                }
                return _cart;
            }
        }

        public GenericRepository<CartItem> CartItemRepository
        {
            get
            {
                if (_cartItem == null)
                {
                    _cartItem = new GenericRepository<CartItem>(_context);
                }
                return _cartItem;
            }
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
