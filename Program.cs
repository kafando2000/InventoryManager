using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
class Program
{   
    static InventoryManager ProductInventory = new InventoryManager();
     // main 
    static void Main(string[] args)
    {   
        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine("\n 1. Add a new product to stock.");
            Console.WriteLine("\n 2. Remove product from stock.");
            Console.WriteLine("\n 3. Sell a product in stock.");
            Console.WriteLine("\n 4. Add items to a product in stock (restock).");
            Console.WriteLine("\n 5. View products in stock and their stock level.");
            Console.WriteLine("\n q. to quite");
            Console.WriteLine("\n Enter a number above of your choice ");
            string? serviceNumber = Console.ReadLine();
            switch (serviceNumber)
            {
                case "1":
                    // ask user for entries
                    Console.WriteLine("Enter product Name : ");
                    var Name = Console.ReadLine();
                    Console.WriteLine("Enter product Price( numerical value) : ");
                    bool isDecimal = decimal.TryParse(Console.ReadLine(), out decimal price) ;
                    Console.WriteLine("Enter product Quantity (whole number) : ");
                    bool isInt = int.TryParse(Console.ReadLine(), out int quantity) ;
                    if(!string.IsNullOrWhiteSpace(Name) && isDecimal && isInt)
                    {
                        if(ProductInventory.addProduct(Name, price, quantity))
                        {
                            Console.WriteLine($"\n {Name} is added successfully !");
                        }
                        else
                        {
                            Console.WriteLine($"\n {Name} could not be added !");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\n check your entries, they are not valid!");
                    }
                    break;
                case "2":
                    // remove a product
                    Console.WriteLine("Enter product Name : ");
                    var NameToRemove = Console.ReadLine();
                    if(!string.IsNullOrWhiteSpace(NameToRemove))
                    {
                        if(ProductInventory.removeProduct(NameToRemove))
                        {
                            Console.WriteLine($"\n {NameToRemove} is removed successfully !");
                        }
                        {
                            Console.WriteLine($"\n {NameToRemove} could not be removed !");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\n check your entries, they are not valid!");
                    }
                    break;
                case "3":
                    // sell a product
                    Console.WriteLine("Enter product Name : ");
                    var NameToSell = Console.ReadLine();
                    Console.WriteLine("Enter product Quantity (whole number) : ");
                    bool isInt1 = int.TryParse(Console.ReadLine(), out int quantityTosell) ;
                    if(!string.IsNullOrWhiteSpace(NameToSell) && isInt1)
                    {
                        if(ProductInventory.sellProduct(NameToSell, quantityTosell))
                        {
                            Console.WriteLine($"\n {quantityTosell} items of {NameToSell}"+
                              " sold successfully !");
                        }
                        else
                        {
                            Console.WriteLine($"\n {NameToSell} could not be sold!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\n check your entries, they are not valid!");
                    }
                    break;
                case "4":
                    // update a product in stock
                    Console.WriteLine("Enter product Name : ");
                    var NameOfProduct = Console.ReadLine();
                    Console.WriteLine("Enter product price (numerical) : ");
                    bool isDecimal1 = decimal.TryParse(Console.ReadLine(), out decimal newPrice) ;
                    Console.WriteLine("Enter product Quantity (whole number) : ");
                    bool isInt2 = int.TryParse(Console.ReadLine(), out int quantityofItem) ;
                    if(!string.IsNullOrWhiteSpace(NameOfProduct) && isInt2 && isDecimal1)
                    {
                        if(ProductInventory.restockProduct(NameOfProduct, newPrice, quantityofItem))
                        {
                            Console.WriteLine($"\n {quantityofItem} items of {NameOfProduct}  added successfully !");
                        }
                         else
                        {
                            Console.WriteLine($"\n {NameOfProduct} could not be added!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\n check your entries, they are not valid!");
                    }
                    break;
                case "5":
                    // view products
                    ProductInventory.viewProductsAndStocksLevels();
                    break;
                case "q":
                case "quit":
                    isRunning = false;
                    Console.WriteLine("\n Quitting the program ... ");
                    break;
                default:
                    Console.WriteLine("Enter a valid Number between 1 and 5 or q to quit");
                    break;

            }
    }
       
    }
}
class InventoryManager
{   
    // product structure
    class Product
    {   
        static int productUniqueCounter = 0;
        static void updateCounter()
        {
            productUniqueCounter++;
        }
        public Product()
        {
            soldCounter = 0;
            quantity = 0;
            name = "";
            price = 0.0m;
            quantity = 0;
            updateCounter();
            Id = name + productUniqueCounter.ToString() ;
        }
        public Product(string productName, decimal productPrice, int productQuantity)
        {
            soldCounter = 0;
            quantity = productQuantity;
            name = productName;
            price = productPrice;
            updateCounter();
            Id = name + productUniqueCounter.ToString();
        }
        public string name {get; set;}
        public decimal price {get; set;}
        // stock level
        public int quantity {get; set;}
        // tracks total items sold for each product
        public int soldCounter {get; set;}
        public string Id { get; private set; } 

    }
    // list of the product
    static List<Product> Products = new List<Product>();


    // add a Product to the list
    public bool addProduct(string productName, decimal productPrice, int productQuantity)
    {   
        // check if name is valid
        if (!string.IsNullOrWhiteSpace(productName))
        {
            for (int i = 0; i < Products.Count; i++)
            {
                Product product = Products[i];
                if (product.name == productName)
                {
                    return UpdateProduct(ref product, productPrice, productQuantity, true);
                }
            }


            Products.Add(new Product(productName, productPrice, productQuantity));
            return true;
        }
        return false;
    }

    // remove a Product from the list 
    // return true if removed and false otherwise
    public bool removeProduct(string productName)
    {   
        // if not valid name
       if(string.IsNullOrWhiteSpace(productName))
            return false;
        // if product list is empty
       if(Products.Count == 0)
            return false;
       foreach (var product in Products)
       {
            if (product.name == productName)
            {
                Products.Remove(product);
                return true;
            }
       }
       return false;
    }

    // update product : quantity represents the quantity restocked or sold
    // is bool determines if it is restocked ortherwise it is sold
    bool UpdateProduct(ref Product product, decimal productPrice, int quantity, bool isRestocked)
    {   
        // it is to add more items
        if (isRestocked)
        {
            product.quantity += quantity;
            return true;
        }
        // it is to sell
        if(quantity > product.quantity)
        {
            return false;
        }
        product.quantity -= quantity;
        product.soldCounter += quantity;
        return true;
    }
    public bool sellProduct(string productName, int quantity)
    {
         // if not valid name
       if(string.IsNullOrWhiteSpace(productName))
            return false;
        // if product list is empty
       if(Products.Count == 0)
            return false;
        for (int i = 0; i < Products.Count; i++)
            {
                Product product = Products[i];
                if (product.name == productName)
                {
                    return UpdateProduct(ref product, product.price, quantity, false);
                }
        }
        return false;
    }

    public bool restockProduct(string productName, decimal price, int quantity)
    {
        // if not valid name
        if(string.IsNullOrWhiteSpace(productName))
            return false;
        // if product list is empty
        if(Products.Count == 0)
            return false;
        for (int i = 0; i < Products.Count; i++)
            {
                Product product = Products[i];
                if (product.name == productName)
                {
                    return UpdateProduct(ref product, price, quantity, true);

                }
        }
        return false;
    }

    public void viewProductsAndStocksLevels()
    {
         // if not valid name
        if(Products.Count == 0){
            Console.WriteLine("\n No product in the stock.");
            return;
        }
        Console.WriteLine($"\n Name | \t Stock Level |");
        foreach (var product in Products)
        {
            Console.WriteLine($"\n { product.name } | \t { product.quantity } |");
        }
    }

}