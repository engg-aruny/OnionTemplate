using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDemo.Domain.Entities
{
    public class PingPong
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public DateTimeOffset? PublishedDate { get; set; }
    }
}
