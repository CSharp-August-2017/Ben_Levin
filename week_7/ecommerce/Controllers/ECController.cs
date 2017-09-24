using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; //for password hashing
using System.Linq; //for queries
using Microsoft.Extensions.Options; //for secure strings, not DB connection

using ecommerce.Models;

namespace ecommerce.Controllers
{
    public class ECController : Controller
    {
        private ECContext _context;
    
        public ECController(ECContext context)
        {
            _context = context;
        }

//**LOGIN + LOGOUT + REGISTER

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //redirect if user already signed in
            {
                return Redirect("/dashboard");
            }
            return View("Index");
        }
        
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel Login)
        {
            if(ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(User => User.Email == Login.LogEmail); //searching for e-mail in db
                if(user != null) //if e-mail is in db
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    var Authentication = Hasher.VerifyHashedPassword(user, user.Password, Login.LogPassword); //check hashed password match
                    if(Authentication == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetInt32("ID", user.ID); //logging in via session variables
                        HttpContext.Session.SetString("Name", user.FirstName);
                        return Redirect("/");
                    }
                }
                ViewBag.LoginError = "invalid password"; //intentionally ambiguous for null e-mail (security reasons)
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel Register)
        {
            if(ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(User => User.Email == Register.RegEmail); //checking db for e-mail address to prevent duplicates
                if(user == null) //if e-mail not already in use
                {
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    string HashedPassword = Hasher.HashPassword(Register, Register.RegPassword); //hashing password
                    User newUser = new User() //creating user
                    {
                        FirstName = Register.RegFirstName,
                        LastName = Register.RegLastName,
                        Email = Register.RegEmail,
                        Password = HashedPassword,
                        Created_At = DateTime.Now,
                        Updated_At = DateTime.Now,
                    };
                    LoginViewModel UserLogin = new LoginViewModel() //creating login model for automatic login
                    {
                        LogEmail = newUser.Email,
                        LogPassword = Register.RegPassword,
                    };
                    _context.Users.Add(newUser); //adding user to database
                    _context.SaveChanges();
                    return Login(UserLogin);
                }
                ViewBag.RegistrationError = "e-mail address is already in use";
            }
            return View("Index");
        }

