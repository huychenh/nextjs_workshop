using CarStore.TestAuthentication.Models;
using CarStore.TestAuthentication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStore.TestAuthentication.Commons
{
    public class CarStoreDataHelper
    {
        /// <summary>
        /// Convert a datetime value to a datetime with format yyyy-MM-dd HH':'mm':'ss
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <returns>string</returns>
        public static string GetDateTimeFormat(DateTime? dt)
        {
            if (dt == null)
            {
                return string.Empty;
            }

            try
            {
                DateTime formatDt = Convert.ToDateTime(dt);
                return formatDt.ToString("yyyy-MM-dd HH':'mm':'ss");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Catalog
        /// <summary>
        /// Common function to map data to model
        /// </summary>
        /// <param name="model">CatalogViewModel</param>
        /// <returns>CatalogModel</returns>
        public static CatalogModel MapDataToModel(CatalogViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new CatalogModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                ModifiedDate = model.ModifiedDate,
                ModifiedBy = model.ModifiedBy,
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted
            };
        }

        /// <summary>
        /// Common function to map data to ViewModel
        /// </summary>
        /// <param name="model">CatalogModel</param>
        /// <returns>CatalogViewModel</returns>
        public static CatalogViewModel MapDataToViewModel(CatalogModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new CatalogViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CreatedDate = (DateTime)model.CreatedDate,
                CreatedBy = model.CreatedBy,
                ModifiedDate = (DateTime)model.ModifiedDate,
                ModifiedBy = model.ModifiedBy,
                IsActive = (bool)model.IsActive,
                IsDeleted = (bool)model.IsDeleted
            };
        }

        #endregion
    }
}
