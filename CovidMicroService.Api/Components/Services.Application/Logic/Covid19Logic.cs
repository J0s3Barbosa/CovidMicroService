using Services.Application.Interfaces;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Application.Logic
{
    public class Covid19Logic : ICovid19Logic
    {
        readonly ICovid19 _ICovid19;
        public Covid19Logic(ICovid19 iCovid19)
        {
            this._ICovid19 = iCovid19;
        }

        public async Task<List<Covid19>> ListAsync()
        {
            List<Covid19> listCovid19Data = await _ICovid19.ListAsync();

            return listCovid19Data;
        }

        public async Task<List<Covid19>> ListAsync(string country)
        {

            IEnumerable<Covid19> listCovid19Data = await ListAsync();

            if (!string.IsNullOrEmpty(country))
                listCovid19Data = await Task.Run(() => listCovid19Data
                .Where(x => x.Local.Contains(country, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DadosDoDia)
                );

            return listCovid19Data.ToList();
        }

        /// <summary>
        /// get Covid19 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Covid19> GetEntityAsync(Guid id)
        {
            Covid19 Covid19 = await _ICovid19.GetEntityAsync(id);

            return Covid19;
        }

        public async Task<int> AddAsync(Covid19 Covid19)
        {
            return await this._ICovid19.AddAsync(Covid19);
        }

        public async Task<Result<Covid19>> AddCovidAsync(Covid19 covid)
        {
            var result = new Result<Covid19>();

            try
            {

                var Covid19 = new Covid19
                {
                    Id = Guid.NewGuid().ToString(),
                    Confirmados = covid.Confirmados,
                    DadosDoDia = covid.DadosDoDia,
                    Local = covid.Local,
                    Mortes = covid.Mortes,
                    Recuperados = covid.Recuperados,
                };

                var save = await this.AddAsync(Covid19);
                return (save > 0 ? result.ResultResponse(
                   this.ListAsync().GetAwaiter().GetResult().First(x => x.Id == Covid19.Id))
                    : throw new Exception());

            }
            catch (Exception exc)
            {
                return result.ResultError(exc.Message);
            }


        }

        public async Task<int> UpdateAsync(Covid19 Covid19)
        {
            return await this._ICovid19.UpdateAsync(Covid19);
        }
        /// <summary>
        /// update Covid19
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Covid19"></param>
        /// <returns></returns>
        public async Task<Result<Covid19>> UpdateAsync(Guid id, Covid19 Covid19)
        {
            var result = new Result<Covid19>();

            var resource = await this.GetEntityAsync(id);
            if (resource == null) return result.ResultError("Resource not found!");

            //use reflection to check wich properties have changed and set proper value
            foreach (var entityProp in Covid19.GetType().GetProperties())
            {
                var entityValue = entityProp.GetValue(Covid19);

                if (entityProp.Name != "Id" && (!string.IsNullOrEmpty(entityValue as string) || entityValue != null))
                {
                    foreach (var resourceProp in resource.GetType().GetProperties().Where(x => x.Name == entityProp.Name))
                    {
                        var resourceValue = resourceProp.GetValue(resource);

                        if (string.IsNullOrEmpty(resourceValue as string) && entityValue != null || !resourceValue.ToString().ToLower().Equals(entityValue.ToString().ToLower()))
                        {
                            resourceProp.SetValue(resource, entityValue, null);
                        }
                    }
                }
            }


            if (await this.UpdateAsync(resource) <= 0) return result.ResultError("The resource was not updated!");
            else return result.ResultResponse(resource);
        }

        private static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }

        public async Task<int> DeleteAsync(Covid19 Covid19)
        {
            return await this._ICovid19.DeleteAsync(Covid19);
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            var Covid19 = await this.GetEntityAsync(id);
            if (Covid19 == null) return null;
            return await this.DeleteAsync(Covid19);
        }

    }

}

