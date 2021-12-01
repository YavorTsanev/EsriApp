namespace EsriClient.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EditFormModel
    {
        [Required]
        public string StateName { get; set; }
    }
}
