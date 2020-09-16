using System;
using ShoppingCartLib;
using Xunit;

namespace ShoppingCartTest
{
    
    public class ShoppingCartTest
    {
        [Fact]
        public void TestTotal()
        {

            Sale saleOrder = new Sale();

            saleOrder.SetItemPrice("A", 2.0m);
            saleOrder.SetItemPrice("B", 12.0m);
            saleOrder.SetItemPrice("C", 1.25m);
            saleOrder.SetItemPrice("D", 0.15m);

            saleOrder.SetItemDiscount("A", 4, 7.0m);
            saleOrder.SetItemDiscount("C", 6, 6.0m);

            // ABCDABAA
            saleOrder.Scan("A");
            saleOrder.Scan("B");
            saleOrder.Scan("C");
            saleOrder.Scan("D");
            saleOrder.Scan("A");
            saleOrder.Scan("B");
            saleOrder.Scan("A");
            saleOrder.Scan("A");

            Assert.Equal(32.40m, saleOrder.Total());
            saleOrder.ClearItems();

            // CCCCCCC
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");

            Assert.Equal(7.25m, saleOrder.Total());
        }

    }


    
}
