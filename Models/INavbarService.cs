namespace OnlineGasBooking.Models
{
    public interface INavbarService
    {
        Task<Dictionary<string, IEnumerable<string>>> GetNavbarDataAsync();
    }
}
