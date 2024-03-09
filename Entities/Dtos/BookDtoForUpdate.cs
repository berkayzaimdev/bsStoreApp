using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record BookDtoForUpdate : BookDtoForManipulation 
    {
        [Required]
        public int Id { get; init; }
    }
    //public record BookDtoForUpdate
    //{
    //    public int Id { get; init; }
    //    public String Title { get; init; }
    //    public decimal Price { get; init; }

    //    // init olarak tanım kullanmamızın sebebi, ilk başta set etmek zorunda oluruz ve daha sonra değiştirilemez.

    //    // Aynı tanımlamayı
    //    // public record BookDtoForUpdate(int Id, String Title, decimal Price); olarak da sağlayabilirdik
    //}

    ///*
    // DTO'lar;
    //    *readonly olmalıdır.
    //    *immutable olmalıdır. Değerleri değişmemelidir.
    //    *LINQ desteği vardır. Sorgular yazabiliriz.
    //    *reference type'lardır. Class ile aynı özellikleri taşır.
    //    *Constructor'a sahiptir. 
    //*/
}
