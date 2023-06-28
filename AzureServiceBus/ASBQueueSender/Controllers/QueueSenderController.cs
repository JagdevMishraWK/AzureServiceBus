using ASBQueueSender.Models;
using ASBQueueSender.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASBQueueSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueSenderController : ControllerBase
    {
        private readonly IQueueSenderService<QueueMessage> _commandPublisher;
        public QueueSenderController(IQueueSenderService<QueueMessage> commandPublisher) 
        {
            _commandPublisher = commandPublisher;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var correlationId = Guid.NewGuid().ToString();
            var documentCommand = new QueueMessage
            {
                QueueMessageId = Guid.NewGuid(),
                MessageDetails = "Hello_"+ correlationId,
                Amount = 500,
                MessageDate = DateTime.UtcNow,
            };

            var messageId = await _commandPublisher.Send(documentCommand, correlationId, props => {
                props.Add("ServiceName", "AccountingSvc");
            });
            return Ok(messageId);
        }
    }
}
