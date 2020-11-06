using CRUDApp.Controllers;
using CRUDApp.Db;
using CRUDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace CRUDApp.Test
{
    public class ContactsTest
    {
        ContactController _contactController;
        ContactsContext _dbContext;

        public ContactsTest()
        {
            var options = new DbContextOptionsBuilder<ContactsContext>();
            options.UseInMemoryDatabase("ContactsTestDb");
            _dbContext = new ContactsContext(options.Options);
            _contactController = new ContactController(_dbContext);

            _dbContext.Contacts.AddRange(
                new Contact() { FirstName = "John", MiddleName = "Lopez", LastName = "Santos", Organization = "FirstGen", Title = "Employee", EmailAddress = "jl.santos@gmail.com", PhoneNumber = "8472931" },
                new Contact() { FirstName = "Kevin", MiddleName = "Carlos", LastName = "Pineda", Organization = "SecondGen", Title = "Employee", EmailAddress = "kc.pineda@gmail.com", PhoneNumber = "7839402" },
                new Contact() { FirstName = "Jose", MiddleName = "Velasco", LastName = "De Castro", Organization = "ThirdGen", Title = "Employee", EmailAddress = "jv.decastro@gmail.com", PhoneNumber = "8473623" },
                new Contact() { FirstName = "Shiela", MiddleName = "Aquino", LastName = "Lim", Organization = "FirstGen", Title = "Employee", EmailAddress = "sa.lim@gmail.com", PhoneNumber = "7483920" },
                new Contact() { FirstName = "Alicia", MiddleName = "Santos", LastName = "Delos Santos", Organization = "SecondGen", Title = "Employee", EmailAddress = "as.delossantos@gmail.com", PhoneNumber = "6472839" },
                new Contact() { FirstName = "Franco", MiddleName = "Cruz", LastName = "Valdez", Organization = "ThirdGen", Title = "Employee", EmailAddress = "fc.valdez@gmail.com", PhoneNumber = "7482837" }
            );

            _dbContext.SaveChanges();
        }

        [Fact]
        public async void GetAllContactsTest()
        {
            //Act  
            var data = await _contactController.GetAllContacts();

            //Assert  
            Assert.NotNull(data.Value);
            Assert.Equal(6, data.Value.Count());
        }

        [Fact]
        public async void GetContactTest()
        {
            //Act
            var data = await _contactController.GetContact(3);

            //Assert
            Assert.NotNull(data.Value);
            Assert.Equal("Jose", data.Value.FirstName);
            Assert.Equal("Velasco", data.Value.MiddleName);
            Assert.Equal("De Castro", data.Value.LastName);
            Assert.Equal("ThirdGen", data.Value.Organization);
            Assert.Equal("Employee", data.Value.Title);
            Assert.Equal("jv.decastro@gmail.com", data.Value.EmailAddress);
            Assert.Equal("8473623", data.Value.PhoneNumber);
        }

        [Fact]
        public async void AddContactTest()
        {
            //Act
            var newContact = new Contact() 
            { 
                FirstName = "Franco", 
                MiddleName = "Cruz", 
                LastName = "Valdez", 
                Organization = "ThirdGen", 
                Title = "Employee", 
                EmailAddress = "fc.valdez@gmail.com", 
                PhoneNumber = "7482837" 
            };

            var data = await _contactController.AddContact(newContact);

            //Assert
            Assert.NotNull(data.Result);
            var result = data.Result as CreatedAtActionResult;
            var createdValue = result.Value as Contact;
            Assert.Equal("Franco", createdValue.FirstName);
            Assert.Equal("Cruz", createdValue.MiddleName);
            Assert.Equal("Valdez", createdValue.LastName);
            Assert.Equal("ThirdGen", createdValue.Organization);
            Assert.Equal("Employee", createdValue.Title);
            Assert.Equal("fc.valdez@gmail.com", createdValue.EmailAddress);
            Assert.Equal("7482837", createdValue.PhoneNumber);
        }

        [Fact]
        public async void UpdateContactTest()
        {
            var contactToUpdate = _contactController.GetContact(5).Result.Value as Contact;
            contactToUpdate.MiddleName = "Delos Santos";
            contactToUpdate.LastName = "Valderama";
            contactToUpdate.EmailAddress = "ads.valderama@gmail.com";

            //Act
            var data = await _contactController.UpdateContact(5, contactToUpdate);

            //Assert
            Assert.NotNull(data);
            var updatedContactValue = _contactController.GetContact(5).Result.Value as Contact;
            Assert.Equal(5, updatedContactValue.ContactId);
            Assert.Equal("Delos Santos", updatedContactValue.MiddleName);
            Assert.Equal("Valderama", updatedContactValue.LastName);
            Assert.Equal("ads.valderama@gmail.com", updatedContactValue.EmailAddress);
        }

        [Fact]
        public async void DeleteContactTest()
        {
            //Act
            var data = await _contactController.DeleteContact(2);

            //Assert
            Assert.NotNull(data);
            var checkDeletedData = _contactController.GetContact(2);
            Assert.IsType<NotFoundResult>(checkDeletedData.Result.Result);

        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }


    }
}
