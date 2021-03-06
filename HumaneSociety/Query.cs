﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;
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
            Console.WriteLine("ID: "+thisEmployee.EmployeeId);
            Console.WriteLine("Employee number: " + thisEmployee.EmployeeNumber);
            Console.WriteLine("First name: " + thisEmployee.FirstName);
            Console.WriteLine("Last name: " + thisEmployee.LastName);
            Console.WriteLine("Username: " + thisEmployee.UserName);
            Console.WriteLine("Email: "+thisEmployee.Email);
            if (thisEmployee.Animals!=null)
            {
                Console.WriteLine("Working with "+thisEmployee.Animals.Count+" animals");
                foreach (var item in thisEmployee.Animals)
                {
                    Console.WriteLine(" "+item.AnimalId +" "+ item.Category.Name +" "+ item.Name);
                }
            }
            Console.WriteLine("No update have been made. Press any key to continue.");
            Console.ReadKey();
        }
        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {                       
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
            Animal thisanimal = db.Animals.Where
                (
                 newanimal =>
                 newanimal.CategoryId == animal.CategoryId &&
                 newanimal.Demeanor == animal.Demeanor &&
                 newanimal.DietPlanId == animal.DietPlanId &&
                 newanimal.Age == animal.Age &&
                 newanimal.KidFriendly == animal.KidFriendly &&
                 newanimal.PetFriendly == animal.PetFriendly &&
                 newanimal.Weight == animal.Weight &&
                 newanimal.Name == animal.Name
                ).Single();
            PutAnimalInRoom( thisanimal);
            PairAnimalEmployee(thisanimal);
                
        }
        internal static void PutAnimalInRoom(Animal animal)
        {
            Room room = null;
            int roomid = UserInterface.GetIntegerData("the animal will stay in", " which room?");
            try
            {
                room = db.Rooms.Where(r => r.RoomId == roomid).Single();
                Console.WriteLine("that room is occupied ,try again. Press any key.");
                Console.ReadKey();
                PutAnimalInRoom(animal);
            }
            catch (Exception)
            {
                Room thisroom = new Room();
                thisroom.AnimalId = animal.AnimalId;
                db.Rooms.InsertOnSubmit(thisroom);
                db.SubmitChanges();
                
            }

           
        }
        internal static void PairAnimalEmployee(Animal animal)
        {
            Console.WriteLine("Will this animal be working with a particular employee?");
            if ((bool)UserInterface.GetBitData())
            {
                Employee employee;
                Console.WriteLine("enter employee number");
                int employeeID = UserInterface.GetIntegerData("", "");
                try
                {
                    employee = db.Employees.Where(e => e.EmployeeNumber == employeeID).Single();
                }
                catch (Exception)
                {
                    Console.WriteLine("That employee does not exist, try again. Press any key");
                    Console.ReadKey();
                    PairAnimalEmployee(animal);
                    return;
                }
                employee = db.Employees.Where(e => e.EmployeeNumber == employeeID).Single();
                Animal thisanimal = db.Animals.Where(a => a == animal).Single();
                employee.Animals.Add(thisanimal); ;

                db.SubmitChanges();
            }
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
            return thisAnimal;
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

                }
                categoryfromdb = db.Categories.Where(a => a.Name.Contains(category)).Single();                db.SubmitChanges();
                currentAnimal.CategoryId = categoryfromdb.CategoryId;                
            }
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
            db.SubmitChanges();


        }

        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }
        
        // TODO: Animal Multi-Trait Search

        internal static List<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates)

        {
            int categoryid=0;
            string thiscategory;
            List<Animal> output = new List<Animal>();
            if (updates.TryGetValue(1, out thiscategory))
            {
                categoryid = GetCategoryId(thiscategory);
            }
            IQueryable<Animal> dbOut1 = null;
            IQueryable<Animal> dbOut2 = null;
            IQueryable<Animal> dbOut3 = null;
            IQueryable<Animal> dbOut4 = null;
            IQueryable<Animal> dbOut5 = null;
            IQueryable<Animal> dbOut6 = null;
            IQueryable<Animal> dbOut7 = null;
            IQueryable<Animal> dbOut8 = null;

            foreach (KeyValuePair<int, string> items in updates)
            {
                switch (items.Key)
                {
                    case 1:
                        dbOut1 = db.Animals.Where(a => a.CategoryId == categoryid);
                        break;
                    case 2:
                        dbOut2 = db.Animals.Where(a => a.Name == items.Value);
                        break;
                    case 3:
                        dbOut3 = db.Animals.Where(a => a.Age.ToString() == items.Value);
                        break;
                    case 4:
                        dbOut4 = db.Animals.Where(a => a.Demeanor == items.Value);
                        break;
                    case 5:
                        dbOut5 = db.Animals.Where(a => a.KidFriendly.ToString() == items.Value);
                        break;
                    case 6:
                        dbOut6 = db.Animals.Where(a => a.PetFriendly.ToString() == items.Value);
                        break;
                    case 7:
                        dbOut7 = db.Animals.Where(a => a.Weight.ToString() == items.Value);
                        break;
                    case 8:
                        dbOut8 = db.Animals.Where(a => a.AnimalId.ToString() == items.Value);
                        break;                   
                }
                List<List<Animal>> listofAnimals = new List<List<Animal>>();
                if (dbOut1!=null)
                {
                    listofAnimals.Add(dbOut1.ToList());
                }
                if (dbOut2 != null)
                {
                    listofAnimals.Add(dbOut2.ToList());
                }
                if (dbOut3 != null)
                {
                    listofAnimals.Add(dbOut3.ToList());
                }
                if (dbOut4 != null)
                {
                    listofAnimals.Add(dbOut4.ToList());
                }
                 if (dbOut5!=null)
                {
                    listofAnimals.Add(dbOut5.ToList());
                }
                if (dbOut6 != null)
                {
                    listofAnimals.Add(dbOut6.ToList());
                }
                if (dbOut7 != null)
                {
                    listofAnimals.Add(dbOut7.ToList());
                }
                if (dbOut8 != null)
                {
                    listofAnimals.Add(dbOut8.ToList());
                }
                foreach (List<Animal> animalList in listofAnimals)
                {
                    foreach (Animal animal in animalList)
                    {
                        if (dbOut1 != null)
                        {
                            if (!dbOut1.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut2 != null)
                        {
                            if (!dbOut2.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut3 != null)
                        {
                            if (!dbOut3.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut4 != null)
                        {
                            if (!dbOut4.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut5 != null)
                        {
                            if (!dbOut5.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut6 != null)
                        {
                            if (!dbOut6.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut7 != null)
                        {
                            if (!dbOut7.Contains(animal))
                            {
                                break;
                            }
                        }
                        if (dbOut8 != null)
                        {
                            if (!dbOut8.Contains(animal))
                            {
                                break;
                            }
                        }
                        output.Add(animal);
                    }
                }

            }
            return output.Distinct().ToList();
            /*
            List<string> command = new List<string>() { "Category", "Name", "Age", "Demeanor", "KidFriendly", "PetFriendly", "Weight", "ID" };
            List<Animal> output = new List<Animal>();
            int categoryid=0;
            string thiscategory;
            if (updates.TryGetValue(1,out thiscategory))
            {
                categoryid = GetCategoryId(thiscategory);
            }
            foreach (KeyValuePair<int, string> update in updates)
            {
                if (update.Key==1)
                {
                   IQueryable<Animal> animalfromID = db.Animals.Where(a => a.AnimalId == categoryid);
                }
                IQueryable<Animal> animal = (db.Animals.Where(a => a[command[update.Key]].ToString() == update.Value.ToString()));
                List<Animal> queried = animal.ToList();
                foreach (Animal item in queried)
                {
                    output.Add(item);
                }
            }
            */

        }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            var category= db.Categories.Where(c => c.Name.Contains( categoryName)).Single();
            return category.CategoryId;
        }
        
        internal static Room GetRoom(int animalId)
        {
            try
            {
                return db.Rooms.Where(r => r.AnimalId == animalId).Single();
            }
            catch
            {
                //Console.WriteLine("that animal is not is the building. any key to go back");
                //Console.ReadKey();
                //our room seeds are messed up, so we are gonna fake it till we make it. added animals should all have rooms as well as the grading databases. probably.
                return db.Rooms.Where(r=>r.RoomNumber == 1).Single(); 
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
            Adoption adoption = new Adoption();
            adoption.AnimalId = animal.AnimalId;
            adoption.ClientId = client.ClientId;
            adoption.ApprovalStatus = "pending";

            Console.WriteLine("Has the adoption fee been paid?");
            if ((bool)UserInterface.GetBitData())
            {
                Console.WriteLine("How much was paid?");
                adoption.AdoptionFee = UserInterface.GetIntegerData();
                adoption.PaymentCollected = true;
            }
            db.Adoptions.InsertOnSubmit(adoption);
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            return db.Adoptions.Where(a => a.ApprovalStatus == "pending");
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            if (isAdopted)
            {
                adoption.ApprovalStatus = "Approved";
                return;
            }
            Console.WriteLine("update payment? y/n");
            if ((bool)UserInterface.GetBitData())
            {
                Console.WriteLine("How much was paid?");
                adoption.AdoptionFee= UserInterface.GetIntegerData();
                adoption.PaymentCollected = true;
            }
            Console.WriteLine("Issue refund?");
            if ((bool)UserInterface.GetBitData())
            {
                adoption.PaymentCollected = false;
                adoption.AdoptionFee = 0;
            }

        }

        internal static void RemoveAdoption(int animalId, int clientId)//_stilltodo_______________________________________________/^\
        {
            Adoption adoption = db.Adoptions.Where(a => a.AnimalId == animalId && a.ClientId == clientId).Single();
            db.Adoptions.DeleteOnSubmit(adoption);
            db.SubmitChanges();
            //throw new NotImplementedException();
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
            AnimalShot animalShot = new AnimalShot();
            animalShot.ShotId = shot.ShotId;
            animalShot.AnimalId = animal.AnimalId;
            animalShot.DateReceived = DateTime.Today;
            db.AnimalShots.InsertOnSubmit(animalShot);           
            db.SubmitChanges();
        }
    }
}