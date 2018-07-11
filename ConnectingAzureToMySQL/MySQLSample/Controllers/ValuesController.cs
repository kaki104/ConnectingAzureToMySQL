using System.Data;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using MySQLSample.Helpers;
using Newtonsoft.Json.Linq;
using Swashbuckle.Swagger.Annotations;

namespace MySQLSample.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [SwaggerOperation("GetAll")]
        public async Task<DataSet> Get()
        {
            return await MySQLHelper.Instance.ExecuteAsync("SELECT * FROM Employees");
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        ///     연결 테스트
        /// </summary>
        /// <returns></returns>
        [Route("~/getconnecttest")]
        public async Task<string> GetConnectTest()
        {
            return await MySQLHelper.Instance.GetConnectTestAsync();
        }

        // POST api/values
        [SwaggerOperation("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]
        public async void Post([FromBody] JObject value)
        {
            var query = value["value"].ToString();
            var result = await MySQLHelper.Instance.ExecuteNonQueryAsync(query);
        }

        // PUT api/values/5
        [SwaggerOperation("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [SwaggerOperation("Delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public void Delete(int id)
        {
        }
    }
}