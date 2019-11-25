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


-- Dumping database structure for HangfireRegistry
CREATE DATABASE IF NOT EXISTS `HangfireRegistry` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */;
USE `HangfireRegistry`;

-- Dumping structure for table HangfireRegistry.HangFire_AggregatedCounter
CREATE TABLE IF NOT EXISTS `HangFire_AggregatedCounter` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` int(11) NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_CounterAggregated_Key` (`Key`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_Counter
CREATE TABLE IF NOT EXISTS `HangFire_Counter` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` int(11) NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Counter_Key` (`Key`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_DistributedLock
CREATE TABLE IF NOT EXISTS `HangFire_DistributedLock` (
  `Resource` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_Hash
CREATE TABLE IF NOT EXISTS `HangFire_Hash` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Field` varchar(40) NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Hash_Key_Field` (`Key`,`Field`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_Job
CREATE TABLE IF NOT EXISTS `HangFire_Job` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `StateId` int(11) DEFAULT NULL,
  `StateName` varchar(20) DEFAULT NULL,
  `InvocationData` longtext NOT NULL,
  `Arguments` longtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Job_StateName` (`StateName`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_JobParameter
CREATE TABLE IF NOT EXISTS `HangFire_JobParameter` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Name` varchar(40) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_JobParameter_JobId_Name` (`JobId`,`Name`),
  KEY `FK_JobParameter_Job` (`JobId`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_JobQueue
CREATE TABLE IF NOT EXISTS `HangFire_JobQueue` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Queue` varchar(50) NOT NULL,
  `FetchedAt` datetime DEFAULT NULL,
  `FetchToken` varchar(36) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_JobQueue_QueueAndFetchedAt` (`Queue`,`FetchedAt`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_JobState
CREATE TABLE IF NOT EXISTS `HangFire_JobState` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Reason` varchar(100) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_JobState_Job` (`JobId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_List
CREATE TABLE IF NOT EXISTS `HangFire_List` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_Server
CREATE TABLE IF NOT EXISTS `HangFire_Server` (
  `Id` varchar(100) NOT NULL,
  `Data` longtext NOT NULL,
  `LastHeartbeat` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_Set
CREATE TABLE IF NOT EXISTS `HangFire_Set` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) NOT NULL,
  `Value` varchar(256) NOT NULL,
  `Score` float NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Set_Key_Value` (`Key`,`Value`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table HangfireRegistry.HangFire_State
CREATE TABLE IF NOT EXISTS `HangFire_State` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `JobId` int(11) NOT NULL,
  `Name` varchar(20) NOT NULL,
  `Reason` varchar(100) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangFire_State_Job` (`JobId`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping database structure for NPMRegistryClone
CREATE DATABASE IF NOT EXISTS `NPMRegistryClone` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */;
USE `NPMRegistryClone`;

-- Dumping structure for table NPMRegistryClone.OrgDepartments
CREATE TABLE IF NOT EXISTS `OrgDepartments` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` char(250) COLLATE utf8mb4_bin NOT NULL DEFAULT 'Company',
  `OrgDesc` text COLLATE utf8mb4_bin,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

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
) ENGINE=InnoDB AUTO_INCREMENT=1302 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

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
  `DisplayName` char(250) COLLATE utf8mb4_bin DEFAULT 'Username',
  `OrgDepartment` int(10) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `DisplayName` (`DisplayName`),
  KEY `FK_Users_OrgDepartments` (`OrgDepartment`),
  CONSTRAINT `FK_Users_OrgDepartments` FOREIGN KEY (`OrgDepartment`) REFERENCES `OrgDepartments` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- Data exporting was unselected.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
