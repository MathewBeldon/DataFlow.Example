using DataFlow.Example.Features;
using DataFlow.Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataFlow.Example.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParallelController : ControllerBase
    {
        private readonly SemaphoreProcess _semaphoreProcess;
        private readonly SingleProcess _singleProcess;
        private readonly DataFlowProcess _dataflowProcess;

        public ParallelController(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer)
        {
            _semaphoreProcess = new SemaphoreProcess(fakeRepository, fakeTransformer);
            _singleProcess = new SingleProcess(fakeRepository, fakeTransformer);
            _dataflowProcess = new DataFlowProcess(fakeRepository, fakeTransformer);
        }

        [HttpGet("dataflow/{amount}")]
        public async Task<IActionResult> StartDataflow(int amount)
        {
            var response = await _dataflowProcess.ProcessAsync(amount, CancellationToken.None);

            return Ok(response);
        }

        [HttpGet("semaphore/{amount}")]
        public async Task<IActionResult> StartSemaphore(int amount)
        {
            var response = await _semaphoreProcess.ProcessAsync(amount, CancellationToken.None);

            return Ok(response);
        }

        [HttpGet("single/{amount}")]
        public async Task<IActionResult> StartSingle(int amount)
        {
            var response = await _singleProcess.ProcessAsync(amount, CancellationToken.None);

            return Ok(response);
        }
    }
}