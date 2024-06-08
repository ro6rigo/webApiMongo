using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System.Data;
using WebApiMongo.Models;

namespace WebApiMongo.Services
{
    public class ProdutoService
    {
        private readonly IMongoCollection<Produto> _produtoCollection;
        private readonly string _data_source = "datasource=localhost;username=root;password=root;database=apiMongo";
        private int _id;
        private string _nome;
        private string _marca;
        private string _tipo;
        private string _dataAtualizacao;

        public ProdutoService(IOptions<ProdutoDatabaseSettings> produtoServices)
        {
            var mongoClient = new MongoClient(produtoServices.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(produtoServices.Value.DatabaseName);

            _produtoCollection = mongoDatabase.GetCollection<Produto>(produtoServices.Value.ProdutoCollectionName);
        }

        public async Task<List<Produtos>> GetBanco()
        {
            MySqlConnection Conexao = new MySqlConnection(_data_source);
            string sql = "SELECT * FROM Produtos";
            MySqlCommand comando = new MySqlCommand(sql, Conexao);

            Conexao.Open();
            
            MySqlDataReader reader = comando.ExecuteReader();

            List<Produtos> result = new List<Produtos>();

            while(reader.Read())
            {                
                _id = reader.GetInt32(0);
                _nome = reader.GetString(1);
                _marca = reader.GetString(2);
                _dataAtualizacao = reader.GetString(3);
                _tipo = reader.GetString(4);
                Produtos item = new Produtos()
                {
                    id = _id,
                    nome = _nome,
                    marca = _marca,
                    tipo = _tipo,
                    dataAtualizacao = _dataAtualizacao
                };
                result.Add(item);
            }
            return result;
        }

         public async Task<List<Produto>> GetAsync() => 
            await _produtoCollection.Find(x => true).ToListAsync();

        public async Task<Produto> GetAsync(string id) =>
            await _produtoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateBanco(Produtos produto)
        {
            MySqlConnection Conexao = new MySqlConnection(_data_source);
            string sql = $"INSERT INTO Produtos(nome, marca, dataAtualizacao) VALUES ('{produto.nome}', '{produto.marca}', '{produto.dataAtualizacao}')";
            MySqlCommand comando = new MySqlCommand(sql, Conexao);

            Conexao.Open();

            comando.ExecuteReader();

            Conexao.Close();
        }
        public async Task Sync()
        {
            List<Produtos> lista = await GetBanco();
            foreach (var item in lista)
            {
                //Produto produtoAntigo = await _produtoCollection.Find(x => x.Nome == item.nome).FirstOrDefaultAsync();
                //if (produtoAntigo != null && item.dataAtualizacao != produtoAntigo.DataAtualizacao)
                //{
                //    produtoAntigo.Nome = item.nome;
                //    produtoAntigo.Marca = item.marca;
                //    produtoAntigo.DataAtualizacao = item.dataAtualizacao;
                //    await UpdateAsync(produtoAntigo.Id, produtoAntigo);
                //}
                //IMongoCollection<Produto> collection = _produtoCollection;
                List<WriteModel<Produto>> bulkOps = new List<WriteModel<Produto>>();

                Produto objeto = await _produtoCollection.Find(x => x.Nome == item.nome).FirstOrDefaultAsync();
                objeto.Nome = item.nome;
                objeto.Marca = item.marca;
                objeto.Tipo = item.tipo;
                objeto.DataAtualizacao = item.dataAtualizacao;

                FilterDefinition<Produto> filtro = Builders<Produto>.Filter.Eq((Produto x1) => x1.Nome, objeto.Nome);
                bulkOps.Add(new ReplaceOneModel<Produto>(filtro, objeto)
                {
                    IsUpsert = true
                });
                

                BulkWriteResult<Produto> bulkResult = await _produtoCollection.BulkWriteAsync(bulkOps);
            }
        }
        public async Task CreateAsync(Produto produto) =>
            await _produtoCollection.InsertOneAsync(produto);

        public async Task UpdateAsync(string id, Produto produto) =>
            await _produtoCollection.ReplaceOneAsync(x => x.Id == id, produto);

        public async Task RemoveAsync(string id) =>
            await _produtoCollection.DeleteOneAsync(x => x.Id == id);
    }
}
