using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoCrud
{
    public abstract class AutoCrudControllerBase<Entity, PrimaryKey> : ControllerBase where Entity : class
    {
        protected readonly ILogger _logger;
        protected readonly IAutoCrudRepository<Entity, PrimaryKey> _repository;

        private readonly IDictionary<CrudActionType, Exception> _removedActions;

        protected AutoCrudControllerBase(IAutoCrudRepository<Entity, PrimaryKey> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
            _removedActions = new Dictionary<CrudActionType, Exception>();
        }

        protected AutoCrudControllerBase<Entity, PrimaryKey> RemoveAction(CrudActionType action,
            Exception exception = null)
        {
            var toThrowException = exception ?? new UnsupportedActionException();
            _removedActions.Add(action, toThrowException);
            return this;
        }

        protected bool IsActionRemoved(CrudActionType action)
        {
            return _removedActions.ContainsKey(action);
        }

        protected void ThrowIfRemoved(CrudActionType action)
        {
            if (IsActionRemoved(action))
            {
                var exception = _removedActions[action];
                throw exception;
            }
        }


        public bool EnableLogging { get; set; } = true;

        protected virtual string GetEntityNameInSingularForLogging()
        {
            return "entity";
        }

        protected virtual string GetEntityNameInPluralForLogging()
        {
            return "entities";
        }

        protected void Log(string message)
        {
            if (EnableLogging) _logger.LogInformation(message);
        }

        protected abstract int GetPageSize();

        protected abstract string GetCreatedEntityUri(Entity createdEntity);

        protected abstract void SetPrimaryKeyValueToEntity(Entity entity, PrimaryKey primaryKey);


        protected void BeforeCreate()
        {
            ThrowIfRemoved(CrudActionType.CREATE);
        }

        protected void BeforeUpdate()
        {
            ThrowIfRemoved(CrudActionType.UPDATE);
        }

        protected void BeforeDelete()
        {
            ThrowIfRemoved(CrudActionType.DELETE);
        }

        protected void BeforeGetPage()
        {
            ThrowIfRemoved(CrudActionType.GET_PAGE);
        }

        protected void BeforeFindEntity()
        {
            ThrowIfRemoved(CrudActionType.FIND_ENTITY);
        }
    }
}