//SITE GET HANDLING

        [HttpGet]
        [Route("customers")]
        public IActionResult Customers(string search = null, string customererror = null)
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                List<Customer> AllCustomers = _context.Customers.OrderByDescending(customer => customer.Status).ThenBy(customer => customer.FirstName).ToList();
                List<CustomerRenderModel> Customers = new List<CustomerRenderModel>();
                foreach(var result in AllCustomers) //render viewable data
                {
                    CustomerRenderModel FormattedCustomer = new CustomerRenderModel()
                    {
                        ID = result.ID,
                        CompleteName = (result.FirstName+" "+result.LastName),
                        FirstName = result.FirstName,
                        LastName = result.LastName,
                        Date = ((DateTime)result.Created_At).ToString("MMMM dd, yyyy"),
                        Status = result.Status
                    };
                    Customers.Add(FormattedCustomer);
                }
                if(search != null) //single criteria search functionality, if search string is submitted -- does not include active v. inactive
                {
                    List<CustomerRenderModel> CustomersFiltered = new List<CustomerRenderModel>();
                    string keyword = search.ToUpper();
                    List<CustomerRenderModel> CN = Customers.Where(customer => customer.CompleteName.ToUpper().Contains(keyword)).ToList();
                    foreach(var result in CN)
                    {
                        CustomersFiltered.Add(result);
                    }
                    List<CustomerRenderModel> FN = Customers.Where(customer => customer.FirstName.ToUpper().Contains(keyword)).ToList();
                    foreach(var result in FN)
                    {
                        CustomersFiltered.Add(result);
                    }
                    List<CustomerRenderModel> LN = Customers.Where(customer => customer.LastName.ToUpper().Contains(keyword)).ToList();
                    foreach(var result in LN)
                    {
                        CustomersFiltered.Add(result);
                    }
                    List<CustomerRenderModel> DT = Customers.Where(customer => customer.Date.ToUpper().Contains(keyword)).ToList();
                    foreach(var result in DT)
                    {
                        CustomersFiltered.Add(result);
                    }
                    Customers = CustomersFiltered.Distinct().ToList(); //removing duplicate entries
                }
                ViewBag.Customers = Customers;
                ViewBag.CustomerError = customererror;
                ViewBag.ID = HttpContext.Session.GetInt32("ID");
                return View("Customers");
            }
            return Redirect("/");
        }

        [HttpGet] //marking customer inactive or active instead of delete, due to db table dependencies
        [Route("customer/{CustomerID}/{Status}")]
        public IActionResult CustomerStatus(int CustomerID, int Status)
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                Customer Customer = _context.Customers.Where(customer => customer.ID == CustomerID).SingleOrDefault();
                if(Status == 1) //set customer inactive
                {
                    Customer.Status = 0;
                }
                else if(Status == 0) //set customer active
                {
                    Customer.Status = 1;
                }
                _context.SaveChanges();
                return Redirect("/customers");
            }
            return Redirect("/");
        }

        //ACTIVITY FEEDS RESTRUCTURED FROM WIREFRAME (I.E. CUSTOMER FEED DOES NOT DISPLAY PURCHASES)
        //SEARCH FUNCTIONALITY NOT INCLUDED ON DASHBOARD, SINCE IT IS ON EVERY OTHER PAGE
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                //rendering dashboard product preview data
                List<Product> AllProducts =  _context.Products.OrderBy(product => product.Name).ToList();
                List<Product> Products = new List<Product>();
                if(AllProducts.Count() < 4)
                {
                    Products = AllProducts;
                }
                else
                {
                    Products = AllProducts.GetRange(0, 4);
                }
                //rendering dashboard order preview data
                List<Order> AllOrders = _context.Orders.OrderByDescending(order => order.Created_At).ToList();
                List<Order> OrderPreview = new List<Order>();
                if(AllOrders.Count() < 3)
                {
                    OrderPreview = AllOrders;
                }
                else
                {
                    OrderPreview = AllOrders.GetRange(0,3);
                }
                List<OrderRenderModel> Orders = new List<OrderRenderModel>(); //render viewable data
                foreach(var result in OrderPreview)
                {
                    Customer Customer = _context.Customers.Where(customer => customer.ID == result.customerID).SingleOrDefault();
                    string customer_name = Customer.FirstName+" "+Customer.LastName;
                    Product orderProduct = _context.Products.Where(product => product.ID == result.productID).SingleOrDefault();
                    string product_name = orderProduct.Name;
                    OrderRenderModel FormattedOrder = new OrderRenderModel()
                    {
                        CustomerName = customer_name,
                        ProductName = product_name,
                        ProductQuantity = (int)result.ProductQuantity,
                        HoursAgo = ((DateTime.Now - (DateTime)result.Created_At)).Hours.ToString(),
                    };
                    Orders.Add(FormattedOrder);
                }
                //rendering dashboard customer preview data
                List<Customer> AllCustomers = _context.Customers.OrderByDescending(customer => customer.Created_At).ToList();
                List<Customer> CustomerPreview = new List<Customer>();
                if(AllCustomers.Count() < 3)
                {
                    CustomerPreview = AllCustomers;
                }
                else
                {
                    CustomerPreview = AllCustomers.GetRange(0, 3);
                }
                List<CustomerRenderModel> FormattedCustomers = new List<CustomerRenderModel>();
                foreach(var result in CustomerPreview) //render viewable data
                {
                    CustomerRenderModel FormattedCustomer = new CustomerRenderModel()
                    {
                        CompleteName = (result.FirstName+" "+result.LastName),
                        HoursAgo = ((DateTime.Now - (DateTime)result.Created_At)).Hours.ToString(),
                    };
                    FormattedCustomers.Add(FormattedCustomer);
                }
                List<CustomerRenderModel> Customers = FormattedCustomers;
                @ViewBag.Customers = Customers;
                ViewBag.ID = HttpContext.Session.GetInt32("ID");
                @ViewBag.Orders = Orders;
                @ViewBag.Products = Products;
                return View("Dashboard");
            }
            return Redirect("/");
        }

        [HttpGet]
        [Route("orders")]
        public IActionResult Orders(string search = null, string ordererror = null)
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                List<Order> AllOrders = _context.Orders.OrderBy(order => order.Created_At).ToList();
                List<OrderRenderModel> Orders = new List<OrderRenderModel>(); //render viewable data
                foreach(var result in AllOrders)
                {
                    Customer Customer = _context.Customers.Where(customer => customer.ID == result.customerID).SingleOrDefault();
                    string customer_name = Customer.FirstName+" "+Customer.LastName;
                    Product orderProduct = _context.Products.Where(product => product.ID == result.productID).SingleOrDefault();
                    string product_name = orderProduct.Name;
                    OrderRenderModel FormattedOrder = new OrderRenderModel()
                    {
                        CustomerName = customer_name,
                        CustomerStatus = Customer.Status,
                        ProductName = product_name,
                        ProductQuantity = (int)result.ProductQuantity,
                        Date = ((DateTime)result.Created_At).ToString("MMMM dd, yyyy"),
                    };
                    Orders.Add(FormattedOrder);
                }
                if(search != null) //single criteria search functionality, if search string is submitted -- does not include active v. inactive users
                {
                    string keyword = search.ToUpper();
                    List<OrderRenderModel> FilteredOrders = new List<OrderRenderModel>();
                    List<OrderRenderModel> CN = Orders.Where(order => order.CustomerName.ToUpper().Contains((string)keyword)).ToList();
                    foreach(var result in CN)
                    {
                        FilteredOrders.Add(result);
                    }
                    List<OrderRenderModel> PN = Orders.Where(order => order.ProductName.ToUpper().Contains((string)keyword)).ToList();
                    foreach(var result in PN)
                    {
                        FilteredOrders.Add(result);
                    }
                    List<OrderRenderModel> PQ = Orders.Where(order => Convert.ToString(order.ProductQuantity).Contains((string)keyword)).ToList();
                    foreach(var result in PQ)
                    {
                        FilteredOrders.Add(result);
                    }
                    List<OrderRenderModel> DT = Orders.Where(order => order.Date.ToUpper().Contains((string)keyword)).ToList();
                    foreach(var result in DT)
                    {
                        FilteredOrders.Add(result);
                    }
                    Orders = FilteredOrders.Distinct().ToList(); //removing duplicate entries
                }
                ViewBag.Customers = _context.Customers.Where(customer => customer.Status == 1).OrderBy(customer => customer.FirstName).ToList();
                ViewBag.ID = HttpContext.Session.GetInt32("ID");
                ViewBag.Orders = Orders;
                ViewBag.OrderError = ordererror;
                ViewBag.Products = _context.Products.OrderBy(product => product.Name).ToList();
                return View("Orders");
            }
            return Redirect("/");
        }      

        [HttpGet]
        [Route("products")]
        public IActionResult Products(string search = null, string productserror = null)
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                List<Product> AllProducts =  _context.Products.OrderBy(product => product.Name).ToList();
                List<Product> Products = AllProducts;
                if(search != null) //product name OR description criteria search functionality, if search string is submitted
                {
                    string keyword = search.ToUpper();
                    List<Product> FilteredProducts = new List<Product>();
                    List<Product> PN = Products.Where(product => product.Name.ToUpper().Contains((string)keyword)).ToList();
                    foreach(var result in PN)
                    {
                        FilteredProducts.Add(result);
                    }
                    List<Product> PD = Products.Where(product => product.Description.ToUpper().Contains((string)keyword)).ToList();
                    foreach(var result in PD)
                    {
                        FilteredProducts.Add(result);
                    }
                    Products = FilteredProducts.Distinct().ToList(); //removing duplicate entries
                }
                ViewBag.ID = HttpContext.Session.GetInt32("ID");
                ViewBag.Products = Products;
                ViewBag.ProductsError = productserror;
                return View("Products");
            }
            return Redirect("/");
        }

        //NO CART FUNCTIONALITY -- ORDERS HANDLED THROUGH ORDER PAGE
        [HttpGet] 
        [Route("product/{productID}")]
        public IActionResult ProductDetail(int productID)
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                Product Product = _context.Products.Where(product => product.ID == productID).SingleOrDefault();
                if(Product != null)
                {
                    ViewBag.ID = HttpContext.Session.GetInt32("ID");
                    ViewBag.Product = Product;
                    return View("ProductDetail");
                }
                return Redirect("/products");
            }
            return Redirect("/");
        }

        [HttpGet]
        [Route("settings")]
        public IActionResult Settings()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //check if user is signed in
            {
                //no functionality
                return Redirect("/dashboard");
            }
            return Redirect("/");
        }

