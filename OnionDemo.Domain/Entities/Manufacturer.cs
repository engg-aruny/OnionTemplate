using System.ComponentModel.DataAnnotations.Schema;

namespace OnionDemo.Domain.Entities
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public PingPong PingPong { get; set; }
    }
}
