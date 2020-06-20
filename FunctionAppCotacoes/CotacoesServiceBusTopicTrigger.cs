using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using FunctionAppCotacoes.Models;
using FunctionAppCotacoes.Validators;
using FunctionAppCotacoes.Data;

namespace FunctionAppCotacoes
{
    public static class CotacoesServiceBusTopicTrigger
    {
        [FunctionName("CotacoesServiceBusTopicTrigger")]
        public static void Run([ServiceBusTrigger("topic-cotacoes0", "FunctionAppCotacoes", Connection = "AzureServiceBusConnection")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"CotacoesServiceBusTopicTrigger - Dados: {mySbMsg}");
            
            var cotacao = JsonSerializer.Deserialize<CotacaoMoeda>(mySbMsg,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            var validationResult = new CotacaoMoedaValidator().Validate(cotacao);
            
            if (validationResult.IsValid)
            {
                log.LogInformation($"CotacoesServiceBusTopicTrigger - Dados pós formatação: {mySbMsg}");
                CotacoesMoedasRepository.Save(cotacao);
                log.LogInformation("CotacoesServiceBusTopicTrigger - Cotação registrada com sucesso!");
            }
            else
            {
                log.LogInformation("CotacoesServiceBusTopicTrigger - Dados inválidos para a Cotação");
            }
        }
    }
}