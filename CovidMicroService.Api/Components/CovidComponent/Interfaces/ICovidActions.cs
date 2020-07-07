using Services.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidComponent.Interfaces
{
    public interface ICovidActions
    {
        /// <summary>
        /// Get Covid19 Cases Async using HtmlAgilityPack from website
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// List Covid19
        /// </returns>
        Task<List<Covid19>> GetCovid19CasesAsync();

        /// <summary>
        /// get data from mongo by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns>
        /// 
        /// List Covid19
        /// </returns>
        List<Covid19> GetDataFromMongoDbByDate(string dateToSearch = null);

        /// <summary>
        ///  get data from mongodb by date async
        /// </summary>
        /// <param name="dateToSearch"></param>
        /// <returns>
        /// list of data filtered by date desc
        /// </returns>
        Task<List<Covid19>> GetDataFromMongoDbByDateAsync(string dateToSearch = null);

        /// <summary>
        /// GetDataFromMongoDB
        /// </summary>
        /// <param name="country"></param>
        /// <param name="sortingByField"></param>
        /// <returns></returns>
        List<Covid19> GetDataFromMongoDB(string country, string sortingByField);


        /// <summary>
        /// Get data Report by selected Country
        /// </summary>
        /// <param name="country"></param>
        /// <param name="sortingByField"></param>
        /// <returns>
        /// a string with heighest number
        /// </returns>
        string GetDbReportFromCountry(string country, string sortingByField);

        /// <summary>
        /// post data loaded from google site to mongodb, it checks if has data from the current day, and post if there isnt.
        /// </summary>
        /// <returns>
        /// <para/> Success :   number of data Posted
        /// <para/> fails :   DB has "number of data data" for "this date""
        /// </returns>
        Task<Result<string>> PostDataToMongoDBAsync();
    }
}