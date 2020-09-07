using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AutoCrud
{
    public interface AutoCrudController<Model, PrimaryKey> where Model : class
    {
        Task<IActionResult> GetModels(int page);
        Task<IActionResult> FindModel(PrimaryKey key);
        Task<IActionResult> Create(Model model);
        Task<IActionResult> Update(PrimaryKey key, Model model);
        Task<IActionResult> Delete(PrimaryKey key);
    }
}