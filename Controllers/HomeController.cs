using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Project.Models;
using Project.ConnectionLogic;
using System.Collections.Generic;
using System.Text.Json;
using System.Data;

namespace Project
{
    public class HomeController : Controller
    {
        private readonly Repo _repo;
        private readonly HttpClient _httpClient;
        private static List<Product> _cart = new List<Product>();

        public HomeController(Repo repo, HttpClient httpClient)
        {
            _repo = repo;
            _httpClient = httpClient;
        }
        public async Task<IActionResult> FetchProducts()
        {
            Product prod = new Product();
            List<Product> temp = new List<Product>();
            var response = await _httpClient.GetFromJsonAsync<ProductApiResponse>("https://dummyjson.com/products");

            if (response != null && response.Products != null && response.Products.Count > 0)
            {
                prod.Id = response.Products[0].Id;
                int count = Math.Min(30, response.Products.Count);
                for (int i = 0; i < count; i++)
                {
                    temp.Add(response.Products[i]);
                }
            }

            ViewBag.Cart = _cart;
            ViewBag.CartCount = _cart.Count;
            return View(temp);
        }

        public async Task<IActionResult> AddToCart(int productId)
        {
            var productDetails = await GetProductDetails(productId);

            var product = _cart.Find(p => p.Id == productId);
            if (product == null)
            {

                product = new Product
                {
                    Id = productId,
                    Title = productDetails.Title,
                    Price = productDetails.Price
                };
                _cart.Add(product);
            }

            return RedirectToAction("FetchProducts");
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var product = _cart.Find(p => p.Id == productId);
            if (product != null)
            {
                _cart.Remove(product);
            }

            return RedirectToAction("ViewCart");
        }

        private async Task<ProductDetails> GetProductDetails(int productId)
        {

            var response = await _httpClient.GetFromJsonAsync<ProductApiResponse>("https://dummyjson.com/products");
            var product = response.Products.Find(p => p.Id == productId);

            if (product != null)
            {
                return new ProductDetails
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price
                };
            }
            return null;
        }

        public IActionResult ViewCart()
        {
            ViewBag.Cart = _cart;
            return View(_cart);
        }

        public IActionResult TestData()
        {
            return View();
        }

        public async Task<IActionResult> Test(TestData temp)
        {
            bool result = await _repo.TestCon(temp);
            if (result)
            {
                return View("SuccessPage");
            }
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            bool result = await _repo.Login(email, password);
            if (result)
            {

                return RedirectToAction("FetchProducts");
            }
            else
            {

                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Login");
            }
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SuccessPage()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            ViewBag.Cart = _cart;
            return View();
        }
        public async Task<IActionResult> ProcessCheckout(Order order)
        {
            order.TotalPrice = _cart.Sum(p => p.Price);
            int orderId = await _repo.SaveOrder(order);

            // Ensure fetchorderid() returns the correct order ID
            string oidString = await _repo.fetchorderid();

            if (int.TryParse(oidString, out int oid))
            {
                _cart.Clear();
                ViewBag.OrderId = orderId;
                return RedirectToAction("ThankYou", new { oid = oid });
            }

            ViewBag.ErrorMessage = "Failed to process the order. Please try again.";
            ViewBag.Cart = _cart;
            return View("Checkout", order);
        }
        public IActionResult ThankYou(int oid)
        {
            ViewBag.OrderId = oid;
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }
    }
}