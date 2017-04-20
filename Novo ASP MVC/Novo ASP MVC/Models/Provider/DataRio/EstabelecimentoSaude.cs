using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Novo_ASP_MVC.Models.Provider.DataRio
{
    public class EstabelecimentoSaude
    {
        public string Cnes { get; set; }
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public Endereco Endereco { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string DataCoordenadas { get; set; }
        public string CodigoAtividade { get; set; }
        public string AtividadeEnsino { get; set; }
        public string TipoUnidade { get; set; }
        public string TipoEstabelecimento { get; set; }
        public string CodigoNaturezaJuridica { get; set; }
        public string DescricaoNaturezaJuridica { get; set; }
    }
}