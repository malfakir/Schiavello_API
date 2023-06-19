using System.Text.Json;
using System.Text.Json.Serialization;

namespace Schiavello_API.Models
{
    public enum TodoStatus
    {
        New,                
        Active,
        Completed
    }
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Status { get; set; }

        // [JsonConverter(typeof(TodoStatusConverter))]
        //  public TodoStatus Status { get; set; }

    }
    public class TodoStatusConverter : JsonConverter<TodoStatus>
    {
        public override TodoStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string status = reader.GetString();

            if (Enum.TryParse<TodoStatus>(status, true, out var todoStatus))
                return todoStatus;

            return TodoStatus.New; // Default value if parsing fails
        }

        public override void Write(Utf8JsonWriter writer, TodoStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
