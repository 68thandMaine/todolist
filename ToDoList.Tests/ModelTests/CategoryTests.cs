using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTest
  : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=ToDoList_Tests;";
      }

    public void Dispose()
    {
      Category.ClearAll();
      Item.ClearAll();
    }

    [TestMethod]
    public void CategoryConstructor_CreastesInstanceOfCategory_Category()
    {
      Category newCategory = new Category("test category");
      Assert.AreEqual(typeof(Category), newCategory.GetType());
    }
    [TestMethod]
    public void GetName_ReturnsName_String()
    {
      //Arrange
      string name = "Test Category";
      Category newCategory = new Category(name);

      //Act
      string result = newCategory.GetName();

      //Assert
      Assert.AreEqual(name, result);

    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Category()
    {
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");
      Assert.AreEqual(firstCategory, secondCategory);
    }

    [TestMethod]
    public void Save_SavesCategoryToDatabase_CategoryList()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category> {testCategory};
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
     public void GetAll_ReturnsAllCategoryObjects_CategoryList()
     {
       //Arrange
       string name01 = "Work";
       string name02 = "School";
       Category newCategory1 = new Category(name01);
       newCategory1.Save();
       Category newCategory2 = new Category(name02);
       newCategory2.Save();
       List<Category> newList = new List<Category> { newCategory1, newCategory2 };

       //Act
       List<Category> result = Category.GetAll();

       //Assert
       CollectionAssert.AreEqual(newList, result);
     }
     [TestMethod]
      public void Find_ReturnsCorrectCategory_Category()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        //Act
        Category foundCategory = Category.Find(testCategory.GetId());

        //Assert
        Assert.AreEqual(testCategory, foundCategory);
      }
      // [TestMethod]
      // public void GetItems_ReturnsEmptyItemList_ItemList()
      // {
      //   //Arrange
      //   string name = "Work";
      //   Category newCategory = new Category(name);
      //   List<Item> newList = new List<Item> {};
      //
      //   //Act
      //   List<Item> result = newCategory.GetItems();
      //
      //   //Assert
      //   CollectionAssert.AreEqual(newList, result);
      // }
      // [TestMethod]
      // public void GetItems_RetrievesAllItemsWithCategory_ItemList()
      // {
      //   Category testCategory = new Category("Household chores");
      //   testCategory.Save();
      //   Item firstItem = new Item("Mow the lawn", testCategory.GetId());
      //   firstItem.Save();
      //   Item secondItem = new Item("Do the dishes", testCategory.GetId());
      //   secondItem.Save();
      //   List<Item> testItemList = new List<Item> {firstItem, secondItem};
      //   List<Item> resultItemList = testCategory.GetItems();
      //   CollectionAssert.AreEqual(testItemList, resultItemList);
      // }
      [TestMethod]
      public void GetAll_CategoriesEmptyAtFirst_List()
      {
        int result = Category.GetAll().Count;
        Assert.AreEqual(0, result);
      }

      [TestMethod]
      public void Save_DatabaseAssignsIdToCategory_Id()
      {
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        Category savedCategory = Category.GetAll()[0];

        int result = savedCategory.GetId();
        int testId = testCategory.GetId();
        Console.WriteLine("---------" + testId);
        Assert.AreEqual(testId, result);
      }

  }
}
