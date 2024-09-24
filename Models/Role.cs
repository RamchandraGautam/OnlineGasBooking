using System.ComponentModel.DataAnnotations;

namespace OnlineGasBooking.Models
{
    public partial class Role
    {
        public Role()
        {
            this.admin_Login = new HashSet<admin_Login>();
        }
        [Key]
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<admin_Login> admin_Login { get; set; }
    }
}
