using System;

namespace Domain.ViewModel
{
    public record UserLogin
    {
      public string CPF {get;set;}        
      public string Password {get;set;}        
    }
}
