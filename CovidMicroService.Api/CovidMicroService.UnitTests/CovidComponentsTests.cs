using Xunit;
using CovidComponent.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using Services.Domain.Entities;
using System.Threading.Tasks;

namespace CovidMicroService.UnitTests
{
    public class CovidComponentsTests
    {

        readonly ICovidActions _ICovidLogic;
        readonly string dateOnlyFormat = "dd/MM/yyyy";
        public CovidComponentsTests()
        {
            var services = new ServiceCollection();
            services.AddCovidComponentConnector();

            var provider = services.BuildServiceProvider();

            _ICovidLogic = provider.GetService<ICovidActions>();


        }


        [Fact(DisplayName = "Cenario - get covid data")]
        [Trait("Category", "Success")]
        public void Get_Covid_Data_Test()
        {
            var covidObj = new Covid19();

            var local_Filter = "Brasil";

            var sortinfFiled = GetPropertyName(
    () => covidObj.Confirmados);

            var total = _ICovidLogic.GetDbReportFromCountry(local_Filter, sortinfFiled);

            Assert.NotEmpty(total);
            Assert.NotNull(total);
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member.Name;
        }

        [Fact(DisplayName = "Cenario - get covid data from mongodb filtered by date")]
        [Trait("Category", "Success")]
        public void TestGetDataFilterByDateFromMongoDB()
        {
            var date = "26/04/2020";

            var result = _ICovidLogic.GetDataFromMongoDbByDate(date);

            Assert.NotEqual(0, result.Count);

        }

        [Fact(DisplayName = "Cenario - get covid data from mongodb filtered by date async")]
        [Trait("Category", "Success")]
        public async Task TestGetDataFilterByDateFromMongoDBAsync()
        {
            var date = "26/04/2020";

            var result = await _ICovidLogic.GetDataFromMongoDbByDateAsync(date);

            Assert.NotEqual(0, result.Count);

        }
        [Fact(DisplayName = "Cenario - get covid data from mongodb filtered by date and country")]
        [Trait("Category", "Success")]
        public void TestGetDataFilterByLocalAndDateFromMongoDB()
        {

            var local_Filter = "Brasil";
            var result_local = _ICovidLogic.GetDataFromMongoDB(local_Filter, "Confirmados");

            Assert.NotEqual(0, result_local.Count);

        }

        [Fact(DisplayName = "Cenario - get covid data from mongodb filtered by confirmed cases")]
        [Trait("Category", "Success")]
        public void TestGetDataFilteredConfirmados()
        {
            var local_Filter = "Brasil";
            var sortinfFiled = "Confirmados";

            var total = _ICovidLogic.GetDbReportFromCountry(local_Filter, sortinfFiled);

            Assert.NotNull(total);

        }

        [Fact(DisplayName = "Cenario - get covid data from mongodb filtered by deads")]
        [Trait("Category", "Success")]
        public void TestGetDataFilteredMortes()
        {

            var local_Filter = "Brasil";
            var sortinfFiled = "Mortes";

            var total = _ICovidLogic.GetDbReportFromCountry(local_Filter, sortinfFiled);

            Assert.NotNull(total);

        }

        [Fact(DisplayName = "Cenario - Post data to mongoDB")]
        [Trait("Category", "Success")]
        public async Task TestPostDataToMongoDBAsync()
        {
            var data = await _ICovidLogic.PostDataToMongoDBAsync();

            Assert.NotNull(data.Response);

        }

        [Fact(DisplayName = "Cenario - get list of data async")]
        [Trait("Category", "Success")]
        public async Task TestGetListOfDataAsync()
        {

            var data = await _ICovidLogic.GetCovid19CasesAsync();
            Assert.NotNull(data);

        }

    }
}
