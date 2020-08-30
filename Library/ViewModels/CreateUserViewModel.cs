using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class CreateUserViewModel
    {
        
        public string Email { get; set; }
        
        public string Name { get; set; }
        
        public string Password { get; set; }
    }
    public class EditUserViewModel
    {
     
        public string Id { get; set; }
        
        public string Email { get; set; }
        
        public string Name { get; set; }
        
    }
}
