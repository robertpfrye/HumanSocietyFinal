﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName != null;
        }


        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)//tony
        {
            Employee workingEmployee;
            switch (crudOperation)
            {
                case "update":
                    UpdateEmployee(employee);
                    break;
                case "read":
                    ReadEmployee(employee);
                    break;
                case "delete":
                    try
                    {
                        workingEmployee = db.Employees.Where(c => c.EmployeeId == employee.EmployeeId).Single();
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine("No employees have an id number that matches the employee requested.");
                        Console.WriteLine("No update have been made.");
                        return;
                    }

                    //todo figure out how to delete employee
                    break;
                case "create":
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();

                    break;                    
            }


        }
        private static void UpdateEmployee(Employee employee)//tony
        {
            Employee workingEmployee = null; 

            try
            {
                workingEmployee = db.Employees.Where(c => c.EmployeeNumber == employee.EmployeeNumber).Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No employees have an id number that matches the employee requested.");
                Console.WriteLine("No update have been made.");
                return;
            }
            workingEmployee.FirstName = employee.FirstName;
            workingEmployee.LastName = employee.LastName;
            workingEmployee.EmployeeNumber = employee.EmployeeNumber;
            workingEmployee.Email = employee.Email;
            db.SubmitChanges();
        }
        private static void ReadEmployee(Employee employee)//tony
        {
            Employee testEmployee = null;
            try
            {
                testEmployee = db.Employees.Where(c => c.EmployeeNumber == employee.EmployeeNumber).Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No employees have an id number that matches the employee requested.");
                Console.WriteLine("No update have been made. Enter to continue.");
                Console.ReadLine();
                return;
            }
            Employee thisEmployee = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).Single();
            Console.WriteLine(thisEmployee.EmployeeId);
            Console.WriteLine(thisEmployee.EmployeeNumber);
            Console.WriteLine(thisEmployee.FirstName);
            Console.WriteLine(thisEmployee.LastName);
            Console.WriteLine(thisEmployee.UserName);
            Console.WriteLine(thisEmployee.Email);
            if (thisEmployee.Animals!=null)
            {
                Console.WriteLine(thisEmployee.Animals);

            }
            Console.WriteLine("No update have been made. Press any key to continue.");
            Console.ReadKey();
        }
        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)//Rob
        {
            Animal thisAnimal = null;
            try
            {
                thisAnimal = db.Animals.Where(a => a.AnimalId == id).Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No animals have an id number that matches the animal requested.");
                Console.WriteLine("No updates have been made. Enter to continue.");
                Console.ReadLine();
            }

            //Console.WriteLine(thisAnimal.AnimalId);
            //Console.WriteLine(thisAnimal.Name);
            //Console.WriteLine(thisAnimal.Weight);
            //Console.WriteLine(thisAnimal.Age);
            //Console.WriteLine(thisAnimal.Demeanor);
            //Console.WriteLine(thisAnimal.KidFriendly);
            //Console.WriteLine(thisAnimal.PetFriendly);
            //Console.WriteLine(thisAnimal.Gender);
            //Console.WriteLine(thisAnimal.AdoptionStatus);
            //Console.WriteLine(thisAnimal.CategoryId);
            //Console.WriteLine(thisAnimal.Category);
            //Console.WriteLine(thisAnimal.DietPlanId);
            //Console.WriteLine(thisAnimal.DietPlan);
            //Console.WriteLine(thisAnimal.EmployeeId);
            //Console.WriteLine(thisAnimal.Employee);

            //Console.WriteLine("No updates have been made. Press any key to continue.");
            //Console.ReadKey();

            return thisAnimal;
            //throw new NotImplementedException();
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)//Rob
        {
            Animal currentAnimal = null;
            Category categoryfromdb = null;
            bool doOperation = false;          
            try
            {
                currentAnimal = db.Animals.Where(a => a.AnimalId == animalId).Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No animals have an id number that matches the animal requested.");
                Console.WriteLine("No updates have been made.");
                return;
            }
            string category;
            doOperation= updates.TryGetValue(1, out category);
            if(doOperation)
            {
                try
                {
                    categoryfromdb = db.Categories.Where(a => a.Name.Contains(category)).Single();
                }
                catch
                {
                    categoryfromdb = new Category();
                    categoryfromdb.Name = category;
                    db.Categories.InsertOnSubmit(categoryfromdb);
                    db.SubmitChanges();
                    categoryfromdb = db.Categories.Where(a => a.Name.Contains(category)).Single();
                }
            }
            currentAnimal.CategoryId = categoryfromdb.CategoryId;
            //dictionary key/value guide
            //"Select Update:", "1. Category", "2. Name", "3. Age", "4. Demeanor", 
            //"5. Kid friendly", "6. Pet friendly", "7. Weight", "8. Finished"
            string name;
            if (updates.TryGetValue(2, out name))
            {
                currentAnimal.Name = name;
            }

            string age;
            if (updates.TryGetValue(3, out age))
            {
                currentAnimal.Age = int.Parse(age);
            }

            string demeanor;
            if (updates.TryGetValue(4, out demeanor))
            {
                currentAnimal.Demeanor = demeanor;
            }

            string kidFriendly;
            if (updates.TryGetValue(5, out kidFriendly))
            {
                currentAnimal.KidFriendly = bool.Parse(kidFriendly);
            }

            string petFriendly;
            if(updates.TryGetValue(6, out petFriendly))
            {
                currentAnimal.PetFriendly = bool.Parse(petFriendly);
            }

            string weight;
            if ( updates.TryGetValue(7, out weight))
            {
                currentAnimal.Weight = int.Parse(weight);
            }


            
            //currentAnimal.Age = animal.Age;
            //currentAnimal.Demeanor = animal.Demeanor;
            //currentAnimal.KidFriendly = animal.KidFriendly;
            //currentAnimal.PetFriendly = animal.PetFriendly;
            //currentAnimal.Gender = animal.Gender;
            //currentAnimal.AdoptionStatus = animal.AdoptionStatus;
            //currentAnimal.DietPlanId = animal.DietPlanId;
            //currentAnimal.DietPlan = animal.DietPlan;
            //currentAnimal.EmployeeId = animal.EmployeeId;
            //currentAnimal.Employee = animal.Employee;
            db.SubmitChanges();

            //throw new NotImplementedException();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            throw new NotImplementedException();
        }
         
        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            var category= db.Categories.Where(c => c.Name == categoryName).Single();
            return category.CategoryId;
        }
        
        internal static Room GetRoom(int animalId)//Check if animal null
        {
            try
            {
                return db.Rooms.Where(r => r.AnimalId == animalId).Single();
            }
            catch
            {
                Console.WriteLine("that animal is not is the building. any key to go back");
                Console.ReadKey();
                return null; 
            }
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            var diet = db.DietPlans.Where(d => d.Name.Contains(dietPlanName)).Single();
            return diet.DietPlanId;
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            throw new NotImplementedException();
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            throw new NotImplementedException();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            return db.AnimalShots.Where(a => a.AnimalId == animal.AnimalId);
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            Shot shot = null;
            try
            {
                shot = db.Shots.Where(s => s.Name == shotName).Single();
            }
            catch (InvalidOperationException e)
            {
                Shot newshot = new Shot();
                newshot.Name = shotName;
                db.Shots.InsertOnSubmit(newshot);
                db.SubmitChanges();
                shot = db.Shots.Where(s => s.Name == shotName).Single();
            }

            animal.AnimalShots = shot.AnimalShots;
            
            db.SubmitChanges();

        }
    }
}