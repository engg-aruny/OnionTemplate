using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDemo.Domain.Exceptions
{
    public sealed class PingPongNotFoundException : NotFoundException
    {
        public PingPongNotFoundException(int postId)
            : base($"The Ping Pong with the identifier {postId} was not found.")
        {
        }
    }
}
