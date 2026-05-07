This was my second software development project using C#, DOTNET framewok, mySQL, and Winforms. This system has a complete user friendly environment.
The system was developed for completely internal information storing and maintaining. The customer can choose their designated cars and select it for rent for their requirement days. Then the system automatically calculates the fare and all of the data is stored in the database. For database, I used the mySQL and Xaamp(PHPmyAdmin) to store the data. The system allows users(rental service employees) authentication[login and registation] to this system. For password storing, I used SHA-256 hashing algorithm. After finishing every successful rental, user can print the invoice and save it for further uses.


The database Schema is-->
`CREATE DATABASE carrental;`

The tables are 
- usertb1,
- cartb1,
- customertb,
- rentaltb,
- returntb. 

The commands for tables:
## Table usertb1
```CREATE TABLE usertb1 (
    Id INT PRIMARY KEY,
    Uname VARCHAR(50) NOT NULL UNIQUE,
    Upass VARCHAR(64) NOT NULL
);
```
## Table cartb1
```CREATE TABLE cartb1 (
    Regno VARCHAR(20) PRIMARY KEY,
    Brand VARCHAR(50) NOT NULL,
    Model VARCHAR(50) NOT NULL,
    Available CHAR(3) NOT NULL CHECK (Available IN ('YES', 'NO')),
    Price DECIMAL(10,2) NOT NULL
);
```
## Table customertb
```CREATE TABLE customertb (
    custid INT PRIMARY KEY,
    custname VARCHAR(100) NOT NULL,
    custaddress VARCHAR(200),
    phone VARCHAR(15) NOT NULL,
    nid VARCHAR(20)
);
```
## Table rentaltb
```CREATE TABLE rentaltb (
    rentid INT PRIMARY KEY,
    carreg VARCHAR(20) NOT NULL,
    custname VARCHAR(100) NOT NULL,
    rentdate DATE NOT NULL,
    returnDate DATE NOT NULL,
    rentfee DECIMAL(10,2) NOT NULL,
    RentDay INT NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (carreg) REFERENCES cartb1(Regno) ON DELETE RESTRICT ON UPDATE CASCADE
);
```
## Table returntb
```CREATE TABLE returntb (
    rentid INT PRIMARY KEY,
    carreg VARCHAR(20) NOT NULL,
    custname VARCHAR(100) NOT NULL,
    returndate DATE NOT NULL,
    delay VARCHAR(50),
    fine DECIMAL(10,2),
    FOREIGN KEY (rentid) REFERENCES rentaltb(rentid) ON DELETE CASCADE,
    FOREIGN KEY (carreg) REFERENCES cartb1(Regno)
);
```
