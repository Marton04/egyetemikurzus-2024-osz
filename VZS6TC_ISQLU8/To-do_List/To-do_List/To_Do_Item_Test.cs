using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using To_do_List; 

namespace TodoList.Tests
{
    [TestFixture] 
    public class TodoItemTests
    {
        [Test] 
        public void TodoItem_Creation_ShouldSetPropertiesCorrectly()
        {
            
            var description = "Buy groceries";
            var deadline = new DateTime(2024, 12, 31);
            var priority = 3;
            var category = "Shopping";

           
            var todoItem = new TodoItem(description, deadline, priority, category);

            
            Assert.AreEqual(description, todoItem.Description);
            Assert.AreEqual(deadline, todoItem.Deadline);
            Assert.AreEqual(priority, todoItem.Priority);
            Assert.AreEqual(category, todoItem.Category);
        }
        [Test]
        public void TodoItem_InvalidPriority_ShouldThrowArgumentException()
        {
            
            var description = "Test";
            var deadline = DateTime.Now;
            var invalidPriority = -1;
            var category = "General";

            
            Assert.Throws<ArgumentException>(() =>
            {
                var todoItem = new TodoItem(description, deadline, invalidPriority, category);
            });
        }

        [Test]
        public void TodoItem_EmptyDescription_ShouldThrowArgumentException()
        {
            
            var deadline = DateTime.Now;
            var priority = 3;
            var category = "Work";

            
            Assert.Throws<ArgumentException>(() =>
            {
                var todoItem = new TodoItem(string.Empty, deadline, priority, category);
            });
        }


        [Test]
        public void TodoItem_ValidPriorities_ShouldNotThrowException()
        {
            
            var description = "Test";
            var deadline = DateTime.Now;
            var category = "Work";

            for (int priority = 1; priority <= 5; priority++)
            {
                
                Assert.DoesNotThrow(() =>
                {
                    var todoItem = new TodoItem(description, deadline, priority, category);
                });
            }
        }
        [Test]
        public void TodoItem_EmptyCategory_ShouldThrowArgumentException()
        {

            var description = "Test description";
            var deadline = DateTime.Now;
            var priority = 3;

            Assert.Throws<ArgumentException>(() =>
            {
                var todoItem = new TodoItem(description, deadline, priority, string.Empty);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                var todoItem = new TodoItem(description, deadline, priority, "   ");
            });
        }
    }
}