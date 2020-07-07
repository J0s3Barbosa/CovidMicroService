using Services.Domain.Entities;
using Services.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Application.Interfaces
{
    public interface ICovid19Logic : IGenericLogicAsync<Covid19>
    {
        /// <summary>
        /// List async, could be filtered by country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<List<Covid19>> ListAsync(string country);

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="covid"></param>
        /// <returns></returns>
        Task<Result<Covid19>> AddCovidAsync(Covid19 covid);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="covid"></param>
        /// <returns></returns>
        Task<Result<Covid19>> UpdateAsync(Guid id, Covid19 covid);

        Task<int?> DeleteAsync(Guid id);


    

    }
}
