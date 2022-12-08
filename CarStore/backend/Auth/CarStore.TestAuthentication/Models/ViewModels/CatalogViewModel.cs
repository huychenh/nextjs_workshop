using System.ComponentModel.DataAnnotations;

namespace CarStore.TestAuthentication.Models.ViewModels
{
    public class CatalogViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
