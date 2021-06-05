using System;
using APIDOTNET.DATA.Collections;
using APIDOTNET.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APIDOTNET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        DATA.MongoDB _mongoDB;

        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(DATA.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.Datanascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);

            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.Datanascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.Datanascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));

            return Ok("Atualizado Com sucesso");
        }

        [HttpDelete("{dataNascimento}")]
        public ActionResult Delete(string dataNascimento)
        {
                        
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == Convert.ToDateTime(dataNascimento)));

            return Ok("Atualizado Com sucesso");
        }

        
    }
}