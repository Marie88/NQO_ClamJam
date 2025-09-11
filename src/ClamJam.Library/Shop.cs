using System.Diagnostics.CodeAnalysis;

namespace ClamJam.Library;

public class ShopManager
{
    // static properties make the system non-thread safe, as they are shared accross all the instances of the shop manager
    // Complex objects are represented in a fragile manner by being serialized into strings instead of complex classes
    public static List<string> ITEMS = new List<string>(); // Fragile data storage
    public static double TAX = 0.21;
    public static string currency = "usd";

    public string Add(string n, double p)
    {
        ITEMS.Add(n + "|" + p);
        return "ok";
    }

    // no data validation, all values are truthy
    public double Checkout(string coupon)
    {
        // Calculating the total of the cart by separating the name from the price
        double s = 0; // non suggestive name of what the variable represents
        // decimal should be used for the final price, given the need for precision
        // when it comes to taxes, discount and exchange rates calculations that can
        // lead to rounding errors
        // comes of course with less performance and needs more space
        foreach (var it in ITEMS)
        {
            var parts = it.Split('|');
            s = s + double.Parse(parts[1]);
        }
        if (coupon == "FREE") s = 0; // hardcoded values for coupons, currency, tax
        if (coupon != null && coupon.StartsWith("PERC-"))
        {
            try
            {
                // fragile data serialization, non extensible
                var perc = int.Parse(coupon.Substring(5));
                s = s - (s * perc / 100);
            }
            // Exception swallowing
            catch { }
        }
        // add tax at the end in place
        s = s + (s * TAX);
        return Math.Round(s, 2);
    }

    public string Export()
    {
        var header = "currency," + currency + ",count," + ITEMS.Count;
        var body = string.Join(",", ITEMS.Select(x => x.Replace("|", ":")));
        return header + "," + body;
    }
}
