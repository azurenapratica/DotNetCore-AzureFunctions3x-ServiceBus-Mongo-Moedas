using System;
using MongoDB.Driver;
using FunctionAppCotacoes.Models;
using FunctionAppCotacoes.Document;

namespace FunctionAppCotacoes.Data
{
    public static class CotacoesMoedasRepository
    {
        public  static void Save(CotacaoMoeda cotacao)
        {
            var client = new MongoClient(
                Environment.GetEnvironmentVariable("MongoConnection"));
            IMongoDatabase db = client.GetDatabase(
                Environment.GetEnvironmentVariable("MongoDatabase"));

            var historico =
                db.GetCollection<CotacaoMoedaDocument>(
                    Environment.GetEnvironmentVariable("MongoCollection"));

            var horario = DateTime.Now;
            var document = new CotacaoMoedaDocument();
            document.HistLancamento = cotacao.Sigla + horario.ToString("yyyyMMdd-HHmmss");
            document.Sigla = cotacao.Sigla;
            document.Valor = cotacao.Valor.Value;
            document.Data = horario.ToString("yyyy-MM-dd HH:mm:ss");

            historico.InsertOne(document);
        }        
    }
}