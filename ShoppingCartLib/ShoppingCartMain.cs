using System;
using System.Collections.Generic;

namespace ShoppingCartLib
{
    public interface ITerminal
    {
        void SetItemPrice(string item, decimal price);
        void SetItemDiscount(string item, int discountQuantity, decimal price);
        void Scan(string item);
        void ClearItems();
        decimal Total();
    }

    public class Sale : ITerminal
    {
        private static Dictionary<string, int> itemQuantities = new Dictionary<string, int>();
        private static Dictionary<string, decimal> itemPrices = new Dictionary<string, decimal>();
        private static Dictionary<string, Dictionary<int, decimal>> ItemDiscount = new Dictionary<string, Dictionary<int, decimal>>();

        public void ClearItems()
        {
            itemQuantities.Clear();
        }

        public void SetItemPrice(string item, decimal price)
        {
            try
            {
                if ((item.Length <= 0) || (price < 0.0m))
                {
                    throw new Exception("Invalid item/price !!");

                }

                itemPrices.Add(item, price);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void SetItemDiscount(string item, int discountQuantity, decimal price)
        {
            var discount = new Dictionary<int, decimal>();
            try
            {
                if ((item.Length <= 0) || (price < 0.0m))
                {
                    throw new Exception("Invalid item/price !!");
                }

                discount[discountQuantity] = price;
                ItemDiscount[item] = discount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public decimal CalculateDiscount(string item, int quantity)
        {
            int discountQuanity = 0;
            decimal discountPrice = 0;
            decimal actualPrice = 0;
            decimal finalPriceOfItem = 0;

            try
            {
                if ((item.Length == 0) || (quantity < 0))
                {
                    throw new Exception("Invalid item/quantity");
                }

                actualPrice = itemPrices[item];

                finalPriceOfItem = actualPrice * quantity;

                if (ItemDiscount.ContainsKey(item))
                {
                    var temp = ItemDiscount[item];
                    foreach (var i in temp)
                    {
                        discountQuanity = i.Key;
                        discountPrice = i.Value;
                    }

                    if (quantity >= discountQuanity)
                    {
                        // This can be done differntly by calculating discounted price per each item and then multiplying it with the quantity. 
                        // I felt this way is more accurate way as they are decimal values.
                        finalPriceOfItem = (actualPrice * (quantity % discountQuanity)) + ((quantity / discountQuanity) * discountPrice);
                    }
                }

                Console.WriteLine("Total discount on item {0} is {1}", item, finalPriceOfItem);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return finalPriceOfItem;
        }

        public void Scan(string item)
        {
            try
            {
                if (!itemPrices.ContainsKey(item))
                {
                    throw new Exception("Item not found in inventory !!");

                }

                AddItemToCart(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void AddItemToCart(string item)
        {
            int quantityCount;
            if (itemQuantities.TryGetValue(item, out quantityCount))
                itemQuantities[item] = ++quantityCount;
            else
                itemQuantities.Add(item, quantityCount = 1);
        }

        public decimal Total()
        {
            decimal totalPrice = 0;
            foreach (var item in itemQuantities)
            {
                totalPrice += CalculateDiscount(item.Key, item.Value);
            }

            return totalPrice;
        }
        // Added for self testing purpose before adding the test project
        public static void Main()
        {
            Sale saleOrder = new Sale();

            saleOrder.SetItemPrice("A", 2.0m);
            saleOrder.SetItemPrice("B", 12.0m);
            saleOrder.SetItemPrice("C", 1.25m);
            saleOrder.SetItemPrice("D", 0.15m);

            saleOrder.SetItemDiscount("A", 4, 7.0m);
            saleOrder.SetItemDiscount("C", 6, 6.0m);
            saleOrder.SetItemDiscount("", 6, 6.0m);
            saleOrder.SetItemDiscount("C", 0, 6.0m);

            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");

            Console.WriteLine("Total = {0}", saleOrder.Total());

        }
    }
}
