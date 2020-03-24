using Alschy.LocalizeServer.Common.Models;
using Alschy.LocalizeServer.Common.Services;
using Alschy.LocalizeServer.Core.Management.Models;
using Alschy.LocalizeServer.Core.Management.Services;
using Alschy.LocalizeServer.MongoDB.Mangement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizeController : ControllerBase
    {
        private readonly ILocalizeService localizeService;
        private readonly ILocalizeManagementService managementService;

        public LocalizeController(ILocalizeService localizeService, ILocalizeManagementService managementService)
        {
            this.localizeService = localizeService;
            this.managementService = managementService;
        }

        public async Task<ActionResult<ResourceResponseModel>> Localize(ResourceRequestModel model, CancellationToken cancel)
        {
            return await localizeService.LocalizeAsync(model, cancel);
        }

        [HttpGet("{key}/{culture}")]
        public async Task<ActionResult<ResourceResponseModel>> Localize(string key, string culture, CancellationToken cancel, string application = null)
        {
            var model = new ResourceRequestModel(key, culture, application);
            return await Localize(model, cancel);
        }

        [HttpPost("manage")]
        public async Task<IActionResult> ManageLocalization([FromBody]ResourceModifyRequestModel model, CancellationToken cancel)
        {
            try
            {
                await managementService.ModifyResourceItem(model, cancel);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

    }
}