

namespace CoreApiTest.Resource
{
    public class OrderApiResource
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public UserApiResource? User { get; set; }
    }
}
