using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoCrud
{
    public abstract class SimpleAutoCrudController<Model, PrimaryKey> : ControllerBase,
        AutoCrudController<Model, PrimaryKey> where Model : class
    {
        private readonly ILogger _logger;
        private readonly IAutoCrudRepository<Model, PrimaryKey> _repository;

        protected SimpleAutoCrudController(IAutoCrudRepository<Model, PrimaryKey> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public bool EnableLogging { get; set; } = true;


        [HttpGet]
        public virtual async Task<IActionResult> GetModels([FromQuery] int page = 0)
        {
            Log($"Fetching {GetModelNameInPluralForLogging()} on page : {page}");
            var models = await _repository.GetPageAsync(page, GetPageSize());
            return Ok(models);
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> FindModel([FromRoute] PrimaryKey key)
        {
            Log($"Fetching {GetModelNameInSingularForLogging()} with key : {key}");
            var model = await _repository.FindAsync(key);
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model model)
        {
            Log($"Creating new {GetModelNameInSingularForLogging()}");
            var createdModel = await _repository.CreateAsync(model);
            await PostProcessCreate(model);
            return Created(GetCreatedModelUrl(createdModel), createdModel);
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Update([FromRoute] PrimaryKey key, [FromBody] Model model)
        {
            Log($"Updating {GetModelNameInSingularForLogging()} with key : {key}");
            SetPrimaryKeyValueToModel(model, key);
            var editedModel = await _repository.UpdateAsync(model);
            await PostProcessUpdate(model);
            return Ok(editedModel);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete([FromRoute] PrimaryKey key)
        {
            Log($"Deleting {GetModelNameInSingularForLogging()} with key : {key}");
            var deletedModel = await _repository.DeleteAsync(key);
            await PostProcessDelete(deletedModel);
            return NoContent();
        }

        protected virtual Task PostProcessCreate(Model model)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostProcessUpdate(Model model)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostProcessDelete(Model model)
        {
            return Task.CompletedTask;
        }

        private void Log(string message)
        {
            if (EnableLogging) _logger.LogInformation(message);
        }

        protected virtual string GetModelNameInSingularForLogging()
        {
            return "model";
        }

        protected virtual string GetModelNameInPluralForLogging()
        {
            return "models";
        }

        protected abstract int GetPageSize();

        protected abstract string GetCreatedModelUrl(Model createdModel);

        protected abstract void SetPrimaryKeyValueToModel(Model model, PrimaryKey primaryKey);
    }
}