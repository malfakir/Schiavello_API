using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Schiavello_API.Models;

namespace Schiavello_API.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly string _filePath;

        public TodoController(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(environment.ContentRootPath, "DataStore", "todos.json");
           
        }

        [HttpGet]
        public IActionResult Get()
        {
            var todoItems = ReadTodoItems();
            return Ok(todoItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todoItems = ReadTodoItems();
            var todoItem = todoItems.Find(item => item.Id == id);
            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        public IActionResult Create(TodoItem todoItem)
        {
            var todoItems = ReadTodoItems();
            var maxId = 0;
            if (todoItems.Count > 0)
                maxId = todoItems.Max(item => item.Id);
            
            todoItem.Id = maxId + 1;
            todoItems.Add(todoItem);
            WriteTodoItems(todoItems);

            return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem updatedTodoItem)
        {
            var todoItems = ReadTodoItems();
            var index = todoItems.FindIndex(item => item.Id == id);
            if (index == -1)
                return NotFound();

            updatedTodoItem.Id = id;
            todoItems[index] = updatedTodoItem;
            WriteTodoItems(todoItems);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todoItems = ReadTodoItems();
            var index = todoItems.FindIndex(item => item.Id == id);
            if (index == -1)
                return NotFound();

            todoItems.RemoveAt(index);
            WriteTodoItems(todoItems);

            return NoContent();
        }

        private List<TodoItem> ReadTodoItems()
        {
            var jsonContent = System.IO.File.ReadAllText(_filePath);
            var todoItems = JsonSerializer.Deserialize<List<TodoItem>>(jsonContent);
            return todoItems ?? new List<TodoItem>();
        }

        private void WriteTodoItems(List<TodoItem> todoItems)
        {
            var jsonContent = JsonSerializer.Serialize(todoItems);
            System.IO.File.WriteAllText(_filePath, jsonContent);
        }
    }
}
