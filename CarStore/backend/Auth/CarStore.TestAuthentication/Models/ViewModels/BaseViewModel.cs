using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStore.TestAuthentication.Models.ViewModels
{
    public class BaseViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
