using FeestBeest.Data.Dto;

namespace FeestBeest.Web.ViewModels
{
    public class BeestjeViewModel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Type { get; set; }
        public decimal Prijs { get; set; }
        public string Afbeelding { get; set; }

        public BeestjeDto ToDto()
        {
            return new BeestjeDto
            {
                Id = this.Id,
                Naam = this.Naam,
                Type = this.Type,
                Prijs = this.Prijs,
                Afbeelding = this.Afbeelding
            };
        }

        public static BeestjeViewModel FromDto(BeestjeDto dto)
        {
            return new BeestjeViewModel
            {
                Id = dto.Id,
                Naam = dto.Naam,
                Type = dto.Type,
                Prijs = dto.Prijs,
                Afbeelding = dto.Afbeelding
            };
        }
    }
}