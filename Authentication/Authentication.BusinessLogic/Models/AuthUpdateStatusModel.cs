using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.BusinessLogic.Models
{
    public class AuthUpdateStatusModel
    {
        public Guid UserId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
