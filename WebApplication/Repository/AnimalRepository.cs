using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication.Models;
using WebApplication.Models.DTOs;
using System.Data.SqlClient;


namespace WebApplication.Repository
{
    public class AnimalRepository
    {
         public interface IAnimalRepository
    {
        Task<bool> Exist(int id);
        Task Create(Animal animal);
        Task<List<Animal>> GetAll(string orderBy);
        Task<bool> Update(string id, UpdateAnimal animal);
        Task<bool> Delete(string id);
    }
         
    public class AnimalsRepository: IAnimalRepository
    {
        private readonly IConfiguration _configration;

        public AnimalsRepository(IConfiguration configuration)
        {
            _configration = configuration;
        }
        
        public async Task<List<Animal>> GetAll(string orderBy)
        {
            var animals = new List<Animal>();
            
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"select * from animal order by {orderBy} asc";
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    animals.Add(new Animal
                    {
                        ID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Category = reader.GetString(3),
                        Area = reader.GetString(4)
                    });
                }
            }

            return animals;
        }

        public async Task<bool> Exist(int id)
        {
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = "select * from Animal where ID = @1";
                command.Parameters.AddWithValue("@1", id);
                await connection.OpenAsync();
                
                if (await command.ExecuteScalarAsync() is not null)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task Create(Animal animal)
        {
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = "insert into Animal (ID, Name, Description, Category, Area) values (@1,@2,@3,@4,@5)";
                command.Parameters.AddWithValue("@1", animal.ID);
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);
                
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> Update(string id, UpdateAnimal animal)
        {
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"update Animal set Name = @2, Description = @3, Category = @4, Area = @5 where ID = {id}";
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);
                
                await connection.OpenAsync();
                var rows = await command.ExecuteNonQueryAsync();
                return rows != 0;
            }
        }

        public async Task<bool> Delete(string id)
        {
            await using (var connection = new SqlConnection(_configration.GetConnectionString("Default")))
            {
                var command = connection.CreateCommand();
                command.CommandText = $"delete from Animal where ID = {id}";
                await connection.OpenAsync();
                
                var rows = await command.ExecuteNonQueryAsync();
                return rows != 0;
            }
        }
    }
    }
}