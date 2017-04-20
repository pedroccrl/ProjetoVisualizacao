using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers.Api.CKAN
{
    [RoutePrefix("api/ckan/busca")]
    public class BuscaController : ApiController
    {
        [HttpGet]
        [Route("{dominio}")]
        public async Task<IEnumerable<Filtro>> Get(string dominio)
        {
            dominio = "http://" + dominio;
            var http = new HttpClient();
            var html = await http.GetStringAsync(dominio + "/dataset" + "?_organization_limit=0&_groups_limit=0&_tags_limit=0&_res_format_limit=0");

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            List<Filtro> filtros = new List<Filtro>();

            foreach (var section in htmlDoc.DocumentNode.SelectNodes("//div[@class='filters']/div/section"))
            {
                var tipo = section.ChildNodes.FindFirst("h2").InnerText.Trim();

                var secoes = new List<Secao>();

                var sectionDoc = new HtmlDocument();
                sectionDoc.LoadHtml(section.InnerHtml);

                foreach (var item in sectionDoc.DocumentNode.SelectNodes("//li[@class='nav-item']"))
                {
                    var att = item.ChildNodes[1].Attributes;
                    var url = WebUtility.HtmlDecode(att[0].Value);
                    var titulo = att[1].Value;

                    if (titulo == string.Empty)
                    {
                        titulo = item.InnerText.Trim();
                    }

                    secoes.Add(new Secao { Url = dominio + url, Nome = titulo });
                }

                filtros.Add(new Filtro { Secoes = secoes, Tipo = tipo });
            }

            return filtros;
        }

        [HttpGet]
        [Route("{dominio}/{query}")]
        public async Task<IEnumerable<DatasetItem>> Get(string dominio, string query)
        {
            var host = dominio;
            dominio = "http://" + dominio;
            var url = dominio + "/" + query + Request.RequestUri.Query;
            var http = new HttpClient();
            var html = await http.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var datasets = new List<DatasetItem>();

            foreach (var datasetItem in htmlDoc.DocumentNode.SelectNodes("//li[@class='dataset-item']"))
            {
                var datasetItemDoc = new HtmlDocument();
                datasetItemDoc.LoadHtml(datasetItem.InnerHtml);

                var content = datasetItemDoc.DocumentNode.SelectSingleNode("/div");
                var titulo = content.ChildNodes.FindFirst("h3").InnerText.Trim();
                var res = content.ChildNodes.FindFirst("h3").ChildNodes.FindFirst("a").Attributes["href"].Value;
                var descricao = content.ChildNodes.FindFirst("div").InnerText;
                descricao = WebUtility.HtmlDecode(descricao.Trim());

                var resList = new List<Resource>();

                foreach (var resourceItem in datasetItemDoc.DocumentNode.SelectNodes("/ul/li"))
                {
                    var resUrl = resourceItem.ChildNodes.FindFirst("a").Attributes["href"].Value;
                    var resFormat = resourceItem.ChildNodes.FindFirst("a").Attributes["data-format"].Value;
                    resList.Add(new Resource { Formato = resFormat, Url = resUrl });
                }

                datasets.Add(new DatasetItem { Host = host, Titulo = titulo, Descricao = descricao, Resources = resList, ResourceUrl = res });
            }

            return datasets;
        }

        [HttpGet]
        [Route("{dominio}/{dataset}/{resource}")]
        public async Task<DatasetItem> Get(string dominio, string dataset, string resource)
        {
            var host = dominio;
            dominio = "http://" + dominio;
            var url = dominio + "/dataset/" + resource;
            var http = new HttpClient();
            var html = await http.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var content = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='module-content']");

            var titulo = content.ChildNodes.FindFirst("h1").InnerText.Trim();
            var descricao = content.ChildNodes.FindFirst("div").InnerText.Trim();

            var resourceList = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='module-content']/section[@id='dataset-resources']");
            var resListDoc = new HtmlDocument();
            resListDoc.LoadHtml(resourceList.InnerHtml);

            var resList = new List<Resource>();

            foreach (var resourceItem in resListDoc.DocumentNode.SelectNodes("/ul/li"))
            {
                var resItemDoc = new HtmlDocument();
                resItemDoc.LoadHtml(resourceItem.InnerHtml);

                var formato = resItemDoc.DocumentNode.SelectSingleNode("//span[@class='format-label']").Attributes["data-format"].Value;

                var resUrl = resItemDoc.DocumentNode.SelectSingleNode("//a[@class='resource-url-analytics']").Attributes["href"].Value;

                resList.Add(new Resource { Formato = formato, Url = resUrl });
            }

            var datasetItem = new DatasetItem { Host = dominio, Titulo = titulo, Descricao = descricao, Resources = resList };
            return datasetItem;
        }
    }

    public class Filtro
    {
        public List<Secao> Secoes { get; set; }
        public string Tipo { get; set; }

    }

    public class Secao
    {
        public string Url { get; set; }
        public string Nome { get; set; }
    }

    public class DatasetItem
    {
        public string Host { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string ResourceUrl { get; set; }
        public List<Resource> Resources { get; set; }
    }

    public class Resource
    {
        public string Url { get; set; }
        public string Formato { get; set; }
    }
}
