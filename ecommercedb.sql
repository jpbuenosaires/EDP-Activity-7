-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 15, 2026 at 05:57 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ecommercedb`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CreateNewOrder` (IN `p_CustomerID` INT, IN `p_OrderDate` DATE)   BEGIN
    INSERT INTO Orders (CustomerID, OrderDate, Status)
    VALUES (p_CustomerID, p_OrderDate, 'Pending');
END$$

--
-- Functions
--
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_CalculateOrderTotal` (`f_OrderID` INT) RETURNS DECIMAL(10,2) DETERMINISTIC BEGIN
    DECLARE total_amount DECIMAL(10,2);
    
    SELECT SUM(Quantity * UnitPrice) INTO total_amount
    FROM OrderDetails
    WHERE OrderID = f_OrderID;
    
    -- Return 0 if the order has no details yet
    RETURN IFNULL(total_amount, 0.00);
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `categories`
--

CREATE TABLE `categories` (
  `CategoryID` int(11) NOT NULL,
  `CategoryName` varchar(50) NOT NULL,
  `Description` varchar(150) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `categories`
--

INSERT INTO `categories` (`CategoryID`, `CategoryName`, `Description`) VALUES
(1, 'Electronics', 'Gadgets and devices'),
(2, 'Clothing', 'Apparel for men and women'),
(3, 'Home & Garden', 'Furniture and outdoor items'),
(4, 'Sports', 'Athletic gear and equipment'),
(5, 'Books', 'Physical and audio books'),
(6, 'Toys', 'Games and toys for children'),
(7, 'Health & Beauty', 'Cosmetics and personal care'),
(8, 'Automotive', 'Car parts and accessories'),
(9, 'Groceries', 'Food and beverages'),
(10, 'Office Supplies', 'Stationery and office equipment'),
(11, 'Pet Supplies', 'Food, toys, and accessories for pets'),
(12, 'Musical Instruments', 'Guitars, keyboards, and accessories'),
(13, 'Movies & TV', 'DVDs, Blu-rays, and digital media'),
(14, 'Hardware', 'Tools and home improvement'),
(15, 'Jewelry', 'Watches, rings, and necklaces');

-- --------------------------------------------------------

--
-- Table structure for table `customers`
--

CREATE TABLE `customers` (
  `CustomerID` int(11) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `RegistrationDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customers`
--

INSERT INTO `customers` (`CustomerID`, `FirstName`, `LastName`, `Email`, `RegistrationDate`) VALUES
(1, 'Alice', 'Walker', 'alice.w@email.com', '2023-01-10'),
(2, 'Bob', 'Harris', 'bharris@email.com', '2023-01-15'),
(3, 'Charlie', 'Clark', 'cclark@email.com', '2023-02-05'),
(4, 'Diana', 'Prince', 'dprince@email.com', '2023-02-14'),
(5, 'Evan', 'Wright', 'ewright@email.com', '2023-03-22'),
(6, 'Fiona', 'Gallagher', 'fgallagher@email.com', '2023-04-11'),
(7, 'George', 'Miller', 'gmiller@email.com', '2023-05-09'),
(8, 'Hannah', 'Abbott', 'habbott@email.com', '2023-06-30'),
(9, 'Ian', 'Somerhalder', 'ian.s@email.com', '2023-07-21'),
(10, 'Julia', 'Roberts', 'jroberts@email.com', '2023-08-15'),
(11, 'Kevin', 'Bacon', 'kbacon@email.com', '2023-09-01'),
(12, 'Laura', 'Dern', 'ldern@email.com', '2023-09-05'),
(13, 'Michael', 'Scott', 'mscott@email.com', '2023-09-10'),
(14, 'Nina', 'Simone', 'nsimone@email.com', '2023-09-15'),
(15, 'Oscar', 'Isaac', 'oisaac@email.com', '2023-09-20'),
(16, 'Paula', 'Abdul', 'pabdul@email.com', '2023-09-25'),
(17, 'Quincy', 'Jones', 'qjones@email.com', '2023-10-01'),
(18, 'Rachel', 'Green', 'rgreen@email.com', '2023-10-05'),
(19, 'Steve', 'Carell', 'scarell@email.com', '2023-10-10'),
(20, 'Tina', 'Fey', 'tfey@email.com', '2023-10-15');

-- --------------------------------------------------------

--
-- Table structure for table `orderdetails`
--

CREATE TABLE `orderdetails` (
  `OrderDetailID` int(11) NOT NULL,
  `OrderID` int(11) DEFAULT NULL,
  `ProductID` int(11) DEFAULT NULL,
  `Quantity` int(11) NOT NULL,
  `UnitPrice` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orderdetails`
