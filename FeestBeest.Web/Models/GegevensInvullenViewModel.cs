// using FeestBeest.Data.Dto;
// using System;
// using System.Collections.Generic;
// using System.Linq;
//
// public class GegevensInvullenViewModel
// {
//     public int Id { get; set; }
//     public DateTime SelectedDate { get; set; }
//     public List<Product> SelectedBeestjes { get; set; } = new List<Product>();
//     public string ContactNaam { get; set; }
//     public string ContactAdres { get; set; }
//     public string ContactEmail { get; set; }
//     public string ContactTelefoonnummer { get; set; }
//
//     public static GegevensInvullenViewModel FromDto(GegevensInvullenDto dto)
//     {
//         return new GegevensInvullenViewModel
//         {
//             Id = dto.Id,
//             SelectedDate = dto.SelectedDate,
//             SelectedBeestjes = dto.SelectedBeestjes.Select(b => new Product
//             {
//                 Id = b.Id,
//                 Naam = b.Naam,
//                 Type = b.Type,
//                 Prijs = b.Prijs,
//                 Afbeelding = b.Afbeelding
//             }).ToList(),
//             ContactNaam = dto.ContactNaam,
//             ContactAdres = dto.ContactAdres,
//             ContactEmail = dto.ContactEmail,
//             ContactTelefoonnummer = dto.ContactTelefoonnummer
//         };
//     }
// }