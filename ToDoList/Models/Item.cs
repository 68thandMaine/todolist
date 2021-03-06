using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    private int _id;


    public Item (string description, int id = 0)
    {
      _description = description;
      _id = id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetId()
    {
      return _id;
            // To fail GetId - add return 0; and comment out the private id property and id prperty in the item constructor.
    }
    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> { };
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        Item newItem = new Item(itemDescription,  itemId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      // To fail Get All Empty List method use this code
      // Item dummyItem = new Item("dummy item");
      // List<Item> allItems = new List<Item> { dummyItem };
      return allItems;
      // Get All Returns Items will fail until Save is running on objects.
      // Add object.Save() after Save method code is added to make it pass.
    }
    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Item Find(int id)
    {
      //Open a connectoin to the database
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //Pass a command to SQL
      var cmd = conn.CreateCommand() as MySqlCommand;
      //Remember to use @thisId for the placeholder
      cmd.CommandText = @"Select * FROM `items` WHERE id = (@thisId);";

      //Create the MySqlparameter object for feeding VALUES
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;

      //Associate the parameter object with the SqlCommand. Because the find method returns something we need to use ExecuteReader() and cast the data as a MySqlDataReader.
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      //Initiate reading the database with a while loop
      int itemId = 0;
      string itemDescription = "";
      while (rdr.Read())
      {
        itemId = rdr.GetInt32(0);
        itemDescription = rdr.GetString(1);
      }

      //Create and return a new Item object with the values that were located
      Item foundItem = new Item(itemDescription, itemId);

      //Close a connection to the database
      conn.Close();
      if(conn!=null)
      {
        conn.Dispose();
      }

      return foundItem;
      // Temporarily returning dummy item to get beyond compiler errors, until we refactor to work with database.
      // Item dummyItem = new Item("dummy item");
      // return dummyItem;
    }
    public void Edit(string newDescription)
    {
      //Open a connection to the database: /////////////
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //////////////////////////////////////////////////

      //Create a new MySqlCommand object and enter a SQL statement
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";
      ///////////////////////////////////////////////////////

      // We now pass the parameter placeholders into MySqlParameter objects //////
      // Obect 1:
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      // Object 2:
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newDescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);
      ///////////////////////////////////////////////////////////////////////////

      // Execute the command with an ExecuteNonQuery() /////////
      cmd.ExecuteNonQuery();
      /////////////////////////////////////////////////////

      // Reset the Object property on the app ////
      _description = newDescription;
      //////////

      //Close the connection to the database: ////////
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
      public override bool Equals(System.Object otherItem)
      {
        if(!(otherItem is Item))
        {
          return false;
        }
        else
        {
          Item newItem = (Item) otherItem;
          bool idEquality = (this.GetId() == newItem.GetId());
          bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
          return (idEquality && descriptionEquality );
        }
      }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description) VALUES (@ItemDescription);";
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@ItemDescription";
      description.Value = this._description;
      cmd.Parameters.Add(description);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Category> GetCategories()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT category_id FROM categories_id  WHERE item_id = @itemId;";
      MySqlParameter itemIdParameter = new MySqlParameter();
      itemIdParameter.ParameterName = @"itemId";
      itemIdParameter.Value=_id;
      cmd.Parameters.Add(itemIdParameter);
      var rdr = cmd.ExecuteReader() as My MySqlDataReader;
      List<int> categoryIds = new List<int> {};
      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        categoryIds.Add(categoryId);
      }
      rdr.Dispose();
      List<Category> categories = new List<Category> {};
      foreach (int categoryId in categoryIds)
      {

      }
    }
    public void AddCategory(Category newCategory)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, ItemId);";
      MySqlParameter category_id = new MySqlParameter();
      category_id.ParameterName = "@CategoryId";
      category_id.Value = newCategory.GetId();
      cmd.Parameters.Add(category_id);
      MySqlParameter item_id = new MySqlParameter();
      item_id.ParameterName = "@ItemId";
      item_id.Value = _id;
      cmd.Parameters.Add(item_id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    }
  }
