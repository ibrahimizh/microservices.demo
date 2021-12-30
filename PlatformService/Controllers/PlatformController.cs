using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadModel>> GetPlatforms()
        {
            var platforms = _repo.GetPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadModel>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatform")]
        public ActionResult<PlatformReadModel> GetPlatform(int id)
        {
            var platform = _repo.GetPlatform(id);
            if(platform != null)
                return Ok(_mapper.Map<PlatformReadModel>(platform));

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadModel>> AddPlatform([FromBody] CreatePlatformModel model)
        {
            var platform = _mapper.Map<Platform>(model);
            _repo.CreatePlatform(platform);
            _repo.SaveChanges();
            var platformReadModel = _mapper.Map<PlatformReadModel>(platform);
            
            try
            {
                await _commandDataClient.SendPlatformToClient(platformReadModel);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"===>>> Could not send synchronously : {ex.Message}");
            }
            
            return CreatedAtAction(nameof(GetPlatform), new { Id = platformReadModel.Id }, platformReadModel);
        }
    }
}