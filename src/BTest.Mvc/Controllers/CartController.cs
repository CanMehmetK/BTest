using BTest.Infrastructure;
using BTest.Mvc.Models;
using BTest.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BTest.Mvc.Controllers
{
  public class CartController : Controller
  {
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

      CartViewModel cartViewModel = new()
      {
        CartItems = cart,
        Total = cart.Sum(cartItem => cartItem.Quantity * cartItem.Price)
      };

      return View(cartViewModel);
    }

    public async Task<IActionResult> Add(Guid Id)
    {
      List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

      var product = await _context.Products.FindAsync(Id);
      if (product == null)
      {

      }

      var cartItem = cart.Where(cartItem => cartItem.Id == Id).FirstOrDefault();

      if (cartItem == null)
      {
        cart.Add(new CartItem(product));
      }
      else
      {
        cartItem.Quantity += 1;
      }


      HttpContext.Session.SetJson("Cart", cart);
      TempData["Success"] = "Product Added Succefully";

      return Redirect(Request.Headers["Referer"].ToString());
    }
    public IActionResult Decrease(Guid Id)
    {
      List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

      var cartItem = cart.Where(cartItem => cartItem.Id == Id).FirstOrDefault();
      if (cartItem.Quantity == 1) return RedirectToAction("Remove", new { Id = Id });
      else cartItem.Quantity -= 1;

      HttpContext.Session.SetJson("Cart", cart);
      TempData["Success"] = "Product Decreased Succefully";

      return Redirect(Request.Headers["Referer"].ToString());
    }
    public IActionResult Remove(Guid Id)
    {
      List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

      var cartItem = cart.Where(cartItem => cartItem.Id == Id).FirstOrDefault();
      cart.RemoveAll(cartItem => cartItem.Id == Id);

      HttpContext.Session.SetJson("Cart", cart);
      TempData["Success"] = "Product Removed Succefully";

      return RedirectToAction("Index");
    }
    public IActionResult Clear()
    {
      List<CartItem> cart = new List<CartItem>();

      HttpContext.Session.SetJson("Cart", cart);
      TempData["Success"] = "Cart Cleared Succefully";

      return Redirect(Request.Headers["Referer"].ToString());
    }
  }
}
