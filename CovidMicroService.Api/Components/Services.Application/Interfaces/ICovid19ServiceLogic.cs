using Services.Domain.Entities;
using Services.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Application.Interfaces
{
    public interface ICovid19ServiceLogic
    {
        /// <summary>
        /// GetDataByCountryandDateFromDB
        /// </summary>
        /// <returns></returns>
        IEnumerable<Covid19> GetDataByCountryandDateFromDB();
        /// <summary>
        /// GetDataByCountryFromDB
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        IEnumerable<Covid19> GetDataByCountryFromDB(string country);
        /// <summary>
        /// GetDataByDateFromDBAsync
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IEnumerable<Covid19>> GetDataByDateFromDBAsync(string date);

        /// <summary>
        /// ListOfCovid19DataAsync
        /// </summary>
        /// <returns></returns>
        Task<List<Covid19>> ListOfCovid19DataAsync();
        /// <summary>
        /// PostCovid19DataToMongoDBAsync
        /// </summary>
        /// <returns></returns>
        Task<Result<string>> PostCovid19DataToMongoDBAsync();
    }
}