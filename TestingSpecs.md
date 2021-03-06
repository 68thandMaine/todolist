# Testing specs for child objects
<sup>_Chris Rudnicky_</sup>

## Table of Contents:
- [Get Parent Objects]("#GetCategories") <sub>GetCategories()</sub>
- [Add Parent Object]("#AddCategories") <sub>AddCategory()</sub>

---
## `GetCategories()`
###### You will need to write the `AddCategory()` method and test method simultaneously for this method to test correctly.
###### In this example the category is the parent object. Naming conventions will differ based on the project.  
- Written as:
>`public List<category> GetCategories() { }`  

This method is used to retrieve a list of **parent objects**  we can insert this test into the child object class to associate child objects with parent objects, or we could use it to display a information about parent objects if it is used in the parent object class.

Inside the **test file** write the following method. Be sure change any mention of a category to whatever the parent object in your current project is.

      [TestMethod]
      public void GetCategories_ReturnsAllItemCategories_CategoryList()
      {
        Item testItem = new Item("Mow the lawn");
        testItem.Save();
        Category testCategory1 = new Category("Home stuff");
        testCategory1.Save();
        Category testCategory2 = new Category("Work stuff");
        testCategory2.Save();
        testItem.AddCategory(testCategory1);
        List<Category> result = testItem.GetCategories();
        List<Category> testList = new List<Category> {testCategory1};
        CollectionAssert.AreEqual(testList, result);
      }


To fail this test/avoid compilation errors we will write this code inside the **class file**:

    public List<Category> GetCategories()
    {
      List<Category> categories = new List<Category>{};
      return categories;
    }

<sub>[**_Return to TOC_**]("#table-of-contents")</sub>

---
## `AddCategory()`
###### You will need to write the `GetCategories()` method and test method simultaneously for this method to test correctly.
###### In this example the category is the parent object. Naming conventions will differ based on the project.
This method must be written for the `GetCategories()` test method to pass. This is because in order to test the ability to add join table entries, we must also be able to return them, but we cannot return a join table if there are no join tables either.

Inside the **test file** write the following method. Be sure change any mention of a category to whatever the parent object in your current project is.

    [TestMethod]
    public void AddCategory_AddsCategoryToItem_CategoryList()
    {
      Item testItem = new Item("Mow the lawn");
      testItem.Save();
      Category testCategory = new Category("Home stuff");
      testCategory.Save();
      testItem.AddCategory(testCategory);
      List<Category> result = testItem.GetCategories();
      List<Category> testList = new List<Category>{testCategory};
      CollectionAssert.AreEqual(testList, result);
    }

To fail this test, write the following code inside the **class file**:

    public void AddCategory(Category newCategory)
    {

    }

To pass this test, replace the above code with:

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


<sub>[**_Return to TOC_**]("#table-of-contents")</sub>

---