//SITE POST HANDLING

        [HttpPost]
        [Route("addcustomer")]
        public IActionResult AddCustomer(Customer Customer)
        {
            if(ModelState.IsValid)
            {
                Customer UniqueCustomer = _context.Customers.Where(customer => customer.FirstName == Customer.FirstName).Where(customer => customer.LastName == Customer.LastName).SingleOrDefault();
                if(UniqueCustomer == null)
                {
                    Customer.userID = (int)HttpContext.Session.GetInt32("ID");
                    Customer.Status = 1;
                    Customer.Created_At = DateTime.Now;
                    Customer.Updated_At = DateTime.Now;
                    _context.Customers.Add(Customer);
                    _context.SaveChanges();
                    return Redirect("/customers");
                }
                string search = null;
                string customererror = "customer name already in use";
                return Customers(search, customererror);
            }
            return Customers();
        }

        //DOES NOT PERMIT RETURNS (NEGATIVE QUANTITY TRANSACTIONS)
        [HttpPost]
        [Route("addorder")]
        public IActionResult AddOrder(int CID, int PID, int PQ)
        {
            if(ModelState.IsValid) //not using a model, allow returns with negative numbers
            {
                string search = null; //in event of errors
                if(PQ >= 1)
                {
                    Product Product = _context.Products.Where(product => product.ID == PID).SingleOrDefault();
                    if(PQ <= Product.Quantity) //insure ordered quantity is permitted by inventory
                    {
                        Order order = new Order()
                        {
                            userID = (int)HttpContext.Session.GetInt32("ID"),
                            customerID = (int)CID,
                            productID = (int)PID,
                            ProductQuantity = (double)PQ,
                            Created_At = DateTime.Now,
                            Updated_At = DateTime.Now
                        };
                        _context.Orders.Add(order);
                        Product.Quantity = (Product.Quantity - PQ); //update inventory for product
                        _context.SaveChanges();
                        return Redirect("/orders");
                    }
                    string ordererror1 = "not enough product quantity is in stock";
                    return Orders(search, ordererror1);  
                }
                string ordererror2 = "quantity must be one or more";
                return Orders(search, ordererror2);  
            }
            return Orders();            
        }

        //NO FUNCTIONALITY FOR ADDING QUANTITY TO EXISTING PRODUCTS
        [HttpPost]
        [Route("addproduct")]
        public IActionResult AddProduct(Product Product)
        {
            if(ModelState.IsValid)
            {
                Product UniqueProductName = _context.Products.Where(product => product.Name == Product.Name).SingleOrDefault();
                string search = null; //in event of error
                if(UniqueProductName == null) // preventing duplicate name entries
                {
                    Product UniqueProductImageURL = _context.Products.Where(product => product.ImageURL == Product.ImageURL).SingleOrDefault();
                    if(UniqueProductImageURL == null) // preventing duplicate image URL entries
                    {
                        Product.userID = (int)HttpContext.Session.GetInt32("ID");
                        Product.Created_At = DateTime.Now;
                        Product.Updated_At = DateTime.Now;
                        _context.Products.Add(Product);
                        _context.SaveChanges();
                        return Redirect("/products");
                    }
                    string producterror1 = "product image already in catalog";
                    return Products(search, producterror1);
                }
                string producterror2 = "product name already in catalog";
                return Products(search, producterror2);
            }
            return Products();
        }

//**REDIRECTS FOR GET REQUESTS SENT TO POST URLS

        [HttpGet]
        [Route("addcustomer")]
        public IActionResult AddCustomerRedirect()
        {
            return Redirect("/customers");
        }

        [HttpGet]
        [Route("addorder")]
        public IActionResult AddOrderRedirect()
        {
            return Redirect("/orders");
        }

        [HttpGet]
        [Route("addproduct")]
        public IActionResult AddProductRedirect()
        {
            return Redirect("/products");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginRedirect()
        {
            return Redirect("/");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult RegisterRedirect()
        {
            return Redirect("/");
        }
    }
}