﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ShopEsameElis.Models
{
    public partial class Utente
    {
        public Utente()
        {
            Carts = new HashSet<Cart>();
        }

        public int IdUtente { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
    }
}