/*INSERT INTO Categories VALUES('Dogs');
INSERT INTO Categories VALUES('Cats');
INSERT INTO Categories VALUES('Fish');
INSERT INTO Categories VALUES('Birds');
INSERT INTO Categories VALUES('Reptiles');*/

/*INSERT INTO Employees VALUES(FirstName VARCHAR(50), 
LastName VARCHAR(50), UserName VARCHAR(50), 
Password VARCHAR(50), EmployeeNumber INTEGER, Email VARCHAR(50));*/
INSERT INTO Employees VALUES('Tony','Stark','Ironman','iAmIronMan',1,'ironman@stark.com');
INSERT INTO Employees VALUES('Steve','Rogers','Captain America','iCanDoThisAllDay',2,'cap@stark.com');
INSERT INTO Employees VALUES('Thor','Odinson','God of Thunder','imStillWorthy',3,'thor@stark.com');
INSERT INTO Employees VALUES('Bruce','Banner','Hulk','imAlwaysAngry',4,'hulk@stark.com');
INSERT INTO Employees VALUES('Peter','Parker','Spider-Man','withGreatPower',5,'spiderman@stark.com');

INSERT INTO DietPlans VALUES('SAD','Cheeseburger Diet',5);
INSERT INTO DietPlans VALUES('Vegetariean','Produce & Dairy',8);
INSERT INTO DietPlans VALUES('Vegan','Vegetables, Fruits, Nuts & Seeds',10);
INSERT INTO DietPlans VALUES('Carnivore','Raw Meat & Organs',7);
INSERT INTO DietPlans VALUES('Fish Food','Flakes & Pellets',3);

/*Name VARCHAR(50), Weight INTEGER, Age INTEGER, Demeanor VARCHAR(50), 
KidFriendly BIT, PetFriendly BIT, Gender VARCHAR(50), AdoptionStatus VARCHAR(50), 
CategoryId INTEGER FOREIGN KEY REFERENCES Categories(CategoryId), 
INTEGER FOREIGN KEY REFERENCES DietPlans(DietPlanId), 
EmployeeId INTEGER FOREIGN KEY REFERENCES Employees(EmployeeId));*/

INSERT INTO Animals VALUES('Duke',50, 8,'Lazy',1,1,'Male','Adopted',1,4,1);
INSERT INTO Animals VALUES('Penelope',10,5,'Feline',1,1,Female,'Adopted',2,4,2);
INSERT INTO Animals VALUES('Star',1,1,'Dull',1,0,'Female','Waiting',3,5,3);
INSERT INTO Animals VALUES('Cloud',1,3,'Hateful',0,0,'Male','Waiting',4,3,4);
INSERT INTO Animals VALUES('Spike',2,4,'Chill',1,0,'Male','Adopted',5,1,5);

/*RoomNumber INTEGER, AnimalId INTEGER FOREIGN KEY REFERENCES Animals(AnimalId)*/
INSERT INTO Rooms VALUES(1);
INSERT INTO Rooms VALUES(2);
INSERT INTO Rooms VALUES(3);
INSERT INTO Rooms VALUES(4);
INSERT INTO Rooms VALUES(5);
INSERT INTO Rooms VALUES(6);
INSERT INTO Rooms VALUES(7);
INSERT INTO Rooms VALUES(8);
INSERT INTO Rooms VALUES(9);
INSERT INTO Rooms VALUES(10);

FirstName VARCHAR(50), LastName VARCHAR(50), UserName VARCHAR(50), Password VARCHAR(50), AddressId INTEGER FOREIGN KEY REFERENCES Addresses(AddressId), Email VARCHAR(50));
INSERT INTO Clients VALUES('Michael','Terril',

