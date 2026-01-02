using DEMO_Shop.Data;
using DEMO_Shop.Models;

namespace DEMO_Shop.Services
{
    public class CartService
    {
        private readonly AppDbContext _db;

        public CartService(AppDbContext db)
        {
            _db = db;
        }

        public Cart GetOrCreateCart(string sessionId, int? userId)
        {
            var cart = _db.Carts.FirstOrDefault(x =>
                x.SessionId == sessionId || (userId != null && x.UserId == userId));

            if (cart == null)
            {
                cart = new Cart
                {
                    SessionId = sessionId,
                    UserId = userId
                };
                _db.Carts.Add(cart);
                _db.SaveChanges();
            }

            return cart;
        }

        public void AddItem(string sessionId, int productId, int? userId)
        {
            var cart = GetOrCreateCart(sessionId, userId);

            var item = _db.CartItems
                .FirstOrDefault(x => x.CartId == cart.Id && x.ProductId == productId);

            if (item == null)
            {
                _db.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            _db.SaveChanges();
        }

        public List<CartItem> GetItems(string sessionId, int? userId)
        {
            var cart = _db.Carts.FirstOrDefault(x =>
                x.SessionId == sessionId || (userId != null && x.UserId == userId));

            if (cart == null) return new List<CartItem>();

            return _db.CartItems.Where(x => x.CartId == cart.Id).ToList();
        }

        public void MergeCart(string sessionId, int userId)
        {
            var guestCart = _db.Carts.FirstOrDefault(x => x.SessionId == sessionId);
            var userCart = _db.Carts.FirstOrDefault(x => x.UserId == userId);

            if (guestCart == null) return;

            if (userCart == null)
            {
                guestCart.UserId = userId;
                guestCart.SessionId = null;
                _db.SaveChanges();
                return;
            }

            var guestItems = _db.CartItems
                .Where(x => x.CartId == guestCart.Id)
                .ToList();

            foreach (var item in guestItems)
            {
                var exist = _db.CartItems.FirstOrDefault(x =>
                    x.CartId == userCart.Id &&
                    x.ProductId == item.ProductId);

                if (exist == null)
                {
                    item.CartId = userCart.Id;
                }
                else
                {
                    exist.Quantity += item.Quantity;
                    _db.CartItems.Remove(item);
                }
            }

            _db.Carts.Remove(guestCart);
            _db.SaveChanges();
        }

        public void UpdateQuantity(int cartItemId, int quantity)
        {
            var item = _db.CartItems.Find(cartItemId);
            if (item == null) return;

            if (quantity <= 0)
                _db.CartItems.Remove(item);
            else
                item.Quantity = quantity;

            _db.SaveChanges();
        }

        public decimal GetTotal(string sessionId, int? userId)
        {
            var cart = _db.Carts.FirstOrDefault(x =>
                x.SessionId == sessionId || x.UserId == userId);

            if (cart == null) return 0;

            return (from ci in _db.CartItems
                    join p in _db.Products on ci.ProductId equals p.Id
                    select ci.Quantity * p.Price).Sum();
        }


    }
}
