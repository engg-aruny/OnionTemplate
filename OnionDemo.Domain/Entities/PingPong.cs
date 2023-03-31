using System.ComponentModel.DataAnnotations.Schema;

namespace OnionDemo.Domain.Entities
{
    public class PingPong
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public DateTimeOffset ManufactureDate { get; set; }


        [ForeignKey(nameof(ManufacturerId))]
        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}
