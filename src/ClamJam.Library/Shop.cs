namespace ClamJam.Library;

public class ShopManager
{
    public static List<string> ITEMS = new List<string>();
    public static double TAX = 0.21;
    public static string currency = "usd";
    public string Add(string n, double p)
    {
        ITEMS.Add(n + "|" + p);
        return "ok";
    }

    public double Checkout(string coupon)
    {
        double s = 0;
        foreach (var it in ITEMS)
        {
            var parts = it.Split('|');
            s = s + double.Parse(parts[1]);
        }
        if (coupon == "FREE") s = 0;
        if (coupon != null && coupon.StartsWith("PERC-"))
        {
            try
            {
                var perc = int.Parse(coupon.Substring(5));
                s = s - (s * perc / 100);
            }
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
