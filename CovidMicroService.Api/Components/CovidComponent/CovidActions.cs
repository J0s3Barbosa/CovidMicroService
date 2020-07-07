using ComponentsLib;
using ComponentsLib.Interfaces;
using CovidComponent.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidComponent
{
    public class CovidActions : ICovidActions
    {
        private readonly IConfiguration configuration;
        private readonly IMongoGeneric<Covid19> _iMongoGeneric;

        private readonly string dateFormat = "dd/MM/yyyy HH:mm:ss";
        private readonly string dateOnlyFormat = "dd/MM/yyyy";

        public CovidActions(IMongoGeneric<Covid19> iMongoGeneric)
        {
            _iMongoGeneric = iMongoGeneric;

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            configuration = config;

            var services = new ServiceCollection();
            services.AddCovidComponentConnector();

        }

        /// <summary>
        /// get data from mongodb, you can filter and sort data from object desired
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="databaseName"></param>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns>
        /// list of data type List<Covid19>
        /// </returns>
        public List<Covid19> GetDataFromMongoDB(object obj, string databaseName,
            FilterDefinition<Covid19> filter, SortDefinition<Covid19> sort)
        {
            var configMongoDb = configuration.GetSection("MongoDB").Value;
            if (string.IsNullOrEmpty(configMongoDb))
            {
                configMongoDb = Environment.GetEnvironmentVariable("MongoDB");
            }

            var dbCollection = obj.GetType().Name;

            MongoClient client = new MongoClient(configMongoDb);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<Covid19>(dbCollection);

            List<Covid19> CoronaDatas = collection
                .Find(filter)
                .Sort(sort)
                .ToList();

            return CoronaDatas;
        }

        /// <summary>
        /// list with latest numbers from DB
        /// </summary>
        /// <param name="localFilter"></param>
        /// <param name="sortingField"></param>
        /// <returns></returns>
        public List<Covid19> GetDataFromMongoDB(string country, string sortingByField)
        {
            var coronaObj = new Covid19();
            var listCoronaData = new List<Covid19>();

            //configure filtering
            var filter = Builders<Covid19>.Filter.Or(
               Builders<Covid19>.Filter.Where(p => p.Local.ToLower().Contains(country.ToLower())));

            //configure sorting
            var sort = Builders<Covid19>.Sort.Descending(sortingByField);

            listCoronaData = _iMongoGeneric.GetdbCollectionGeneric(coronaObj, "node", filter, sort);

            return listCoronaData;
        }

        /// <summary>
        /// Get Data From mongoDB By Date
        /// </summary>
        /// <param name="dateToSearch"></param>
        /// <returns>
        /// a List<Covid19> 
        /// 
        /// </returns>
        public List<Covid19> GetDataFromMongoDbByDate(string dateToSearch = null)
        {
            var coronaData = new Covid19();

            //filter date today
            if (string.IsNullOrEmpty(dateToSearch))
            {
                dateToSearch = DateTime.Now.ToString(dateOnlyFormat);
            }

            //configure filtering
            var filter_date = Builders<Covid19>.Filter.Or(
             Builders<Covid19>.Filter.Where(p => p.DadosDoDia.ToLower().Contains(dateToSearch.ToLower()))
             );
            //configure sorting
            var sort = Builders<Covid19>.Sort.Descending("Confirmados");

            //get data
            var result_local = GetDataFromMongoDB(coronaData, "node", filter_date, sort);
            return result_local;
        }

       
        public async Task<List<Covid19>> GetDataFromMongoDbByDateAsync(string date = null)
        {
            var coronaData = new Covid19();

            //filter date today
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Now.ToString(dateOnlyFormat);
            }

            //configure filtering
            var filter_date = Builders<Covid19>.Filter.Or(
             Builders<Covid19>.Filter.Where(p => p.DadosDoDia.ToLower().Contains(date.ToLower()))
             );
            //configure sorting
            var sort = Builders<Covid19>.Sort.Descending("Confirmados");

            //get data
            var result_local = await Task.Run(() => GetDataFromMongoDB(coronaData, "node", filter_date, sort));

            return result_local;
        }

       
        public string GetDbReportFromCountry(string country, string sortingByField)
        {
            var coronaData = new Covid19();
            var result = string.Empty;

            //configure filtering
            var filter = Builders<Covid19>.Filter.Or(
               Builders<Covid19>.Filter
               .Where(p => p.Local.ToLower().Contains(country.ToLower())
               ));

            //configure sorting
            var sort = Builders<Covid19>.Sort.Descending(sortingByField);
            //

            var result_local = _iMongoGeneric.GetdbCollectionGeneric(coronaData, "node", filter, sort);
            if (result_local.Any())
            {
                //use reflection to get data accordingly to sortingField
                foreach (var entityProp in result_local[0].GetType().GetProperties())
                {
                    if (entityProp.Name == sortingByField)
                    {
                        var entityValue = entityProp.GetValue(result_local[0]);
                        if (!string.IsNullOrEmpty(entityValue as string))
                        {
                            result = entityValue.ToString();
                        }
                    }
                }
            }

            return result;
        }

       
        public async Task<Result<string>> PostDataToMongoDBAsync()
        {
            var result = new Result<string>();
            string err = string.Empty;
            string res = string.Empty;
            try
            {
                var data = await GetCovid19CasesAsync().ConfigureAwait(false);
                if (data.Any())
                {
                    //check if has data from this day
                    var date = DateTime.Now.ToString(dateOnlyFormat);
                    var checkData = await GetDataFromMongoDbByDateAsync(date).ConfigureAwait(false);
                    if (!checkData.Any())
                    {
                        MongoDbActions mongoDb = new MongoDbActions();
                        List<string> logResults = new List<string>();
                        foreach (var item in data)
                        {
                            var insertResult = mongoDb.Save2Mongodb(item, "node");
                            logResults.Add(insertResult);
                        }
                        res = $"{logResults.Count} Posted";
                    }
                    else if (checkData.Any())
                    {
                        err = $"DB has {checkData.Count} data for {date}";
                    }
                }
            }
            catch (Exception exc)
            {
                err = exc.Message;
            }
            return (!string.IsNullOrEmpty(res) ? result.ResultResponse(
           res
             )
             : result.ResultError(err));
        }

        private static async Task<string> WebScrapingCore(string url)
        {
            var httpclient = new HttpClient();
           var urlConverted = new Uri(url);
            var html = await httpclient.GetStringAsync(urlConverted).ConfigureAwait(false);
            return html;
        }


        public async Task<List<Covid19>> GetCovid19CasesAsync()
        {
            var listResult = new List<Covid19>();
            string url = $"https://news.google.com/covid19/map?hl=pt-BR&gl=BR&ceid=BR:pt-419";
            var html = await WebScrapingCore(url).ConfigureAwait(false);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            foreach (var row in htmlDocument.DocumentNode.SelectNodes("//*[@class='pH8O4c']//tr"))
            {
                string local = null;
                var nodesTh = row.SelectNodes("th");
                if (nodesTh != null)
                {
                    local = nodesTh[0].InnerText;
                }
                var nodes = row.SelectNodes("td");
                if (nodes != null)
                {
                    var coronaData = new Covid19();
                    coronaData.Id = Guid.NewGuid().ToString();
                    coronaData.Local = local;
                    coronaData.Confirmados = nodes[0].InnerText;
                    coronaData.Mortes = nodes[4].InnerText;
                    coronaData.Recuperados = nodes[3].InnerText;
                    coronaData.DadosDoDia = DateTime.Now.ToString(dateFormat);
                    listResult.Add(coronaData);
                }
            }

            return listResult;
        }

       
    }
}