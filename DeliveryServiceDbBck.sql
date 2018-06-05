CREATE DATABASE  IF NOT EXISTS `deliveryservice` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;
USE `deliveryservice`;
-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: deliveryservice
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `points`
--

DROP TABLE IF EXISTS `points`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `points` (
  `POINT_ID` double NOT NULL AUTO_INCREMENT,
  `POINT_NAME` varchar(255) NOT NULL,
  PRIMARY KEY (`POINT_ID`),
  UNIQUE KEY `NAME_UNIQUE` (`POINT_NAME`),
  UNIQUE KEY `POINT_ID_UNIQUE` (`POINT_ID`),
  KEY `idx_points_ID` (`POINT_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `points`
--

LOCK TABLES `points` WRITE;
/*!40000 ALTER TABLE `points` DISABLE KEYS */;
INSERT INTO `points` (`POINT_ID`, `POINT_NAME`) VALUES (1,'A'),(2,'B'),(3,'C'),(4,'D'),(5,'E'),(6,'F'),(7,'G'),(8,'H'),(9,'I');
/*!40000 ALTER TABLE `points` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `routes`
--

DROP TABLE IF EXISTS `routes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `routes` (
  `ROUTE_ID` double NOT NULL AUTO_INCREMENT,
  `ROUTE_COST` float NOT NULL,
  `ROUTE_TIME` float NOT NULL,
  `START_POINT_ID` double NOT NULL,
  `END_POINT_ID` double NOT NULL,
  PRIMARY KEY (`ROUTE_ID`),
  UNIQUE KEY `ROUTE_ID_UNIQUE` (`ROUTE_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `routes`
--

LOCK TABLES `routes` WRITE;
/*!40000 ALTER TABLE `routes` DISABLE KEYS */;
INSERT INTO `routes` (`ROUTE_ID`, `ROUTE_COST`, `ROUTE_TIME`, `START_POINT_ID`, `END_POINT_ID`) VALUES (1,20,1,1,3),(2,5,30,1,5),(3,1,10,1,8),(4,12,1,3,2),(5,1,30,8,5),(6,5,3,5,4),(7,50,4,4,6),(8,50,40,6,7),(9,50,45,6,9),(10,73,64,7,2),(11,5,65,9,2);
/*!40000 ALTER TABLE `routes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-06-05 10:37:29
