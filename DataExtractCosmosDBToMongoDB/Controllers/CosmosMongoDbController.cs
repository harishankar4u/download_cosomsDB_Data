using DataExtractCosmosDBToMongoDB.Handler;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataExtractCosmosDBToMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CosmosMongoDbController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CosmosMongoDbController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// check the console log for get downloaded file path
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCosmosdbJsonFileDownload")]
        public async Task<bool> GetAll(string connectionString,string databaseName,string containerName)
        {
            var resp = await _mediator.Send(new GetCosmosdbJsonFileDownloadQuery(connectionString, databaseName, containerName));
            return resp;
        }


        /// <summary>
        /// this end point not working properly
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertDataInMongoDb")]
        public async Task<bool> post(IFormFile file)
        {
            var resp = await _mediator.Send(new InsertDataInMongoDbCommand(file));
            return resp;
        }
    }
}
