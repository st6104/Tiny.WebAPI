// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using AutoBogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.WebUtilities;
using NBomber.CSharp;
using Tiny.Application.Handlers.Commands;
using Tiny.Domain.AggregateModels.GLAccountAggregate;

var startCode = 1035838091;
var lorem = new Lorem(locale: "ko");
var fakes = new AutoFaker<GLAccount>()
    .StrictMode(true)
    .RuleFor(o => o.Code, f => startCode.ToString())
    .RuleFor(o => o.Name, f => lorem.Sentence(3))
    .RuleFor(o => o.PostableId, f => f.PickRandom<Postable>(Postable.List).Value)
    .RuleFor(o => o.AccountingTypeId, f => f.PickRandom<AccountingType>(AccountingType.List).Value)
    .Ignore(o => o.Id)
    .Ignore(o => o.AccountingType)
    .Ignore(o => o.Balance)
    .Ignore(o => o.Deleted);

var httpClientForAdd = new HttpClient();
httpClientForAdd.DefaultRequestHeaders.Add("X-Tenant-ID", "1000");

var httpClientForGet = new HttpClient();
httpClientForGet.DefaultRequestHeaders.Add("X-Tenant-ID", "1000");


var addScenario = Scenario.Create("Add GLAccounts", async context =>
{
    var glAccount = fakes.Generate(1)!.First();
    startCode++;
    var requestObj = new GLAccountAddCommand(startCode.ToString(), glAccount.Name,
        glAccount.PostableId, glAccount.AccountingTypeId);

    var response =
        await httpClientForAdd.PostAsJsonAsync("http://localhost:5000/api/GLAccount", requestObj);

    return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
}).WithLoadSimulations(
    Simulation.Inject(rate: 20, interval: TimeSpan.FromSeconds(1),
        during: TimeSpan.FromSeconds(120))
);

var getScenario = Scenario.Create("Get GLAccounts Random", async context =>
{
    var random = new Random(DateTime.Now.Nanosecond);
    var skipCount = random.Next(0, 6000);
    var getCount = random.Next(10, 150);

    var parameters = new Dictionary<string, string?>()
    {
        ["skipCount"] = skipCount.ToString(), ["queryCount"] = getCount.ToString()
    };

    var uri = QueryHelpers.AddQueryString("http://localhost:5000/api/GLAccount", parameters);
    var response = await httpClientForGet.GetAsync(uri);

    return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
}).WithLoadSimulations(
    Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1),
        during: TimeSpan.FromSeconds(120))
);


NBomberRunner.RegisterScenarios(addScenario, getScenario).Run();
