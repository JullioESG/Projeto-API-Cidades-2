using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Models;
using Projetos_Web_2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
 

namespace Projetos_Web_2.Controllers;

public class HomeController : Controller
{

    public async Task<IActionResult> Index()
    {
        var Cidades = new List<Cidade>();
        using (var httpClient = new HttpClient())
        {
    
            try
            {
                using (var resposta = await httpClient.GetAsync(
                $"http://localhost:5264/paises/55/estados/11/cidades"))
                {
                    if (resposta.StatusCode == HttpStatusCode.OK)
                    {
                        var conteudo = resposta.Content.ReadAsStringAsync().Result;

                        if(!String.IsNullOrEmpty(conteudo))
                        {
                           Cidades = JsonSerializer.Deserialize<List<Cidade>>(conteudo, new JsonSerializerOptions
                           {
                               PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                           });
                        }

                    }
                    else
                    {
                       ViewBag.ErroBusca = "Ocorreu um erro ao buscar as cidades."; 
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.ErroBusca = "Ocorreu um erro ao buscar as cidades.";
            }
        }

        return View(Cidades);
    }


    [HttpPost]
public async Task<IActionResult> Index(int idCidade, int estado, int pais, string nome, int populacao)
{
    Cidade cidade = new Cidade()
{
    Id = idCidade,
    Nome= nome,
    Populacao = populacao
};

    using (var httpClient = new HttpClient())
    {
        StringContent conteudo = new StringContent(
            JsonSerializer.Serialize(cidade)
            , Encoding.UTF8, "application/json");
        
        try
        {
            using (var resposta = await httpClient.PostAsync(
                $"http://localhost:5264/paises/{pais}/estados/{estado}/cidades", 
                conteudo))
            {
                if (resposta.StatusCode == HttpStatusCode.Created)
                {
                    ViewBag.MensagemGravacao = "Gravado com sucesso.";
                }
                else
                {
                    
                    ViewBag.MensagemGravacao = "Ocorreu um erro";
                }
            }
        }
        catch (Exception)
        {
            ViewBag.MensagemGravacao = "Ocorreu um erro";
        }
    }
    return await Index();
}

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
