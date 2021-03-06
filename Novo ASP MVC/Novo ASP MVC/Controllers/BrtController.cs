﻿using Newtonsoft.Json;
using Novo_ASP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers.Transporte
{
    public class BrtController : ApiController
    {
        // GET: api/Brt
        public async Task<IEnumerable<Veiculo>> Get()
        {
            var veiculos = new List<Veiculo>();
            HttpClient client = new HttpClient();
            var r = await client.GetAsync("http://webapibrt.rio.rj.gov.br/api/v1/brt");
            var json = await r.Content.ReadAsStringAsync();
            var vs = JsonConvert.DeserializeObject<Models.Provider.DataRio.BrtVeiculos>(json);
            foreach (var item in vs.veiculos)
            {
                if (veiculos.Find(v=>v.Linha==item.linha.ToString())==null) // Adicionar apenas linhas unicas
                veiculos.Add(new Veiculo
                {
                    Codigo = item.codigo,
                    Linha = item.linha.ToString(),
                    Latitude = item.latitude,
                    Longitude = item.longitude,
                    DataHora = item.datahora.ToString(),
                    Velocidade = item.velocidade.ToString()
                });
            }
            return veiculos;
        }

        // GET: api/Brt/5
        public async Task<IEnumerable<Veiculo>> Get(string id)
        {
            var veiculos = new List<Veiculo>();
            HttpClient client = new HttpClient();
            var r = await client.GetAsync("http://webapibrt.rio.rj.gov.br/api/v1/brt");
            var json = await r.Content.ReadAsStringAsync();
            var vs = JsonConvert.DeserializeObject<Models.Provider.DataRio.BrtVeiculos>(json);
            foreach (var item in vs.veiculos)
            {
                if (item.linha.ToString() != id.ToString()) continue;
                veiculos.Add(new Veiculo
                {
                    Codigo = item.codigo,
                    Linha = item.linha.ToString(),
                    Latitude = item.latitude,
                    Longitude = item.longitude,
                    DataHora = item.datahora.ToString(),
                    Velocidade = item.velocidade.ToString()
                });
            }
            return veiculos;
        }

        // POST: api/Brt
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Brt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Brt/5
        public void Delete(int id)
        {
        }
    }


    

}