--

INSERT INTO `orderdetails` (`OrderDetailID`, `OrderID`, `ProductID`, `Quantity`, `UnitPrice`) VALUES
(1, 1, 1, 5, 99.99),
(3, 2, 2, 3, 19.99),
(4, 3, 10, 1, 199.99),
(5, 4, 5, 2, 85.00),
(6, 5, 3, 1, 35.50),
(7, 6, 4, 2, 25.00),
(8, 7, 9, 4, 14.99),
(9, 8, 6, 1, 49.99),
(10, 9, 7, 3, 15.75),
(11, 10, 8, 2, 22.50),
(12, 11, 11, 2, 25.99),
(13, 11, 12, 1, 150.00),
(14, 12, 13, 1, 14.99),
(15, 13, 14, 1, 89.99),
(16, 14, 15, 1, 120.00),
(17, 15, 16, 1, 75.50),
(18, 15, 17, 3, 10.99),
(19, 16, 18, 2, 8.50),
(20, 17, 19, 1, 12.00),
(21, 18, 20, 6, 3.99),
(22, 19, 21, 2, 15.00),
(23, 20, 22, 1, 110.00),
(24, 21, 23, 4, 18.50),
(25, 22, 24, 1, 22.99),
(26, 23, 25, 5, 9.99),
(27, 24, 1, 1, 99.99),
(28, 24, 25, 2, 9.99),
(29, 25, 2, 2, 19.99),
(30, 25, 3, 1, 35.50),
(31, 1, 1, 5, 99.99);

--
-- Triggers `orderdetails`
--
DELIMITER $$
CREATE TRIGGER `trg_AfterDeleteOrderDetail` AFTER DELETE ON `orderdetails` FOR EACH ROW BEGIN
    -- PURPOSE: 
    -- This trigger handles order cancellations at the line-item level. 
    -- If an item is removed from an order, the quantity that was reserved 
    -- is automatically added back to the Products table's stock.
    UPDATE Products
    SET StockQuantity = StockQuantity + OLD.Quantity
    WHERE ProductID = OLD.ProductID;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_AfterInsertOrderDetail` AFTER INSERT ON `orderdetails` FOR EACH ROW BEGIN
    UPDATE Products
    SET StockQuantity = StockQuantity - NEW.Quantity
    WHERE ProductID = NEW.ProductID;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `OrderID` int(11) NOT NULL,
  `CustomerID` int(11) DEFAULT NULL,
  `OrderDate` date NOT NULL,
  `Status` varchar(20) DEFAULT 'Pending'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`OrderID`, `CustomerID`, `OrderDate`, `Status`) VALUES
(1, 1, '2026-05-01', 'Shipped'),
(2, 2, '2026-05-01', 'Pending'),
(3, 3, '2026-05-02', 'Pending'),
(4, 4, '2026-05-03', 'Shipped'),
(5, 5, '2026-05-03', 'Delivered'),
(6, 6, '2026-05-04', 'Pending'),
(7, 7, '2026-05-05', 'Shipped'),
(8, 8, '2026-05-06', 'Delivered'),
(9, 9, '2026-05-07', 'Pending'),
(10, 10, '2026-05-08', 'Shipped'),
(11, 11, '2026-05-09', 'Pending'),
(12, 12, '2026-05-10', 'Shipped'),
(13, 13, '2026-05-11', 'Delivered'),
(14, 14, '2026-05-12', 'Pending'),
(15, 15, '2026-05-12', 'Shipped'),
(16, 1, '2026-05-13', 'Delivered'),
(17, 3, '2026-05-14', 'Pending'),
(18, 5, '2026-05-14', 'Shipped'),
(19, 16, '2026-05-15', 'Delivered'),
(20, 17, '2026-05-16', 'Pending'),
(21, 18, '2026-05-17', 'Shipped'),
(22, 19, '2026-05-18', 'Delivered'),
(23, 20, '2026-05-19', 'Pending'),
(24, 2, '2026-05-19', 'Shipped'),
(25, 4, '2026-05-20', 'Delivered');

-- --------------------------------------------------------

--
-- Table structure for table `orderstatuslog`
--

CREATE TABLE `orderstatuslog` (
  `LogID` int(11) NOT NULL AUTO_INCREMENT,
  `OrderID` int(11) NOT NULL,
  `OldStatus` varchar(20) NOT NULL,
  `NewStatus` varchar(20) NOT NULL,
  `ChangedAt` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`LogID`),
  KEY `OrderID` (`OrderID`),
  CONSTRAINT `orderstatuslog_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Triggers `orders`
