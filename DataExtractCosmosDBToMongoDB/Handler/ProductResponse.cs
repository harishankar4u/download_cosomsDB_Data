namespace DataExtractCosmosDBToMongoDB.Handler
{
    public class ProductResponse
    {
        public string productId { get; set; }
        public string SKU { get; set; }
        public string sellerId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int category { get; set; }
        public string color { get; set; }
        public string fabric { get; set; }
        public string fact { get; set; }
        public string size { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string bannderId { get; set; }
        public string bannderDiscription { get; set; }
        public bool makeInIndia { get; set; }
        public bool vegan { get; set; }
        public bool handmade { get; set; }
        public double productGST { get; set; }
        public List<ProductAttribute> productAttributes { get; set; }
        public double price { get; set; }
        public double sellingPrice { get; set; }
        public Delivery delivery { get; set; }
        public string search { get; set; }
        public DateTime date { get; set; }
        public bool active { get; set; }
        public string id { get; set; }
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _attachments { get; set; }
        public long _ts { get; set; }
    }

    public class ProductAttribute
    {
        public string A_Key { get; set; }
        public string A_Value { get; set; }
    }

    public class Delivery
    {
        public string cashOnDelivery { get; set; }
        public string returnable { get; set; }
        public string refundable { get; set; }
    }

}
