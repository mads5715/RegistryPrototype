-- --------------------------------------------------------
-- Host:                         debian-server.local
-- Server version:               5.7.25 - MySQL Community Server (GPL)
-- Server OS:                    Linux
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for NPMRegistryClone
CREATE DATABASE IF NOT EXISTS `NPMRegistryClone` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */;
USE `NPMRegistryClone`;

-- Dumping structure for table NPMRegistryClone.OrgDepartments
CREATE TABLE IF NOT EXISTS `OrgDepartments` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` char(250) COLLATE utf8mb4_bin NOT NULL DEFAULT 'Company',
  `OrgDesc` text COLLATE utf8mb4_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Data exporting was unselected.

-- Dumping structure for table NPMRegistryClone.Packages
CREATE TABLE IF NOT EXISTS `Packages` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `_ID` varchar(250) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Name` varchar(250) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Filename` mediumtext CHARACTER SET utf8mb4,
  `DistTags` json DEFAULT NULL,
  `Versions` json DEFAULT NULL,
  `Modified` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `PackageDescription` mediumtext CHARACTER SET utf8mb4,
  `Author` varchar(150) CHARACTER SET utf8mb4 DEFAULT NULL,
  `RawMetaData` json DEFAULT NULL,
  `IsPublic` bit(1) DEFAULT NULL,
  `IsSecure` bit(1) DEFAULT NULL,
  `IsFromPublicRepo` bit(1) DEFAULT NULL,
  `CanBeServedFromServer` bit(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `_ID` (`_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1625 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Data exporting was unselected.

-- Dumping structure for table NPMRegistryClone.UserPackages
CREATE TABLE IF NOT EXISTS `UserPackages` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) DEFAULT NULL,
  `PackageID` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK__Users` (`UserID`),
  KEY `FK__Packages` (`PackageID`),
  CONSTRAINT `FK__Packages` FOREIGN KEY (`PackageID`) REFERENCES `Packages` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `FK__Users` FOREIGN KEY (`UserID`) REFERENCES `Users` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Data exporting was unselected.

-- Dumping structure for table NPMRegistryClone.Users
CREATE TABLE IF NOT EXISTS `Users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` char(250) COLLATE utf8mb4_bin DEFAULT 'Username',
  `UserPassword` char(255) COLLATE utf8mb4_bin DEFAULT 'Password',
  `UserType` int(11) DEFAULT NULL,
  `DisplayName` char(250) COLLATE utf8mb4_bin DEFAULT 'Username',
  `OrgDepartment` int(10) DEFAULT NULL,
  `Email` varchar(250) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `DisplayName` (`DisplayName`),
  KEY `FK_Users_OrgDepartments` (`OrgDepartment`),
  KEY `FK_Users_UserTypes` (`UserType`),
  CONSTRAINT `FK_Users_OrgDepartments` FOREIGN KEY (`OrgDepartment`) REFERENCES `OrgDepartments` (`ID`),
  CONSTRAINT `FK_Users_UserTypes` FOREIGN KEY (`UserType`) REFERENCES `UserTypes` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Data exporting was unselected.

-- Dumping structure for table NPMRegistryClone.UserTypes
CREATE TABLE IF NOT EXISTS `UserTypes` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(250) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Data exporting was unselected.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
