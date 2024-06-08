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
        [Route("SyncBancoMongo")]
        [HttpGet]
        public async Task SyncBancoMongo() =>
            await _produtoService.Sync(); 
        [Route("GetBanco")]
        [HttpGet]
        public async Task<List<Produtos>> GetBanco() =>
            await _produtoService.GetBanco();
        [HttpGet]
        public async Task<List<Produto>> GetProdutos() =>
            await _produtoService.GetAsync();

        [Route("PostBanco")]
        [HttpPost]
        public async Task PostBanco(Produtos produto) =>
            await _produtoService.CreateBanco(produto);
        
        [HttpPost]
        public async Task<Produto> PostProduto(Produto produto)
        {
            await _produtoService.CreateAsync(produto);
            return produto;
        }
        [HttpPut]
        public async Task PutProduto(string id, Produto produto)
        {
            await _produtoService.UpdateAsync(id, produto);
        }
        [HttpDelete]
        public async Task DeleteProduto(string id)
        {
            await _produtoService.RemoveAsync(id);
        }
    }
}
