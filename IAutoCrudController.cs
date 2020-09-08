using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AutoCrud
{
    public interface IAutoCrudControllerAsync<in Entity, in PrimaryKey> where Entity : class
    {
        Task<IActionResult> GetPage(int page);
        Task<IActionResult> Find(PrimaryKey key);
        Task<IActionResult> Create(Entity entity);
        Task<IActionResult> Update(PrimaryKey key, Entity entity);
        Task<IActionResult> Delete(PrimaryKey key);
    }

    public interface IAutoCrudController<in Entity, in PrimaryKey> where Entity : class
    {
        IActionResult GetPage(int page);

        IActionResult Find(PrimaryKey key);

        IActionResult Create(Entity entity);

        IActionResult Update(PrimaryKey key, Entity entity);

        IActionResult Delete(PrimaryKey key);
    }
}