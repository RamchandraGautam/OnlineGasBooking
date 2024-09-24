namespace OnlineGasBooking.Models
{
    public class TempShpData
    {
        public static int UserID { get; set; }
        public static List<OrderDetail>? items { get; set; }
        public static List<NewConnection>? Connections { get; set; }
    }
}
