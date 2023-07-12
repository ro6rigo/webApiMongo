using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiMongo.Models;
using WebApiMongo.Services;

namespace WebApiMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _produtoService;
        public ProdutosController(ProdutoService produtosService)
        {
            _produtoService = produtosService;
        }

        [HttpGet]
        public async Task<List<Produto>> GetProdutos() =>
            await _produtoService.GetAsync();
        
        [HttpPost]
        public async Task<Produto> PostProduto(Produto produto)
        {
            await _produtoService.CreateAsync(produto);
            return produto;
        }
    }
}
