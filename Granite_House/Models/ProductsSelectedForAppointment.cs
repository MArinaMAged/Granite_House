using System.ComponentModel.DataAnnotations.Schema;

namespace Granite_House.Models
{
    public class ProductsSelectedForAppointment
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointments Appointments { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }

    }
}
