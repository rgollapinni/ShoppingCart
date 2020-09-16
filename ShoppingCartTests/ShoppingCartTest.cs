using System;
using System.Collections.Generic;
using System.Text;
using ShoppingCartLib;
using Xunit;

namespace ShoppingCartTests
{
    public class ShoppingCartTest
    {
        [Fact]
        public void TestTotal()
        {
            decimal expected;
            decimal actual;

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

            expected = 32.40m;
            actual = saleOrder.Total();
            Assert.Equal(expected, actual);
            saleOrder.ClearItems();

            // CCCCCCC
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");
            saleOrder.Scan("C");

            expected = 7.25m;
            actual = saleOrder.Total();
            Assert.Equal(expected, actual);
            saleOrder.ClearItems();
        }
    }
}


