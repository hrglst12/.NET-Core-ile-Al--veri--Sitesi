using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Site.Models
{
    //rol,sipariş durumu,sessions ile ilgili işlemler bu class içinde oluşucak
    public static class Diger
    {
        public const string Role_Birey = "Birey";//facebook yada google ile giriş yapanlar için
        public const string Role_Admin = "Admin";
        public const string Role_User  = "User";
        public const string ssShoppingCart = "Shopping Cart Session";
    }
}
