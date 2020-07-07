using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Application.Extensions;
using Services.Application.Interfaces;
using Services.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CovidMicroService.Api.Controllers
{
    //[Authorize]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [SwaggerGroup("Covid19 Api")]
    [ApiController, Route("api/v{version:apiVersion}/[controller]"), Produces("application/json")]
    public class CovidController : ControllerBase
    {
        private readonly ICovid19ServiceLogic _IlistCovid19Data;

        public CovidController(ICovid19ServiceLogic ilistCovid19Data)
        {
            _IlistCovid19Data = ilistCovid19Data;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<Covid19>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IEnumerable<Covid19>> Covid19DataAsync()
        {
            IEnumerable<Covid19> data = await _IlistCovid19Data.ListOfCovid19DataAsync();

            Request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            Request.HttpContext.Response.Headers.Add("X-Total-Count", data?.Count().ToString());

            return data;
        }

        [HttpGet("/api/v{version:apiVersion}/GetDataByCountryFromDB/{country}")]
        [ProducesResponseType(typeof(IList<Covid19>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IEnumerable<Covid19> GetDataByCountryFromDB(string country)
        {
            IEnumerable<Covid19> data = _IlistCovid19Data.GetDataByCountryFromDB(country);

            Request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            Request.HttpContext.Response.Headers.Add("X-Total-Count", data?.Count().ToString());

            return data;
        }

        [HttpGet("/api/v{version:apiVersion}/GetDataByDateFromDB/{date}")]
        [ProducesResponseType(typeof(IList<Covid19>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IEnumerable<Covid19>> GetDataByDateFromDBAsync(string date)
        {
            IEnumerable<Covid19> data = await _IlistCovid19Data.GetDataByDateFromDBAsync(date);

            Request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            Request.HttpContext.Response.Headers.Add("X-Total-Count", data?.Count().ToString());

            return data;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Covid19), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<bool>> SendDataToMongo()
        {
            var result = await _IlistCovid19Data.PostCovid19DataToMongoDBAsync();

            if (result.Errors.Count > 0) return await Task.FromResult<ActionResult>(this.UnprocessableEntity(result.Errors.First()));
            else return await Task.FromResult<ActionResult>(this.Created(result.Response.ToString(), result.Response));
        }
    }
}