--
DELIMITER $$
CREATE TRIGGER `trg_AfterUpdateOrderStatus` AFTER UPDATE ON `orders` FOR EACH ROW BEGIN
    -- PURPOSE: 
    -- This trigger maintains an audit trail. If an order's status changes 
    -- (e.g., 'Pending' to 'Shipped'), it logs the old status, the new status, 
    -- and the timestamp into the OrderStatusLog table.
    IF OLD.Status != NEW.Status THEN
        INSERT INTO OrderStatusLog (OrderID, OldStatus, NewStatus)
        VALUES (NEW.OrderID, OLD.Status, NEW.Status);
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_BeforeDeleteOrder` BEFORE DELETE ON `orders` FOR EACH ROW BEGIN
    -- PURPOSE: 
    -- This trigger protects historical financial data. It prevents users 
    -- or applications from deleting an order if it has already been 
    -- marked as 'Shipped' or 'Delivered'.
    IF OLD.Status IN ('Shipped', 'Delivered') THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Error: Cannot delete an order that has already been shipped or delivered.';
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_BeforeUpdateOrderStatus` BEFORE UPDATE ON `orders` FOR EACH ROW BEGIN
    IF OLD.Status = 'Delivered' AND NEW.Status != 'Delivered' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Error: Cannot change the status of an order that is already Delivered.';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `ProductID` int(11) NOT NULL,
  `ProductName` varchar(100) NOT NULL,
  `CategoryID` int(11) DEFAULT NULL,
  `Price` decimal(10,2) NOT NULL,
  `StockQuantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`ProductID`, `ProductName`, `CategoryID`, `Price`, `StockQuantity`) VALUES
(1, 'Wireless Headphones', 1, 99.99, 45),
(2, 'Cotton T-Shirt', 2, 19.99, 200),
(3, 'Desk Lamp', 3, 35.50, 75),
(4, 'Yoga Mat', 4, 25.00, 100),
(5, 'Database Systems Textbook', 5, 85.00, 30),
(6, 'Lego Spaceship', 6, 49.99, 40),
(7, 'Moisturizing Cream', 7, 15.75, 120),
(8, 'Windshield Wipers', 8, 22.50, 60),
(9, 'Organic Coffee Beans', 9, 14.99, 150),
(10, 'Ergonomic Office Chair', 10, 199.99, 25),
(11, 'Dog Food 5kg', 11, 25.99, 100),
(12, 'Acoustic Guitar', 12, 150.00, 15),
(13, 'Inception Blu-ray', 13, 14.99, 50),
(14, 'Power Drill', 14, 89.99, 30),
(15, 'Silver Necklace', 15, 120.00, 20),
(16, 'Running Shoes', 4, 75.50, 60),
(17, 'Novel: The Great Gatsby', 5, 10.99, 100),
(18, 'Shampoo 500ml', 7, 8.50, 150),
(19, 'Car Wash Soap', 8, 12.00, 80),
(20, 'Almond Milk 1L', 9, 3.99, 200),
(21, 'Smartphone Case', 1, 15.00, 302),
(22, 'Winter Jacket', 2, 110.00, 40),
(23, 'Throw Pillow', 3, 18.50, 90),
(24, 'Board Game: Monopoly', 6, 22.99, 45),
(25, 'Printer Paper 500ct', 10, 9.99, 120);

--
-- Triggers `products`
--
DELIMITER $$
CREATE TRIGGER `trg_BeforeInsertProduct` BEFORE INSERT ON `products` FOR EACH ROW BEGIN
    IF NEW.Price < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Error: Product price cannot be negative.';
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_BeforeUpdateProductStock` BEFORE UPDATE ON `products` FOR EACH ROW BEGIN
    -- PURPOSE: 
    -- This trigger acts as a safety net for inventory. If an update 
    -- attempts to push a product's StockQuantity below 0, it aborts 
    -- the update. This prevents selling items we don't physically have.
    IF NEW.StockQuantity < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Error: Stock quantity cannot fall below zero.';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `RoleID` int(11) NOT NULL,
  `RoleName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`RoleID`, `RoleName`) VALUES
(1, 'Admin'),
(3, 'Customer'),
(2, 'Staff');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `RoleID` int(11) NOT NULL,
  `StatusID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserID`, `Username`, `Password`, `FirstName`, `LastName`, `Email`, `RoleID`, `StatusID`) VALUES
(1, 'jbuenosaires', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Jayson', 'Buenosaires', 'jaysonbuenosaires2@gmail.com', 1, 1),
(2, 'msantos', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Maria', 'Santos', 'msantos@ecommerce.local', 2, 1),
(3, 'jrizal', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Jose', 'Rizal', 'jrizal@ecommerce.local', 2, 1),
(4, 'abonifacio', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Andres', 'Bonifacio', 'abonifacio@ecommerce.local', 2, 1),
(5, 'eaguinaldo', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Emilio', 'Aguinaldo', 'eaguinaldo@ecommerce.local', 2, 1),
(6, 'amabini', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Apolinario', 'Mabini', 'amabini@ecommerce.local', 2, 2),
(7, 'SarahDuterte', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Sarah', 'Duterte', 'SarahDuterte@ecommerce.local', 3, 1),
(8, 'gsilang', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Gabriela', 'Silang', 'gsilang@ecommerce.local', 3, 1),
(9, 'maquino', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Melchora', 'Aquino', 'maquino@ecommerce.local', 3, 1),
(10, 'aluna', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Antonio', 'Luna', 'aluna@ecommerce.local', 3, 1),
(11, 'gdelpilar', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Gregorio', 'Del Pilar', 'gdelpilar@ecommerce.local', 3, 2),
(12, 'ejacinto', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Emilio', 'Jacinto', 'ejacinto@ecommerce.local', 3, 1),
(13, 'llapu', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Lapu', 'Lapu', 'llapu@ecommerce.local', 3, 1),
(14, 'tmagbanua', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Teresa', 'Magbanua', 'tmagbanua@ecommerce.local', 3, 1),
(15, 'jllanes', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Josefa', 'Llanes', 'jllanes@ecommerce.local', 3, 1),
(16, 'dsilang', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Diego', 'Silang', 'dsilang@ecommerce.local', 3, 1),
(17, 'newCustomer', '548d8cf86e2d301f6e1f5dc621cba2e409e8e814ba35ca1feeff6b0b995d848f', 'Test', 'Customer', 'newcustomer@gmail.com', 3, 1);

-- --------------------------------------------------------

--
-- Table structure for table `user_status`
--

CREATE TABLE `user_status` (
  `StatusID` int(11) NOT NULL,
  `StatusName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `user_status`
