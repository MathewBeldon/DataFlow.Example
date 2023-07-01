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
        private DataFlowProcess _dataflowProcess;

        public ParallelController(
            IFakeRepository fakeRepository,
            IFakeTransformer fakeTransformer,
            IFakeTelemetry fakeTelemetry)
        {
            _semaphoreProcess = new SemaphoreProcess(fakeRepository, fakeTransformer, fakeTelemetry);
            _singleProcess = new SingleProcess(fakeRepository, fakeTransformer);
            _dataflowProcess = new DataFlowProcess(fakeRepository, fakeTransformer, fakeTelemetry);
        }

        [HttpGet("dataflow/{amount}")]
        public IActionResult StartDataflow(int amount)
        {

            var response = _dataflowProcess.ProcessAsync(amount, CancellationToken.None);

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