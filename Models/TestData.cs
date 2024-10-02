namespace Project.Models
{
    public class TestData
    {

        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string DOB { get; set; }

        public string PhoneNumber { get; set; }

        public string Age { get; set; }

        public string City { get; set; }

        public string state { get; set; }

        public string ZipCode { get; set; }

        public string password { get; set; }

        public string gender { get; set; }

    }
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }


    }
    public class ProductDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public interface IOrderService
    {
        Task<int> SaveOrder(Order order);
    }
    public class ProductApiResponse
    {
        public List<Product> Products { get; set; }
    }


}