--

INSERT INTO `user_status` (`StatusID`, `StatusName`) VALUES
(1, 'Active'),
(2, 'Inactive');

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_customerordersummary`
-- (See below for the actual view)
--
CREATE TABLE `vw_customerordersummary` (
`CustomerID` int(11)
,`FirstName` varchar(50)
,`LastName` varchar(50)
,`OrderID` int(11)
,`OrderDate` date
,`Status` varchar(20)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_invoicedetails`
-- (See below for the actual view)
--
CREATE TABLE `vw_invoicedetails` (
`OrderID` int(11)
,`ProductName` varchar(100)
,`Quantity` int(11)
,`UnitPrice` decimal(10,2)
,`LineTotal` decimal(20,2)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `vw_productcatalog`
-- (See below for the actual view)
--
CREATE TABLE `vw_productcatalog` (
`ProductID` int(11)
,`ProductName` varchar(100)
,`CategoryName` varchar(50)
,`Price` decimal(10,2)
,`StockQuantity` int(11)
);

-- --------------------------------------------------------

--
-- Structure for view `vw_customerordersummary`
--
DROP TABLE IF EXISTS `vw_customerordersummary`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_customerordersummary`  AS SELECT `c`.`CustomerID` AS `CustomerID`, `c`.`FirstName` AS `FirstName`, `c`.`LastName` AS `LastName`, `o`.`OrderID` AS `OrderID`, `o`.`OrderDate` AS `OrderDate`, `o`.`Status` AS `Status` FROM (`customers` `c` join `orders` `o` on(`c`.`CustomerID` = `o`.`CustomerID`)) ;

-- --------------------------------------------------------

--
-- Structure for view `vw_invoicedetails`
--
DROP TABLE IF EXISTS `vw_invoicedetails`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_invoicedetails`  AS SELECT `od`.`OrderID` AS `OrderID`, `p`.`ProductName` AS `ProductName`, `od`.`Quantity` AS `Quantity`, `od`.`UnitPrice` AS `UnitPrice`, `od`.`Quantity`* `od`.`UnitPrice` AS `LineTotal` FROM (`orderdetails` `od` join `products` `p` on(`od`.`ProductID` = `p`.`ProductID`)) ;

-- --------------------------------------------------------

--
-- Structure for view `vw_productcatalog`
--
DROP TABLE IF EXISTS `vw_productcatalog`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_productcatalog`  AS SELECT `p`.`ProductID` AS `ProductID`, `p`.`ProductName` AS `ProductName`, `c`.`CategoryName` AS `CategoryName`, `p`.`Price` AS `Price`, `p`.`StockQuantity` AS `StockQuantity` FROM (`products` `p` join `categories` `c` on(`p`.`CategoryID` = `c`.`CategoryID`)) ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `categories`
--
ALTER TABLE `categories`
  ADD PRIMARY KEY (`CategoryID`);

--
-- Indexes for table `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`CustomerID`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indexes for table `orderdetails`
--
ALTER TABLE `orderdetails`
  ADD PRIMARY KEY (`OrderDetailID`),
  ADD KEY `OrderID` (`OrderID`),
  ADD KEY `ProductID` (`ProductID`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`OrderID`),
  ADD KEY `CustomerID` (`CustomerID`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`ProductID`),
  ADD KEY `CategoryID` (`CategoryID`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`RoleID`),
  ADD UNIQUE KEY `RoleName` (`RoleName`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `Username` (`Username`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `RoleID` (`RoleID`),
  ADD KEY `StatusID` (`StatusID`);

--
-- Indexes for table `user_status`
--
ALTER TABLE `user_status`
  ADD PRIMARY KEY (`StatusID`),
  ADD UNIQUE KEY `StatusName` (`StatusName`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `categories`
--
ALTER TABLE `categories`
  MODIFY `CategoryID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `customers`
--
ALTER TABLE `customers`
  MODIFY `CustomerID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `orderdetails`
--
ALTER TABLE `orderdetails`
  MODIFY `OrderDetailID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `OrderID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `ProductID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `roles`
--
ALTER TABLE `roles`
  MODIFY `RoleID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT for table `user_status`
--
ALTER TABLE `user_status`
  MODIFY `StatusID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `orderdetails`
--
ALTER TABLE `orderdetails`
  ADD CONSTRAINT `orderdetails_ibfk_1` FOREIGN KEY (`OrderID`) REFERENCES `orders` (`OrderID`),
  ADD CONSTRAINT `orderdetails_ibfk_2` FOREIGN KEY (`ProductID`) REFERENCES `products` (`ProductID`);

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`CustomerID`) REFERENCES `customers` (`CustomerID`);

--
-- Constraints for table `products`
--
ALTER TABLE `products`
  ADD CONSTRAINT `products_ibfk_1` FOREIGN KEY (`CategoryID`) REFERENCES `categories` (`CategoryID`);

--
-- Constraints for table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`RoleID`) REFERENCES `roles` (`RoleID`),
  ADD CONSTRAINT `users_ibfk_2` FOREIGN KEY (`StatusID`) REFERENCES `user_status` (`StatusID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
