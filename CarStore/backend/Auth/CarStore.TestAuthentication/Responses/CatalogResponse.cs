using CarStore.TestAuthentication.Models;
using System.Collections.Generic;

namespace CarStore.TestAuthentication.Responses
{
    public class CatalogResponse : BaseResponse
    {
        /// <summary>
        /// List catalogs.
        /// </summary>
        public IEnumerable<CatalogModel> Catalogs { get; set; }

        /// <summary>
        /// Catalog object.
        /// </summary>
        public CatalogModel Catalog { get; set; }
    }
}
