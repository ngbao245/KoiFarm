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
        private GenericRepository<Review> _review;
        private GenericRepository<Order> _order;
        private GenericRepository<OrderItem> _orderItem;
        private GenericRepository<Promotion> _promotion;
        private GenericRepository<Payment> _payment;
        private GenericRepository<Consignment> _consignment;
        private GenericRepository<ConsignmentItems> _consignmentItem;
        private GenericRepository<Certificate> _certificate;
        private GenericRepository<ProductCertificate> _productcertificate;
        private GenericRepository<Batch> _batch;


        private bool disposed = false;

        public UnitOfWork(KoiFarmDbContext context)
        {
            _context = context;
        }


        public GenericRepository<Payment> PaymentRepository
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new GenericRepository<Payment>(_context);
                }
                return _payment;
            }
        }

        public GenericRepository<Promotion> PromotionRepository
        {
            get
            {
                if (_promotion == null)
                {
                    _promotion = new GenericRepository<Promotion>(_context);
                }
                return _promotion;
            }
        }

        public GenericRepository<Review> ReviewRepository
        {
            get
            {
                if (_review == null)
                {
                    _review = new GenericRepository<Review>(_context);
                }
                return _review;
            }
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

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (_order == null)
                {
                    _order = new GenericRepository<Order>(_context);
                }
                return _order;
            }
        }

        public GenericRepository<OrderItem> OrderItemRepository
        {
            get
            {
                if (_orderItem == null)
                {
                    _orderItem = new GenericRepository<OrderItem>(_context);
                }
                return _orderItem;
            }
        }

        public GenericRepository<Consignment> ConsignmentRepository
        {
            get
            {
                if (_consignment == null)
                {
                    _consignment = new GenericRepository<Consignment>(_context);
                }
                return _consignment;
            }
        }

        public GenericRepository<ConsignmentItems> ConsignmentItemRepository
        {
            get
            {
                if (_consignmentItem == null)
                {
                    _consignmentItem = new GenericRepository<ConsignmentItems>(_context);
                }
                return _consignmentItem;
            }
        }

        public GenericRepository<Certificate> CertificateRepository
        {
            get
            {
                if (_certificate == null)
                {
                    _certificate = new GenericRepository<Certificate>(_context);
                }
                return _certificate;
            }
        }

        public GenericRepository<ProductCertificate> ProductCertificateRepository
        {
            get
            {
                if (_productcertificate== null)
                {
                    _productcertificate = new GenericRepository<ProductCertificate>(_context);
                }
                return _productcertificate;
            }
        }

        public GenericRepository<Batch> BatchRepository
        {
            get
            {
                if (_batch == null)
                {
                    _batch = new GenericRepository<Batch>(_context);
                }
                return _batch;
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
